﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Linq;
using CustomerFinderCognitiveServices.Models;
using Newtonsoft.Json.Linq;

namespace CustomerFinderCognitiveServices.Models
{
    public partial class OperationResult
    {
        private DateTimeOffset? _createdDateTime;
        
        /// <summary>
        /// Optional. Operation creation date time (ISO 8601 literal).
        /// </summary>
        public DateTimeOffset? CreatedDateTime
        {
            get { return this._createdDateTime; }
            set { this._createdDateTime = value; }
        }
        
        private DateTimeOffset? _lastActionDateTime;
        
        /// <summary>
        /// Optional. Operation last status change date time (ISO 8601 literal).
        /// </summary>
        public DateTimeOffset? LastActionDateTime
        {
            get { return this._lastActionDateTime; }
            set { this._lastActionDateTime = value; }
        }
        
        private string _message;
        
        /// <summary>
        /// Optional. Error message. Exists only in case the operation has
        /// reached a 'Failed' state.
        /// </summary>
        public string Message
        {
            get { return this._message; }
            set { this._message = value; }
        }
        
        private OperationProcessingResult _operationProcessingResult;
        
        /// <summary>
        /// Optional. Operation result. Specific format varies according to the
        /// operation type. Exists only in case the operation has reached a
        /// 'Succeeded' state.
        /// </summary>
        public OperationProcessingResult OperationProcessingResult
        {
            get { return this._operationProcessingResult; }
            set { this._operationProcessingResult = value; }
        }
        
        private string _operationType;
        
        /// <summary>
        /// Optional. Name of API endpoint that created the operation.
        /// </summary>
        public string OperationType
        {
            get { return this._operationType; }
            set { this._operationType = value; }
        }
        
        private string _status;
        
        /// <summary>
        /// Optional. Operation status.
        /// </summary>
        public string Status
        {
            get { return this._status; }
            set { this._status = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the OperationResult class.
        /// </summary>
        public OperationResult()
        {
        }
        
        /// <summary>
        /// Deserialize the object
        /// </summary>
        public virtual void DeserializeJson(JToken inputObject)
        {
            if (inputObject != null && inputObject.Type != JTokenType.Null)
            {
                JToken createdDateTimeValue = inputObject["createdDateTime"];
                if (createdDateTimeValue != null && createdDateTimeValue.Type != JTokenType.Null)
                {
                    this.CreatedDateTime = ((DateTimeOffset)createdDateTimeValue);
                }
                JToken lastActionDateTimeValue = inputObject["lastActionDateTime"];
                if (lastActionDateTimeValue != null && lastActionDateTimeValue.Type != JTokenType.Null)
                {
                    this.LastActionDateTime = ((DateTimeOffset)lastActionDateTimeValue);
                }
                JToken messageValue = inputObject["message"];
                if (messageValue != null && messageValue.Type != JTokenType.Null)
                {
                    this.Message = ((string)messageValue);
                }
                JToken operationProcessingResultValue = inputObject["operationProcessingResult"];
                if (operationProcessingResultValue != null && operationProcessingResultValue.Type != JTokenType.Null)
                {
                    OperationProcessingResult operationProcessingResult = null;
                    string typeName = ((string)operationProcessingResultValue["discriminator"]);
                    if (typeName == "TopicDetectionResultV2")
                    {
                        operationProcessingResult = new TopicDetectionResultV2();
                        operationProcessingResult.DeserializeJson(operationProcessingResultValue);
                    }
                    else
                    {
                        operationProcessingResult = new OperationProcessingResult();
                        operationProcessingResult.DeserializeJson(operationProcessingResultValue);
                    }
                    this.OperationProcessingResult = operationProcessingResult;
                }
                JToken operationTypeValue = inputObject["operationType"];
                if (operationTypeValue != null && operationTypeValue.Type != JTokenType.Null)
                {
                    this.OperationType = ((string)operationTypeValue);
                }
                JToken statusValue = inputObject["status"];
                if (statusValue != null && statusValue.Type != JTokenType.Null)
                {
                    this.Status = ((string)statusValue);
                }
            }
        }
    }
}
