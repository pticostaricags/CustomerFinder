using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [ComplexType]
    public class TwitterFollowerId
    {
        public string TwitterAccountsId { get; set; }
    }
}
