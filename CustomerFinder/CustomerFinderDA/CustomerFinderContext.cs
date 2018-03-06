using EntityFramework.Functions;
using Microsoft.Azure;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace CustomerFinderDA
{
    public enum ApplicationType
    {
        CloudService = 0,
        MVCWebSite = 1
    }
    public class CustomerFinderContext : DbContext
    {
        static CustomerFinderContext()
        {
            Database.SetInitializer<CustomerFinderContext>(null);
        }

        private void SetTimeout(int seconds = 0)
        {
            try
            {
                IObjectContextAdapter contextAdapter = (IObjectContextAdapter)this;
                contextAdapter.ObjectContext.CommandTimeout = seconds;
            }
            catch (Exception)
            {

            }
        }

        public CustomerFinderContext()
            : base(CloudConfigurationManager.GetSetting("CustomerFinderContext"))
        {
            SetTimeout();
        }

        public CustomerFinderContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            SetTimeout();
        }

        public virtual DbSet<Follower> Followers { get; set; }
        public virtual DbSet<UnFollower> UnFollowers { get; set; }
        public virtual DbSet<SessionInfo> SessionInfoes { get; set; }
        public virtual DbSet<TwitterAccount> TwitterAccounts { get; set; }
        public virtual DbSet<TwitterUserStatus> TwitterUserStatuses { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<TwitterFollowerCheckProgress> TwitterFollowerCheckProgress { get; set; }
        public virtual DbSet<TwitterUserStatusLanguage> TwitterUserStatusLanguage { get; set; }
        public virtual DbSet<TwitterUserStatusSentiment> TwitterUserStatusSentiment { get; set; }
        public virtual DbSet<ApiLimit> ApiLimit { get; set; }
        public virtual DbSet<TextAnalyticsTransaction> TextAnalyticsTransaction { get; set; }
        public virtual DbSet<TwitterUserStatusKeyPhrase> TwitterUserStatusKeyPhrase { get; set; }
        public virtual DbSet<TwitterMultiClientQueue> TwitterMultiClientQueue { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Add(new FunctionConvention<CustomerFinderContext>());

            modelBuilder.ComplexType<TwitterFollowerId>();
            modelBuilder.ComplexType<TwitterNetworkUserStatus>();
        }

        // Defines table-valued function, which must return IQueryable<T>.
        //[Function(FunctionType.TableValuedFunction, nameof(fnGetTwitterFirstLevelFollowersIds),"CodeFirstDatabaseSchema", Schema = "dbo")]
        [Function(FunctionType.TableValuedFunction, "fnGetTwitterFirstLevelFollowersIds", "CustomerFinderNS", Schema = "dbo")]
        public IQueryable<TwitterFollowerId> fnGetTwitterFirstLevelFollowersIds(
            [Parameter(DbType = "nvarchar", Name = "username")] string username
            )
        {
            ObjectParameter usernameParam = new ObjectParameter("username", username);
            return this.ObjectContext().CreateQuery<TwitterFollowerId>(
                $"[{nameof(this.fnGetTwitterFirstLevelFollowersIds)}](@{nameof(username)})", usernameParam
                );
        }

        [Function(FunctionType.TableValuedFunction, "fnGetTwitterSecondLevelFollowersIds", "CustomerFinderNS", Schema = "dbo")]
        public IQueryable<TwitterFollowerId> fnGetTwitterSecondLevelFollowersIds(
            [Parameter(DbType = "nvarchar", Name = "username")] string username
            )
        {
            ObjectParameter usernameParam = new ObjectParameter("username", username);
            return this.ObjectContext().CreateQuery<TwitterFollowerId>(
                $"[{nameof(this.fnGetTwitterSecondLevelFollowersIds)}](@{nameof(username)})", usernameParam
                );
        }
        //fnGetNetworkTwitterUserStatus
        [Function(FunctionType.TableValuedFunction, "fnGetNetworkTwitterUserStatus", "CustomerFinderNS", Schema = "dbo")]
        public IQueryable<TwitterNetworkUserStatus> fnGetNetworkTwitterUserStatus([Parameter(DbType = "nvarchar", Name = "username")] string username
    )
        {
            ObjectParameter usernameParam = new ObjectParameter("username", username);
            return this.ObjectContext().CreateQuery<TwitterNetworkUserStatus>(
                $"[{nameof(this.fnGetNetworkTwitterUserStatus)}](@{nameof(username)})", usernameParam
                );
        }
    }
}
