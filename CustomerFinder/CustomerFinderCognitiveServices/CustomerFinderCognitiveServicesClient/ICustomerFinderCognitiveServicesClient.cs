﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CustomerFinderCognitiveServices.Models;
using Microsoft.Rest;

namespace CustomerFinderCognitiveServices
{
    public partial interface ICustomerFinderCognitiveServicesClient : IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        Uri BaseUri
        {
            get; set; 
        }
        
        /// <summary>
        /// Credentials for authenticating with the service.
        /// </summary>
        ServiceClientCredentials Credentials
        {
            get; set; 
        }
        
        /// <summary>
        /// The API returns the detected language and a numeric score between 0
        /// and 1.
        /// Scores close to 1 indicate 100% certainty that the
        /// identified language is true.
        /// A total of 120 languages are supported.
        /// </summary>
        /// <param name='numberOfLanguagesToDetect'>
        /// Optional. Format - int32. (Optional) Number of languages to detect.
        /// Set to 1 by default.
        /// </param>
        /// <param name='batchInputV2'>
        /// Optional.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> DetectLanguageWithOperationResponseAsync(int? numberOfLanguagesToDetect = null, BatchInputV2 batchInputV2 = null, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <summary>
        /// The API returns the top detected topics for a list of submitted
        /// text documents.
        /// A topic is identified with a key phrase, which can be
        /// one or more related words.
        /// Use the URL parameters and stop word list to control
        /// which words or documents are filtered out.
        /// You can also supply a list of topics to exclude from
        /// the response.
        /// At least 100 text documents must be submitted, however
        /// it is designed to detect topics across hundreds to thousands of
        /// documents.
        /// Note that one transaction is charged per text document
        /// submitted.
        /// For best performance, limit each document to a short,
        /// human written text paragraph such as review, conversation or user
        /// feedback.
        /// </summary>
        /// <param name='minDocumentsPerWord'>
        /// Optional. Format - int32. (optional) Words that occur in less than
        /// this many documents are ignored.
        /// Use this parameter to help exclude rare document
        /// topics.
        /// Omit to let the service choose appropriate value.
        /// </param>
        /// <param name='maxDocumentsPerWord'>
        /// Optional. Format - int32. (optional) Words that occur in more than
        /// this many documents are ignored.
        /// Use this parameter to help exclude ubiquitous document
        /// topics.
        /// Omit to let the service choose appropriate value.
        /// </param>
        /// <param name='topicDetectionInputV2'>
        /// Optional.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<ErrorResponse>> DetectTopicsWithOperationResponseAsync(int? minDocumentsPerWord = null, int? maxDocumentsPerWord = null, TopicDetectionInputV2 topicDetectionInputV2 = null, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <summary>
        /// The API returns a list of strings denoting the key talking points
        /// in the input text.
        /// We employ techniques from Microsoft Office's
        /// sophisticated Natural Language Processing toolkit.
        /// Currently, the following languages are supported:
        /// English, German, Spanish and Japanese.
        /// </summary>
        /// <param name='multiLanguageBatchInputV2'>
        /// Optional.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> KeyPhrasesWithOperationResponseAsync(MultiLanguageBatchInputV2 multiLanguageBatchInputV2 = null, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <summary>
        /// Get the status of an operation submitted for processing. If the the
        /// operation has reached a 'Succeeded' state, will also return the
        /// result.
        /// </summary>
        /// <param name='operationId'>
        /// Required. A unique id for the submitted operation.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> OperationStatusWithOperationResponseAsync(string operationId, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
        
        /// <summary>
        /// The API returns a numeric score between 0 and 1.
        /// Scores close to 1 indicate positive sentiment, while
        /// scores close to 0 indicate negative sentiment.
        /// Sentiment score is generated using classification
        /// techniques.
        /// The input features to the classifier include n-grams,
        /// features generated from part-of-speech tags, and word embeddings.
        /// Currently, the following languages are supported:
        /// English, Spanish, French, Portuguese.
        /// </summary>
        /// <param name='multiLanguageBatchInputV2'>
        /// Optional.
        /// </param>
        /// <param name='cancellationToken'>
        /// Cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> SentimentWithOperationResponseAsync(MultiLanguageBatchInputV2 multiLanguageBatchInputV2 = null, CancellationToken cancellationToken = default(System.Threading.CancellationToken));
    }
}