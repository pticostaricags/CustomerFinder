using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("TwitterAccount")]
    public partial class TwitterAccount
    {
        /*
        	[TwitterAccountsId] BIGINT NOT NULL PRIMARY KEY IDENTITY, 
    [Username] NVARCHAR(100) NULL, 
    [ProfileDescription] NVARCHAR(1000) NOT NULL
        */
        [Key]
        public long TwitterAccountsId { get; set; }
        [Required]
        [StringLength(100)]
        public string Username { get; set; }
        [StringLength(1000)]
        public string ProfileDescription { get; set; }
        public DateTimeOffset? LastCheckedDate { get; set; }
        public string Location { get; set; }
        public string ProfileImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public DateTimeOffset? LastTimeTweetsProcessed { get; set; }
    }
}
