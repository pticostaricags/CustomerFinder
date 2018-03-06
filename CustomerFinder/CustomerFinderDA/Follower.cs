using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    public class Follower
    {
        [Key]
        public long FollowersId { get; set; }

        [Required]
        [StringLength(50)]
        public string UserFollowed { get; set; }

        [Required]
        [StringLength(50)]
        public string UserFollowing { get; set; }

        public DateTimeOffset LastTimeSeenFollowing { get; set; }

        public Guid SessionId { get; set; }
    }
}
