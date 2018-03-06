using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerFinderDA
{
    [Table("TwitterFollowerCheckProgress")]
    public class TwitterFollowerCheckProgress
    {
        /*
         * 	[TwitterFollowerCheckProgressId] BIGINT NOT NULL CONSTRAINT PK_TWITTERFOLLOWERCHECKPROGRESS PRIMARY KEY, 
    [TwitterAccountsId] BIGINT NOT NULL, 
    [NextCursor] BIGINT NOT NULL
         * */
         [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
         public long TwitterFollowerCheckProgressId { get; set; }
        public long TwitterAccountsId { get; set; }
        public long NextCursor { get; set; }
    }
}
