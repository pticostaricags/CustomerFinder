﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CustomerFinderCognitiveServices.Models
{
    public partial class SentimentBatchResultItemV2
    {
        private string _id;
        
        /// <summary>
        /// Optional. Unique document identifier.
        /// </summary>
        public string ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
        
        private double? _score;
        
        /// <summary>
        /// Optional. A decimal number between 0 and 1 denoting the sentiment
        /// of the document.
        /// A score above 0.7 usually refers to a positive document
        /// while a score below 0.3 normally has a negative connotation.
        /// Mid values refer to neutral text.
        /// </summary>
        public double? Score
        {
            get { return this._score; }
            set { this._score = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the SentimentBatchResultItemV2 class.
        /// </summary>
        public SentimentBatchResultItemV2()
        {
        }
        
        /// <summary>
        /// Deserialize the object
        /// </summary>
        public virtual void DeserializeJson(JToken inputObject)
        {
            if (inputObject != null && inputObject.Type != JTokenType.Null)
            {
                JToken idValue = inputObject["id"];
                if (idValue != null && idValue.Type != JTokenType.Null)
                {
                    this.ID = ((string)idValue);
                }
                JToken scoreValue = inputObject["score"];
                if (scoreValue != null && scoreValue.Type != JTokenType.Null)
                {
                    this.Score = ((double)scoreValue);
                }
            }
        }
    }
}
