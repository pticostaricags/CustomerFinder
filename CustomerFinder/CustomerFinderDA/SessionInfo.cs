using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("SessionInfo")]
    public partial class SessionInfo
    {
        [Key]
        public long SessionInfoId { get; set; }

        [Required]
        public Guid SessionId { get; set; }

        [Required]
        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset? EndDateTime { get; set; }

        public bool IsWaitingTwitterLimit { get; set; }

        public DateTimeOffset? LastTimeWaiting { get; set; }
        public int MillisecondsToWait { get; set; }
        [StringLength(50)]
        public string UserBeingProcessed { get; set; }
        public string CustomerTwitterUsername { get; set; }
    }
}
