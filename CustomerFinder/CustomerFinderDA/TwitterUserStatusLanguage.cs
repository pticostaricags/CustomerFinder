using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("TwitterUserStatusLanguage")]
    public class TwitterUserStatusLanguage
    {
        /*
         * 	[TwitterUserStatusLanguageId] BIGINT NOT NULL CONSTRAINT PK_TWITTERUSERSTATUSLANGUAGE PRIMARY KEY IDENTITY, 
    [TwitterUserStatusId] BIGINT NOT NULL, 
    [Name] NVARCHAR(250) NOT NULL, 
    [iso6391Name] NVARCHAR(250) NOT NULL, 
    [score] FLOAT NOT NULL, 
         * */

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TwitterUserStatusLanguageId { get; set; }
        public long TwitterUserStatusId { get; set; }
        public string Name { get; set; }
        public string iso6391Name { get; set; }
        public float score { get; set; }
    }
}
