using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderWorkerRole.Crawlers.Sitesmaps
{


    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", IsNullable = false)]
    public partial class urlset
    {

        private urlsetUrl[] urlField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("url")]
        public urlsetUrl[] url
        {
            get
            {
                return this.urlField;
            }
            set
            {
                this.urlField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
    public partial class urlsetUrl
    {

        private object[] itemsField;

        private ItemsChoiceType[] itemsElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("image", typeof(image), Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
        [System.Xml.Serialization.XmlElementAttribute("mobile", typeof(object), Namespace = "http://www.google.com/schemas/sitemap-mobile/1.0")]
        [System.Xml.Serialization.XmlElementAttribute("changefreq", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("lastmod", typeof(System.DateTime))]
        [System.Xml.Serialization.XmlElementAttribute("loc", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("priority", typeof(decimal))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName")]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemsChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.google.com/schemas/sitemap-image/1.1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.google.com/schemas/sitemap-image/1.1", IsNullable = false)]
    public partial class image
    {

        private string locField;

        private string titleField;

        private string captionField;

        /// <remarks/>
        public string loc
        {
            get
            {
                return this.locField;
            }
            set
            {
                this.locField = value;
            }
        }

        /// <remarks/>
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        public string caption
        {
            get
            {
                return this.captionField;
            }
            set
            {
                this.captionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9", IncludeInSchema = false)]
    public enum ItemsChoiceType
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("http://www.google.com/schemas/sitemap-image/1.1:image")]
        image,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("http://www.google.com/schemas/sitemap-mobile/1.0:mobile")]
        mobile,

        /// <remarks/>
        changefreq,

        /// <remarks/>
        lastmod,

        /// <remarks/>
        loc,

        /// <remarks/>
        priority,
    }



}
