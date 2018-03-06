using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("TwitterUserStatus")]
    public class TwitterUserStatus
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TwitterUserStatusId { get; set; }
        [Required]
        public long TwitterAccountsId { get; set; }
        public string StatusText { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string StatusUrl { get; set; }
        [ForeignKey("TwitterAccountsId")]
        public TwitterAccount User { get; set; }
        public int? FavoriteCount { get; set; }
        public int? RetweetCount { get; set; }
        public int? StatusCount { get; set; }
        public string TweetId { get; set; }
    }
}
