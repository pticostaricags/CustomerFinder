﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Collections.Generic;
using System.Linq;
using CustomerFinderCognitiveServices.Models;
using Microsoft.Rest;
using Newtonsoft.Json.Linq;

namespace CustomerFinderCognitiveServices.Models
{
    public partial class MultiLanguageBatchInputV2
    {
        private IList<MultiLanguageInputV2> _documents;
        
        /// <summary>
        /// Optional.
        /// </summary>
        public IList<MultiLanguageInputV2> Documents
        {
            get { return this._documents; }
            set { this._documents = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the MultiLanguageBatchInputV2 class.
        /// </summary>
        public MultiLanguageBatchInputV2()
        {
            this.Documents = new LazyList<MultiLanguageInputV2>();
        }
        
        /// <summary>
        /// Serialize the object
        /// </summary>
        /// <returns>
        /// Returns the json model for the type MultiLanguageBatchInputV2
        /// </returns>
        public virtual JToken SerializeJson(JToken outputObject)
        {
            if (outputObject == null)
            {
                outputObject = new JObject();
            }
            JArray documentsSequence = null;
            if (this.Documents != null)
            {
                if (this.Documents is ILazyCollection<MultiLanguageInputV2> == false || ((ILazyCollection<MultiLanguageInputV2>)this.Documents).IsInitialized)
                {
                    documentsSequence = new JArray();
                    outputObject["documents"] = documentsSequence;
                    foreach (MultiLanguageInputV2 documentsItem in this.Documents)
                    {
                        if (documentsItem != null)
                        {
                            documentsSequence.Add(documentsItem.SerializeJson(null));
                        }
                    }
                }
            }
            return outputObject;
        }
    }
}
