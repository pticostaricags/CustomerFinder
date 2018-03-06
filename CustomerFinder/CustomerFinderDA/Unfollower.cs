using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    public partial class UnFollower
    {
        [Key]
        public long UnFollowersId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserUnFollowed { get; set; }

        [Required]
        [StringLength(50)]
        public string UserUnFollowing { get; set; }

        public DateTimeOffset LastTimeSeenUnFollowing { get; set; }

        public Guid SessionId { get; set; }
    }
}
