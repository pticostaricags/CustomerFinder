using LinqToTwitter;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using DKPReq = CustomerFinderWorkerRole.CognitiveServices.TextAnalytics.KeyPhrases.Request;
using DKPResp = CustomerFinderWorkerRole.CognitiveServices.TextAnalytics.KeyPhrases.Response;
namespace CustomerFinderWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public static string Twitter_Username
        {
            get
            {
                return CloudConfigurationManager.GetSetting("Twitter_Username");
            }
        }

        public static string TwitterApp_ConsumerKey
        {
            get
            {
                return CloudConfigurationManager.GetSetting("TwitterApp_ConsumerKey");
            }
        }
        public static string TwitterApp_ConsumerSecret
        {
            get
            {
                return CloudConfigurationManager.GetSetting("TwitterApp_ConsumerSecret");
            }
        }
        public static string TwitterApp_AccessToken
        {
            get
            {
                return CloudConfigurationManager.GetSetting("TwitterApp_AccessToken");
            }
        }
        public static string TwitterApp_AccessTokenSecret
        {
            get
            {
                return CloudConfigurationManager.GetSetting("TwitterApp_AccessTokenSecret");
            }
        }

        public static string TextAnaliticsKey
        {
            get
            {
                return CloudConfigurationManager.GetSetting("TextAnaliticsKey");
            }
        }

        public static string Notifications_ToEmailAddress
        {
            get
            {
                return CloudConfigurationManager.GetSetting("Notifications_ToEmailAddress");
            }
        }

        public override void Run()
        {
            Trace.TraceInformation("CustomerFinderWorkerRole is running");
            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            catch (System.AggregateException aggrEx)
            {
                ProcessError(Twitter_Username, aggrEx);
            }
            catch (Exception ex)
            {
                SendEmail("Fatal error ocurred", ex.ToString());
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("CustomerFinderWorkerRole has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("CustomerFinderWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("CustomerFinderWorkerRole has stopped");
        }

        private static TwitterContext _ctx = null;
        private static LinqToTwitter.IAuthorizer auth = null;

        private IAuthorizer GetAuthorizerFor(string usernamechecking)
        {
            LinqToTwitter.IAuthorizer singleUserAuth = null;
            if (lstAuthorizers.Where(p => p.CredentialStore.ScreenName == usernamechecking).Count() == 0)
            {
                using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                {
                    var aspNetUser = dbCtx.AspNetUsers.Where(p => p.TwitterUsername == usernamechecking).FirstOrDefault();
                    if (aspNetUser != null)
                    {
                        var aspNetUserClaims = aspNetUser.AspNetUserClaims;
                        string singleUserOAuthToken = aspNetUserClaims.Where(p => p.ClaimType == "TwitterAccessToken").Select(p => p.ClaimValue).FirstOrDefault();
                        string singleUserOAuthTokenSecret = aspNetUserClaims.Where(p => p.ClaimType == "TwitterAccessTokenSecret").Select(p => p.ClaimValue).FirstOrDefault();
                        if (!string.IsNullOrWhiteSpace(singleUserOAuthToken) && !string.IsNullOrWhiteSpace(singleUserOAuthTokenSecret))
                        {
                            singleUserAuth = new SingleUserAuthorizer()
                            {
                                CredentialStore = new LinqToTwitter.SingleUserInMemoryCredentialStore()
                                {
                                    ConsumerKey = TwitterApp_ConsumerKey,
                                    ConsumerSecret = TwitterApp_ConsumerSecret,
                                    OAuthToken = singleUserOAuthToken,
                                    OAuthTokenSecret = singleUserOAuthTokenSecret,
                                    ScreenName = usernamechecking
                                },
                            };
                            lstAuthorizers.Add(singleUserAuth);
                        }
                    }
                }
            }
            else
                singleUserAuth = lstAuthorizers.Where(p => p.CredentialStore.ScreenName == usernamechecking).FirstOrDefault();
            if (singleUserAuth == null)
            {
                //if we cannot find an OAuth Tokens for user, we will use application authorized
                singleUserAuth = new LinqToTwitter.ApplicationOnlyAuthorizer()
                {
                    CredentialStore = new LinqToTwitter.InMemoryCredentialStore()
                    {
                        ConsumerKey = TwitterApp_ConsumerKey,
                        ConsumerSecret = TwitterApp_ConsumerSecret
                    }
                };
            }
            return singleUserAuth;
        }

        private List<LinqToTwitter.IAuthorizer> lstAuthorizers = new List<IAuthorizer>();

        private async static Task<TwitterContext> GetTwitterContext()
        {
            if (_ctx == null)
            {
                _ctx = new TwitterContext(auth);
            }
            if (_ctx.RateLimitCurrent < 0 || _ctx.RateLimitRemaining < 0 || _ctx.RateLimitReset < 0)
            {
                await auth.AuthorizeAsync();
            }
            await Task.Yield();
            return _ctx;
        }

        private async Task<int> CheckUnFollowers(string username, Guid sessionId)
        {
            bool checkParam = true;
            long cursor = -1;
            int pasadas = 0;
            bool continueCheckingFollowers = true;
            do
            {
                LinqToTwitter.TwitterContext ctx = await GetTwitterContext();
                try
                {
                    long? twitterAccountsId = null;
                    if (checkParam)
                    {
                        User userToAdd = null;
                        try
                        {
                            userToAdd = ctx.User.Where(p => p.ScreenNameList == username && p.Type == UserType.Lookup).FirstOrDefault();
                        }
                        catch (AggregateException aggrAddEx)
                        {
                            ProcessError(username, aggrAddEx);
                        }
                        if (userToAdd != null)
                        {
                            twitterAccountsId = AddTwitterAccount(userToAdd);
                            if (twitterAccountsId.HasValue)
                            {
                                using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                                {
                                    await ProcessUser(userToAdd.ScreenNameResponse, sessionId, dbCtx, userToAdd, twitterAccountsId.Value);
                                    var nextCursorTuple = dbCtx.TwitterFollowerCheckProgress.Where(p => p.TwitterAccountsId == twitterAccountsId).FirstOrDefault();
                                    if (nextCursorTuple != null)
                                    {
                                        cursor = nextCursorTuple.NextCursor;
                                        CheckIfResetFollowerScan(ref cursor);
                                    }
                                }
                            }
                            checkParam = false;
                        }
                        else
                        {
                            return pasadas;
                        }
                    }
                    Random rnd = new Random();
                    var followerIDs = await ctx.Friendship.Where(p => p.Type == FriendshipType.FollowersList && p.ScreenName == username && p.Cursor == cursor)
                        .OrderBy(p => rnd.Next()).SingleOrDefaultAsync();
                    using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                    {
                        int usersProcessed = 0;
                        List<User> lstUsers = followerIDs.Users;
                        if (IsMultiClientService())
                        {
                            lstUsers = lstUsers.Take(1).ToList();
                        }
                        foreach (var v in lstUsers)
                        {
                            try
                            {
                                if (v.Protected == false)
                                {
                                    long? dbUserId = AddTwitterAccount(v);
                                    await ProcessUser(username, sessionId, dbCtx, v, dbUserId);
                                    bool inserRecord = false;
                                    CustomerFinderDA.Follower follower =
                                        dbCtx.Followers.Where(p => p.UserFollowed == username && p.UserFollowing == v.ScreenNameResponse).FirstOrDefault();
                                    if (follower == null)
                                    {
                                        follower = new CustomerFinderDA.Follower();
                                        inserRecord = true;
                                    }
                                    follower.LastTimeSeenFollowing = DateTimeOffset.UtcNow;
                                    follower.SessionId = sessionId;
                                    follower.UserFollowed = username;
                                    follower.UserFollowing = v.ScreenNameResponse;
                                    if (inserRecord)
                                        dbCtx.Followers.Add(follower);
                                }
                            }
                            catch (AggregateException aggrExSingleFollower)
                            {
                                ProcessError(v.ScreenNameResponse, aggrExSingleFollower);
                            }
                            finally
                            {
                                if (ctx.RateLimitRemaining == 0)
                                    await CheckTwitterLimit(auth, ctx, sessionId, v.ScreenNameResponse);
                                usersProcessed++;
                            }
                            if (IsMultiClientService() && usersProcessed > 0)
                                break;
                        }
                        dbCtx.SaveChanges();
                    }
                    if (followerIDs != null && followerIDs.CursorMovement != null)
                    {
                        cursor = followerIDs.CursorMovement.Next;
                        CheckIfResetFollowerScan(ref cursor);
                        continueCheckingFollowers = false;
                        using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                        {
                            var nextCursorTuple = dbCtx.TwitterFollowerCheckProgress.Where(p => p.TwitterAccountsId == twitterAccountsId).FirstOrDefault();
                            if (nextCursorTuple == null)
                            {
                                nextCursorTuple = new CustomerFinderDA.TwitterFollowerCheckProgress();
                                nextCursorTuple.TwitterAccountsId = twitterAccountsId.Value;
                                nextCursorTuple.NextCursor = cursor;
                                dbCtx.TwitterFollowerCheckProgress.Add(nextCursorTuple);
                            }
                            else
                            {
                                nextCursorTuple.NextCursor = cursor;
                            }
                            dbCtx.SaveChanges();
                        }
                    }
                    else
                        continueCheckingFollowers = false;
                    pasadas++;
                }
                catch (AggregateException aggrEx)
                {
                    ProcessError(username, aggrEx);
                    continueCheckingFollowers = false;
                }
                catch (Exception ex)
                {
                    continueCheckingFollowers = false;
                    SendEmail("An error has ocurred on 'UnfollowCheatScanCloudService' at " + DateTimeOffset.UtcNow, ex.ToString());
                    if (ex.ToString().IndexOf("Rate limit exceeded ") >= 0)
                    {
                        await CheckTwitterLimit(auth, ctx, sessionId, username);
                    }
                }
                finally
                {
                    if (ctx.RateLimitRemaining == 0)
                    {
                        await CheckTwitterLimit(auth, ctx, sessionId, username);
                    }
                }
            } while (continueCheckingFollowers);
            //CHECK UNFOLLOWERS
            //using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
            //{
            //    var unfollowers = dbCtx.Followers.Where(p => p.SessionId != sessionId && p.UserFollowed == username);
            //    foreach (var singleUnfollower in unfollowers)
            //    {
            //        CustomerFinderDA.UnFollower unfollower = new CustomerFinderDA.UnFollower();
            //        unfollower.LastTimeSeenUnFollowing = DateTimeOffset.UtcNow;
            //        unfollower.SessionId = sessionId;
            //        unfollower.UserUnFollowed = username;
            //        unfollower.UserUnFollowing = singleUnfollower.UserFollowing;
            //        dbCtx.UnFollowers.Add(unfollower);
            //        dbCtx.Followers.Remove(singleUnfollower);
            //    }
            //    dbCtx.SaveChanges();
            //}

            return pasadas;
        }

        private void CheckIfResetFollowerScan(ref long cursor)
        {
            //when cursos is equal to 0 we have scan all friend
            if (cursor == 0)
                cursor = -1;
        }

        private async Task ProcessUser(string username, Guid sessionId, CustomerFinderDA.CustomerFinderContext dbCtx, User v,
            long? dbUserId)
        {
            TwitterContext ctx = await GetTwitterContext();
            if (ctx.RateLimitRemaining == 0)
                await CheckTwitterLimit(auth, ctx, sessionId, username);
            if (dbUserId.HasValue && v.Protected == false)
            {
                DateTimeOffset todaysInitialTime = DateTime.Today.ToUniversalTime();
                var lastTimeUserTweetsProcessed = dbCtx.TwitterAccounts.Where(p => p.TwitterAccountsId == dbUserId.Value).Select(p => p.LastTimeTweetsProcessed).FirstOrDefault();
                if (!lastTimeUserTweetsProcessed.HasValue || lastTimeUserTweetsProcessed.HasValue && todaysInitialTime > lastTimeUserTweetsProcessed.Value.AddDays(1))
                {
                    await ProcessUserStatuses(dbCtx, v, dbUserId, ctx);
                }
                else
                {
                    Debug.WriteLine(string.Format("User: {0} already checked today", v.ScreenNameResponse));
                }
                if (ctx.RateLimitRemaining == 0)
                    await CheckTwitterLimit(auth, ctx, sessionId, username);
            }
        }

        private async Task ProcessUserStatuses(CustomerFinderDA.CustomerFinderContext dbCtx, User v, long? dbUserId, TwitterContext ctx)
        {
            //check https://github.com/JoeMayo/LinqToTwitter/wiki/Querying-the-User-Timeline
            var userLast20Tweets = ctx.Status.Where(p => p.Type == StatusType.User && p.ScreenName == v.ScreenNameResponse && p.StatusID != 0).ToList();
            if (userLast20Tweets != null && userLast20Tweets.Count > 0)
            {
                List<CustomerFinderDA.TwitterUserStatus> lstTwitterUserStatus = new List<CustomerFinderDA.TwitterUserStatus>();
                string[] tweetsIds =
                    userLast20Tweets.Select(p => Convert.ToString(p.StatusID)).ToArray();
                var existentTweetIds = dbCtx.TwitterUserStatuses.Where(p => tweetsIds.Contains(p.TweetId)).Select(p => p.TweetId).ToList();

                foreach (var singleTweet in userLast20Tweets)
                {
                    string strTweetId = Convert.ToString(singleTweet.StatusID);
                    //we must avoid duplicated statuses
                    if (existentTweetIds == null ||
                        (existentTweetIds != null && !existentTweetIds.Contains(strTweetId)))
                    {
                        CustomerFinderDA.TwitterUserStatus userStatus = new CustomerFinderDA.TwitterUserStatus();
                        userStatus.TwitterAccountsId = dbUserId.Value;
                        userStatus.StatusText = singleTweet.Text;
                        userStatus.CreatedAt = singleTweet.CreatedAt.ToUniversalTime();
                        string tweetUrl = string.Format("https://www.twitter.com/{0}/status/{1}", v.ScreenNameResponse, singleTweet.StatusID);
                        userStatus.StatusUrl = tweetUrl;
                        userStatus.StatusCount = singleTweet.Count;
                        userStatus.FavoriteCount = singleTweet.FavoriteCount;
                        userStatus.RetweetCount = singleTweet.RetweetCount;
                        userStatus.TweetId = strTweetId;
                        dbCtx.TwitterUserStatuses.Add(userStatus);
                        if (lstTwitterUserStatus.Count == 0)
                            lstTwitterUserStatus.Add(userStatus);
                    }
                    else
                    {
                        Debug.WriteLine("Tweet {0} already exist", new object[] { singleTweet.StatusID });
                    }
                }
                var twitterAccountEntity = dbCtx.TwitterAccounts.Where(p => p.TwitterAccountsId == dbUserId.Value).Single();
                twitterAccountEntity.LastTimeTweetsProcessed = DateTimeOffset.Now;
                dbCtx.SaveChanges();
                try
                {
                    CheckForTextAnalyticsReset();
                    if (!TextAnalyticsLimitFoundAt.HasValue || TextAnalyticsLimitFoundAt.HasValue && DateTimeOffset.Now > TextAnalyticsLimitFoundAt.Value.AddMonths(1))
                    {
                        if (!MaxDailyTextanalyticsTransactionsReached())
                            await PerformTextAnalytics(twitterAccountsId: dbUserId.Value, lstTwitterUserStatus: lstTwitterUserStatus);
                    }
                }
                catch (AggregateException aggrEx)
                {
                    //ignire meanwhile
                }
                catch (Exception ex)
                {
                    //ignore meanwhile
                }
            }
        }

        private bool MaxDailyTextanalyticsTransactionsReached()
        {
            bool result = false;
            int totalPerMonth = 5000;
            //int maxDailyAllowed = totalPerMonth / 31;
            int maxDailyAllowed = 5000;
            int todaysTransactionCount = 0;
            using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
            {
                var todaysUTC = DateTime.Today.ToUniversalTime();
                todaysTransactionCount = ctx.TextAnalyticsTransaction.Where(p => p.CreatedAt >= todaysUTC).Count();
            }
            result = (todaysTransactionCount >= maxDailyAllowed);
            return result;
        }

        private const string TEXTANALYTICS_BASE_URL = "https://southcentralus.api.cognitive.microsoft.com/text/analytics/v2.0/";
        private const string TEXTANALYTICS_LANGUAGES_URL = TEXTANALYTICS_BASE_URL + "languages";
        private const string TEXTANALYTICS_SENTIMENT_URL = TEXTANALYTICS_BASE_URL + "sentiment";
        private const string TEXTANALYTICS_KEYPHRASES_URL = TEXTANALYTICS_BASE_URL + "keyPhrases";

        private async Task PerformTextAnalytics(long twitterAccountsId,
            List<CustomerFinderDA.TwitterUserStatus> lstTwitterUserStatus)
        {
            #region Detect Languages
            using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
            {
                CognitiveServices.TextAnalytics.DetectLanguage.Request.DetectLanguageRequest dlRequest =
                    new CognitiveServices.TextAnalytics.DetectLanguage.Request.DetectLanguageRequest()
                    {
                        documents =
                        lstTwitterUserStatus.Select(p => new CognitiveServices.TextAnalytics.DetectLanguage.Request.Document
                        {
                            id = p.TwitterUserStatusId.ToString(),
                            text = p.StatusText

                        }).ToArray()
                    };
                string dljsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(dlRequest);
                System.Net.Http.StringContent content = new System.Net.Http.StringContent(dljsonRequest);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                content.Headers.Add("Ocp-Apim-Subscription-Key", TextAnaliticsKey);
                var response = await client.PostAsync(TEXTANALYTICS_LANGUAGES_URL, content);
                if (response.IsSuccessStatusCode == false)
                {
                    if (response.ReasonPhrase.Contains("Quota Exceeded"))
                    {
                        SetTextAnalyticsApiLimit(DateTimeOffset.Now);
                    }
                }
                else
                {
                    AddTextAnalyticsTransaction(transactionType: TRANSACTION_DETECTLANGUAGE);
                    var responseContent = await response.Content.ReadAsStringAsync();
                    CognitiveServices.TextAnalytics.DetectLanguage.Response.DetectLanguageResponse
                        dlResponse =
                        Newtonsoft.Json.JsonConvert.DeserializeObject<CognitiveServices.TextAnalytics.DetectLanguage.Response.DetectLanguageResponse>(responseContent);
                    using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                    {
                        foreach (var singleDocument in dlResponse.documents)
                        {
                            foreach (var detectedLanguage in singleDocument.detectedLanguages)
                            {
                                CustomerFinderDA.TwitterUserStatusLanguage dlEntity = new CustomerFinderDA.TwitterUserStatusLanguage()
                                {
                                    iso6391Name = detectedLanguage.iso6391Name,
                                    Name = detectedLanguage.name,
                                    score = detectedLanguage.score,
                                    TwitterUserStatusId = Convert.ToInt64(singleDocument.id)
                                };
                                dbCtx.TwitterUserStatusLanguage.Add(dlEntity);
                            }
                        }
                        dbCtx.SaveChanges();
                    }
                    CognitiveServices.TextAnalytics.Sentiment.Request.SentimentRequest sentimentRequest =
                    new CognitiveServices.TextAnalytics.Sentiment.Request.SentimentRequest()
                    {
                        documents = dlResponse.documents.Select(p => new CognitiveServices.TextAnalytics.Sentiment.Request.Document
                        {
                            id = p.id,
                            language = p.detectedLanguages.First().iso6391Name,
                            text = dlRequest.documents.Where(p1 => p1.id == p.id).Select(x => x.text).FirstOrDefault()
                        }).ToArray()
                    };
                    string sentimentjsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(sentimentRequest);
                    content = new System.Net.Http.StringContent(sentimentjsonRequest);
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    content.Headers.Add("Ocp-Apim-Subscription-Key", TextAnaliticsKey);
                    var sentimentResponse = await client.PostAsync(TEXTANALYTICS_SENTIMENT_URL,
                        content);
                    if (sentimentResponse.IsSuccessStatusCode == false)
                    {
                        if (sentimentResponse.ReasonPhrase.Contains("Quota Exceeded"))
                        {
                            SetTextAnalyticsApiLimit(DateTimeOffset.Now);
                        }
                    }
                    else
                    {
                        AddTextAnalyticsTransaction(transactionType: TRANSACTION_DETECTSENTIMENT);
                        var sentimentjsonResponse = await sentimentResponse.Content.ReadAsStringAsync();
                        CognitiveServices.TextAnalytics.Sentiment.Response.SentimentResponse
                            sentimentObjResponse =
                            Newtonsoft.Json.JsonConvert.DeserializeObject<CognitiveServices.TextAnalytics.Sentiment.Response.SentimentResponse>
                            (sentimentjsonResponse);
                        using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                        {
                            foreach (var singleSentimentDocument in sentimentObjResponse.documents)
                            {
                                CustomerFinderDA.TwitterUserStatusSentiment sentimentEntity = new CustomerFinderDA.TwitterUserStatusSentiment();
                                sentimentEntity.score = singleSentimentDocument.score;
                                sentimentEntity.TwitterUserStatusId = Convert.ToInt64(singleSentimentDocument.id);
                                dbCtx.TwitterUserStatusSentiment.Add(sentimentEntity);
                            }
                            dbCtx.SaveChanges();
                        }
                        DKPReq.DetectKeyPhrasesRequest dkpReq = new CognitiveServices.TextAnalytics.KeyPhrases.Request.DetectKeyPhrasesRequest()
                        {
                            documents = dlResponse.documents.Select(p => new DKPReq.Document
                            {
                                id = p.id,
                                language = p.detectedLanguages.First().iso6391Name,
                                text = dlRequest.documents.Where(p1 => p1.id == p.id).Select(x => x.text).FirstOrDefault()
                            }).ToArray()
                        };
                        string dkpjsonRequest = Newtonsoft.Json.JsonConvert.SerializeObject(dkpReq);
                        content = new System.Net.Http.StringContent(dkpjsonRequest);
                        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                        content.Headers.Add("Ocp-Apim-Subscription-Key", TextAnaliticsKey);
                        var dkpResponse = await client.PostAsync(TEXTANALYTICS_KEYPHRASES_URL,
                            content);
                        if (dkpResponse.IsSuccessStatusCode == false)
                        {
                            if (dkpResponse.ReasonPhrase.Contains("Quota Exceeded"))
                            {
                                SetTextAnalyticsApiLimit(DateTimeOffset.Now);
                            }
                        }
                        else
                        {
                            AddTextAnalyticsTransaction(transactionType: TRANSACTION_DETECTKEYPHRASES);
                            string dkpjsonResponse = await dkpResponse.Content.ReadAsStringAsync();
                            DKPResp.DetectKeyPhrasesResponse dkpRespObj =
                                Newtonsoft.Json.JsonConvert.DeserializeObject<DKPResp.DetectKeyPhrasesResponse>
                                (dkpjsonResponse);
                            using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                            {
                                foreach (var singleKeyPhraseDoc in dkpRespObj.documents)
                                {
                                    foreach (var singleKeyPhrase in singleKeyPhraseDoc.keyPhrases)
                                    {
                                        CustomerFinderDA.TwitterUserStatusKeyPhrase keyPhraseentity =
                                            new CustomerFinderDA.TwitterUserStatusKeyPhrase()
                                            {
                                                TwitterUserStatusId = Convert.ToInt64(singleKeyPhraseDoc.id),
                                                KeyPhrase = singleKeyPhrase
                                            };
                                        dbCtx.TwitterUserStatusKeyPhrase.Add(keyPhraseentity);
                                    }

                                }
                                dbCtx.SaveChanges();
                            }
                        }
                    }
                }
            }
            #endregion Detect Languages
        }

        private void AddTextAnalyticsTransaction(string transactionType)
        {
            using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
            {
                CustomerFinderDA.TextAnalyticsTransaction transaction = new CustomerFinderDA.TextAnalyticsTransaction()
                {
                    TransactionType = transactionType,
                    CreatedAt = DateTimeOffset.Now
                };
                ctx.TextAnalyticsTransaction.Add(transaction);
                ctx.SaveChanges();
            }
        }

        private void SetTextAnalyticsApiLimit(DateTimeOffset pLimitFoundAt)
        {
            using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
            {
                var apiLimitEntity = ctx.ApiLimit.Where(p => p.ApiName == APINAME_TEXTANALYTICS).SingleOrDefault();
                if (apiLimitEntity == null)
                {
                    apiLimitEntity = new CustomerFinderDA.ApiLimit();
                    apiLimitEntity.ApiName = APINAME_TEXTANALYTICS;
                    apiLimitEntity.LimitFoundAt = pLimitFoundAt;
                    ctx.ApiLimit.Add(apiLimitEntity);
                }
                else
                {
                    apiLimitEntity.LimitFoundAt = pLimitFoundAt;
                }
                TextAnalyticsLimitFoundAt = pLimitFoundAt;
                ctx.SaveChanges();
            }
        }

        private bool ShouldProcessAccount(string username)
        {
            bool shouldProcess = true;
            using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
            {
                dbCtx.Configuration.AutoDetectChangesEnabled = false;
                var existentAccount = dbCtx.TwitterAccounts.Where(p => p.Username == username).SingleOrDefault();
                if (existentAccount != null)
                {
                    var lastTimeProcessed = existentAccount.LastCheckedDate;
                    if (lastTimeProcessed.HasValue)
                    {
                        var currentUTCDate = DateTimeOffset.UtcNow;
                        var timeDifference = currentUTCDate.Subtract(lastTimeProcessed.Value);
                        if (timeDifference.TotalDays > 7)
                        {
                            bool hasFollowersFound = dbCtx.Followers.Where(p => p.UserFollowed == username).LongCount() > 0;
                            bool hasBeeenUnfollowed = dbCtx.UnFollowers.Where(p => p.UserUnFollowed == username).LongCount() > 0;
                            if ((hasFollowersFound == false && hasBeeenUnfollowed == false))
                                shouldProcess = true;
                            else
                                shouldProcess = false;
                        }
                        else
                        {
                            var followerCheckProgress = dbCtx.TwitterFollowerCheckProgress.Where(p => p.TwitterAccountsId == existentAccount.TwitterAccountsId).FirstOrDefault();
                            if (followerCheckProgress != null && followerCheckProgress.NextCursor > 0)
                                shouldProcess = true;
                        }
                    }
                }
            }
            return shouldProcess;
        }
        private Crawlers.Sitesmaps.SitemapHelper helper = new Crawlers.Sitesmaps.SitemapHelper();
        private long? AddTwitterAccount(User user)
        {
            long? dbUserId = null;
            if (user != null)
            {
                using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
                {
                    var existentAccount = dbCtx.TwitterAccounts.Where(p => p.Username == user.ScreenNameResponse).SingleOrDefault();
                    string location = user.Location;
                    if (!string.IsNullOrWhiteSpace(location) && location.Length > 1000)
                        location = location.Substring(0, 1000);

                    if (existentAccount == null)
                    {
                        CustomerFinderDA.TwitterAccount newAccount = new CustomerFinderDA.TwitterAccount();
                        newAccount.ProfileDescription = user.Description;
                        newAccount.Username = user.ScreenNameResponse;
                        newAccount.LastCheckedDate = DateTimeOffset.UtcNow;
                        newAccount.Location = location;
                        newAccount.ProfileImageUrl = user.ProfileImageUrl;
                        newAccount.WebsiteUrl = user.Url;
                        dbCtx.TwitterAccounts.Add(newAccount);
                        dbCtx.SaveChanges();
                        dbUserId = newAccount.TwitterAccountsId;
                    }
                    else
                    {
                        existentAccount.ProfileDescription = user.Description;
                        existentAccount.LastCheckedDate = DateTimeOffset.UtcNow;
                        existentAccount.Location = location;
                        existentAccount.ProfileImageUrl = user.ProfileImageUrl;
                        existentAccount.WebsiteUrl = user.Url;
                        dbCtx.SaveChanges();
                        dbUserId = existentAccount.TwitterAccountsId;
                    }
                    //try
                    //{
                    //    if (!string.IsNullOrWhiteSpace(user.Url))
                    //    {
                    //        helper.ProcessSitemapForUrl(user.Url, true);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                }
            }
            return dbUserId;
        }

        private static async Task CheckTwitterLimit(IAuthorizer auth, TwitterContext ctx, Guid sessionId, string userBeingProcessed)
        {
            var d = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            d = d.AddSeconds(ctx.RateLimitReset);
            var timeToWait = d.Subtract(DateTime.UtcNow);
            int iMillisecondsToWait = (int)Math.Ceiling(timeToWait.TotalMilliseconds);
            if (iMillisecondsToWait < -1)
            {
                iMillisecondsToWait = 30 * 1000; //30 seconds
            }
            SetTwitterWaitingStatus(true, sessionId, iMillisecondsToWait, userBeingProcessed);
            await Task.Delay(iMillisecondsToWait);
            SetTwitterWaitingStatus(false, sessionId, iMillisecondsToWait, userBeingProcessed);
        }

        private static void SetTwitterWaitingStatus(bool isWaiting, Guid sessionId, int millisecondsToWait, string userBeingProcessed)
        {
            using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
            {
                var entity = dbCtx.SessionInfoes.Where(p => p.SessionId == sessionId).SingleOrDefault();
                if (entity != null)
                {
                    entity.IsWaitingTwitterLimit = isWaiting;
                    entity.MillisecondsToWait = millisecondsToWait;
                    entity.UserBeingProcessed = userBeingProcessed;
                    entity.CustomerTwitterUsername = Twitter_Username;
                    if (isWaiting)
                        entity.LastTimeWaiting = DateTimeOffset.UtcNow;
                    dbCtx.SaveChanges();
                }
            }
        }

        private static void SendEmail(string subject, string body)
        {
            try
            {
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.Body = body;
                message.To.Add(Notifications_ToEmailAddress);
                message.Subject = subject;
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {

            }
        }

        private static DateTimeOffset? TextAnalyticsLimitFoundAt = null;
        private async Task RunAsync(CancellationToken cancellationToken)
        {
            List<string> userNamesToCheck = new List<string>();
            //string usernamechecking = Properties.Settings.Default.Twitter_Username;
            //using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
            //{
            //    ctx.Configuration.AutoDetectChangesEnabled = false;
            //    userNamesToCheck = ctx.AspNetUsers.Where(p => p.TwitterUsername != null).Select(p => p.TwitterUsername).ToList();
            //}
            //special scenario for multi-client service
            //await TestSearchApi();
            while (!cancellationToken.IsCancellationRequested)
            {
                //helper.ProcessSitemapForUrl("https://t.co/5c0r8ckF5H", true);
                CleanOldSessions();
                bool bIsMultiClientService = IsMultiClientService();
                if (bIsMultiClientService)
                {
                    GetMultiClientInfo(ref userNamesToCheck);
                    if (!userNamesToCheck.Contains(Twitter_Username))
                        userNamesToCheck.Add(Twitter_Username);
                }
                else
                {
                    if (!userNamesToCheck.Contains(Twitter_Username))
                        userNamesToCheck.Add(Twitter_Username);
                }
                GetTextAnalyticsLimitInfo();
                foreach (string usernamechecking in userNamesToCheck)
                {
                    auth = GetAuthorizerFor(usernamechecking);
                    if (_ctx != null)
                    {
                        _ctx.Dispose();
                    }
                    _ctx = new TwitterContext(auth);
                    if (auth == null)
                        continue;
                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    Trace.TraceInformation("Working");
                    Guid sessionId = Guid.NewGuid();
                    CreateSessionStartInfo(sessionId, usernamechecking);
                    int pasadas = 0;
                    try
                    {
                        //Process 1st Level Twitter Followers
                        pasadas = await CheckFirstLevelTwitterFollowers(usernamechecking, sessionId, pasadas);
                    }
                    catch (AggregateException ex)
                    {
                        ProcessError(usernamechecking, ex);
                    }
                    CreateSessionEndInfo(sessionId);
                    var secondLevelCheckResult = await CheckSecondLevelTwitterFollowers(pasadas, usernamechecking);
                    pasadas = secondLevelCheckResult.Pasadas;
                    watch.Stop();
                    var cicleDuration = watch.Elapsed;
                    //SendEmail("Full Cycle completed", string.Format("Duration: {0} days, {1} hours, {2} minutes, {3} seconds. User: {4}. MultiClientApp:{5}", cicleDuration.Days, cicleDuration.Hours, cicleDuration.Minutes, cicleDuration.Seconds, usernamechecking, bIsMultiClientService));
                }
            }
        }

        private async Task TestSearchApi()
        {
            TwitterGnip.SearchApi.SearchApi searchApi = new TwitterGnip.SearchApi.SearchApi();
            var testResult = await searchApi.GetTweetFromUser(Twitter_Username);
        }

        private void GetMultiClientInfo(ref List<string> userNamesToCheck)
        {
            try
            {
                using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
                {
                    userNamesToCheck = ctx.TwitterMultiClientQueue.Where(p => p.Enabled == true).Select(p => p.TwitterUsername).ToList();
                }
            }
            catch (Exception)
            {

            }
            /*userNamesToCheck.Add("YoViajoCR");
            userNamesToCheck.Add("LiAmorCR");
            userNamesToCheck.Add("donpascualoncr");
            userNamesToCheck.Add("AngerithCG");
            userNamesToCheck.Add("PLaLibertad");
            userNamesToCheck.Add("casapropiacr");
            userNamesToCheck.Add("Starbucks_cr");
            userNamesToCheck.Add("mcdonaldscr");*/
        }

        private void CleanOldSessions()
        {
            try
            {
                using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
                {
                    var existentOldSessions = ctx.SessionInfoes.Where(p => p.CustomerTwitterUsername == Twitter_Username);
                    ctx.SessionInfoes.RemoveRange(existentOldSessions);
                    ctx.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
        }

        private bool IsMultiClientService()
        {
            return false;
            //return (Twitter_Username == "REPLACE");
        }

        private class CheckSecondLevelTwitterFollowersResult
        {
            public int Pasadas { get; internal set; }
            public Guid SessionId { get; set; }
        }
        private async Task<CheckSecondLevelTwitterFollowersResult> CheckSecondLevelTwitterFollowers(int pasadas, string usernamechecking)
        {
            List<string> followersUsernames = new List<string>();
            using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
            {
                dbCtx.Configuration.AutoDetectChangesEnabled = false;
                if (IsMultiClientService())
                {
                    followersUsernames = dbCtx.Followers.Where(p => p.UserFollowed == usernamechecking).OrderBy(p => Guid.NewGuid()).Select(p => p.UserFollowing).Take(1).ToList();
                }
                else
                {
                    followersUsernames = dbCtx.Followers.Where(p => p.UserFollowed == usernamechecking).OrderBy(p => Guid.NewGuid()).Select(p => p.UserFollowing).Take(10).ToList();
                }
            }
            Guid sessionId = Guid.NewGuid();
            CreateSessionStartInfo(sessionId, usernamechecking);
            pasadas = await IterateFollowers(followersUsernames, sessionId, pasadas);
            CreateSessionEndInfo(sessionId);
            return new CheckSecondLevelTwitterFollowersResult()
            {
                SessionId = sessionId,
                Pasadas = pasadas
            };
        }

        private async Task<int> CheckFirstLevelTwitterFollowers(string usernamechecking, Guid sessionId, int pasadas)
        {
            if (ShouldProcessAccount(usernamechecking))
                pasadas = await CheckUnFollowers(usernamechecking, sessionId);
            return pasadas;
        }

        private void GetTextAnalyticsLimitInfo()
        {
            using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
            {
                ctx.Configuration.AutoDetectChangesEnabled = false;
                var entity = ctx.ApiLimit.Where(p => p.ApiName == APINAME_TEXTANALYTICS).FirstOrDefault();
                if (entity != null)
                {
                    TextAnalyticsLimitFoundAt = entity.LimitFoundAt;
                }
            }
        }

        private const string APINAME_TEXTANALYTICS = "Text Analytics";
        private const string TRANSACTION_DETECTLANGUAGE = "Detect Language";
        private const string TRANSACTION_DETECTSENTIMENT = "Detect Sentiment";
        private const string TRANSACTION_DETECTKEYPHRASES = "Detect Key Phrases";
        private void CheckForTextAnalyticsReset()
        {
            bool mustResetTextAnalyticsLimit = false;
            if (TextAnalyticsLimitFoundAt.HasValue && DateTimeOffset.Now > TextAnalyticsLimitFoundAt.Value.AddMonths(1))
                mustResetTextAnalyticsLimit = true;
            if (mustResetTextAnalyticsLimit)
            {
                using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
                {
                    var apiLimitEntity = ctx.ApiLimit.Where(p => p.ApiName == APINAME_TEXTANALYTICS).FirstOrDefault();
                    if (apiLimitEntity != null)
                    {
                        apiLimitEntity.LimitFoundAt = null;
                        ctx.SaveChanges();
                        TextAnalyticsLimitFoundAt = null;
                    }
                }
            }
        }

        private async Task<int> IterateFollowers(List<string> followersUsernames, Guid sessionId, int pasadas)
        {
            System.Text.StringBuilder strErrors = new System.Text.StringBuilder();
            foreach (var singleFollower in followersUsernames)
            {
                try
                {
                    if (ShouldProcessAccount(singleFollower))
                    {
                        Stopwatch watch = new Stopwatch();
                        watch.Start();
                        pasadas = await CheckUnFollowers(singleFollower, sessionId);
                        watch.Stop();
                        var cicleDuration = watch.Elapsed;
                        //SendEmail("Cycle for user completed", string.Format("User: {0} Duration: {1} days, {2} hours, {3} minutes, {4} seconds", new object[] { singleFollower, cicleDuration.Days, cicleDuration.Hours, cicleDuration.Minutes, cicleDuration.Seconds }));
                    }
                }
                catch (AggregateException ex)
                {
                    ProcessError(singleFollower, ex);
                }
            }

            return pasadas;
        }

        private static void ProcessError(string singleFollower, AggregateException ex)
        {
            try
            {
                using (CustomerFinderDA.CustomerFinderContext ctx = new CustomerFinderDA.CustomerFinderContext())
                {
                    var binaryAddress = Dns.GetHostAddresses(Dns.GetHostName()).First().GetAddressBytes()
                        .Select(number => Convert.ToString(number, 2).PadLeft(8, '0')).First();
                    var bytes = binaryAddress.Split('.')
                        .Select(@byte => Convert.ToInt32(@byte, 2));
                    var address = IPAddress.Parse(string.Join(".", bytes));
                    ctx.ExceptionLogs.Add(
                        new CustomerFinderDA.ExceptionLog()
                        {
                            Exception = ex.ToString(),
                            LoggedDateTime = DateTime.UtcNow,
                            Message = string.Format("Exceptions Found on 'UnfollowCheatScanCloudService' processing {0} - {1}", singleFollower, ex.Message),
                            StackTrace = ex.StackTrace,
                            ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                            ServerIP = address.ToString()
                        }
                        );
                    System.Text.StringBuilder strExceptions = new System.Text.StringBuilder();
                    strExceptions.AppendLine("Exceptions where found processing: " + singleFollower);
                    strExceptions.AppendLine("Main Exception: " + ex.Message);
                    strExceptions.AppendLine("Main Exception Details: " + ex.ToString());
                    strExceptions.AppendLine("Inner Exceptions: ");
                    strExceptions.AppendLine("****************************");
                    foreach (var e in ex.InnerExceptions)
                    {
                        ctx.ExceptionLogs.Add(
                            new CustomerFinderDA.ExceptionLog()
                            {
                                Exception = e.ToString(),
                                LoggedDateTime = DateTime.UtcNow,
                                Message = string.Format("Exceptions Found on 'UnfollowCheatScanCloudService' processing {0} - {1}", singleFollower, e.Message),
                                StackTrace = e.StackTrace,
                                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                                ServerIP = address.ToString()
                            }
                            );
                        strExceptions.AppendLine("Exception Message: " + e.Message);
                        strExceptions.AppendLine("Exception Details: " + e.ToString());
                        strExceptions.AppendLine("****************************");
                    }
                    SendEmail("Exceptions Found on 'UnfollowCheatScanCloudService' processing " + singleFollower, strExceptions.ToString());
                    ctx.SaveChanges();
                }

            }
            catch (Exception EX)
            {
                //we must ignore any exception that happens here to avoid "infinite loops"
            }
        }

        private void CreateSessionEndInfo(Guid sessionId)
        {
            using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
            {
                var entity = dbCtx.SessionInfoes.Where(p => p.SessionId == sessionId).SingleOrDefault();
                if (entity != null)
                {
                    entity.EndDateTime = DateTimeOffset.UtcNow;
                    entity.IsWaitingTwitterLimit = false;
                    dbCtx.SaveChanges();
                }
            }
        }

        private void CreateSessionStartInfo(Guid sessionId, string userBeingProcessed)
        {
            using (CustomerFinderDA.CustomerFinderContext dbCtx = new CustomerFinderDA.CustomerFinderContext())
            {
                var currentSessions = dbCtx.SessionInfoes.Where(p => p.IsWaitingTwitterLimit == true && p.CustomerTwitterUsername == Twitter_Username);
                foreach (var singleExistentSession in currentSessions)
                {
                    singleExistentSession.IsWaitingTwitterLimit = false;
                    singleExistentSession.EndDateTime = DateTimeOffset.UtcNow;
                }
                dbCtx.SaveChanges();
                CustomerFinderDA.SessionInfo sesionInfo = new CustomerFinderDA.SessionInfo();
                sesionInfo.EndDateTime = null;
                sesionInfo.SessionId = sessionId;
                sesionInfo.StartDateTime = DateTimeOffset.UtcNow;
                sesionInfo.IsWaitingTwitterLimit = false;
                sesionInfo.LastTimeWaiting = null;
                sesionInfo.CustomerTwitterUsername = Twitter_Username;
                sesionInfo.UserBeingProcessed = userBeingProcessed;
                dbCtx.SessionInfoes.Add(sesionInfo);
                dbCtx.SaveChanges();
            }
        }
    }
}
