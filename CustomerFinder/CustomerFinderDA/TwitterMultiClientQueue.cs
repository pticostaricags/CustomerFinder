using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("TwitterMultiClientQueue")]
    public class TwitterMultiClientQueue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TwitterMultiClientQueueId { get; set; }

        public string TwitterUsername { get; set; }
        public bool Enabled { get; set; }
    }
}
