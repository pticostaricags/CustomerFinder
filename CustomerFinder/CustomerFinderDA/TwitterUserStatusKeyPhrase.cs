using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("TwitterUserStatusKeyPhrase")]
    public class TwitterUserStatusKeyPhrase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TwitterUserStatusKeyPhraseId { get; set; }
        public long TwitterUserStatusId { get; set; }
        public string KeyPhrase { get; set; }
    }
}
