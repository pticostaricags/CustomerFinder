using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("TwitterUserStatusSentiment")]
    public class TwitterUserStatusSentiment
    {
        /*
         * CREATE TABLE [dbo].[TwitterUserStatusSentiment]
(
	[TwitterUserStatusLanguageId] BIGINT NOT NULL CONSTRAINT PKTWITTERUSERSTATUSSENTIMENT PRIMARY KEY IDENTITY, 
    [TwitterUserStatusId] BIGINT NOT NULL, 
    [score] FLOAT NOT NULL, 
    CONSTRAINT [FK_TwitterUserStatusSentiment_TwitterUserStatus] FOREIGN KEY ([TwitterUserStatusId]) REFERENCES [TwitterUserStatus]([TwitterUserStatusId])
)

         * */

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TwitterUserStatusSentimentId { get; set; }
        public long TwitterUserStatusId { get; set; }
        public float score { get; set; }
    }
}
