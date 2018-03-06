﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.9.7.0
// Changes may cause incorrect behavior and will be lost if the code is regenerated.

using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CustomerFinderCognitiveServices.Models
{
    public partial class MultiLanguageInputV2
    {
        private string _id;
        
        /// <summary>
        /// Optional. Unique, non-empty document identifier.
        /// </summary>
        public string ID
        {
            get { return this._id; }
            set { this._id = value; }
        }
        
        private string _language;
        
        /// <summary>
        /// Optional. This is the 2 letter ISO 639-1 representation of a
        /// language.
        /// For example, use "en" for English; "es" for Spanish
        /// etc.,
        /// </summary>
        public string Language
        {
            get { return this._language; }
            set { this._language = value; }
        }
        
        private string _text;
        
        /// <summary>
        /// Optional.
        /// </summary>
        public string Text
        {
            get { return this._text; }
            set { this._text = value; }
        }
        
        /// <summary>
        /// Initializes a new instance of the MultiLanguageInputV2 class.
        /// </summary>
        public MultiLanguageInputV2()
        {
        }
        
        /// <summary>
        /// Serialize the object
        /// </summary>
        /// <returns>
        /// Returns the json model for the type MultiLanguageInputV2
        /// </returns>
        public virtual JToken SerializeJson(JToken outputObject)
        {
            if (outputObject == null)
            {
                outputObject = new JObject();
            }
            if (this.ID != null)
            {
                outputObject["id"] = this.ID;
            }
            if (this.Language != null)
            {
                outputObject["language"] = this.Language;
            }
            if (this.Text != null)
            {
                outputObject["text"] = this.Text;
            }
            return outputObject;
        }
    }
}