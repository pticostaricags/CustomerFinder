using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("ApiLimit")]
    public class ApiLimit
    {
        /*
         * [ApiLimitId] BIGINT NOT NULL CONSTRAINT PK_APILIMIT PRIMARY KEY IDENTITY, 
    [ApiName] NVARCHAR(250) NOT NULL, 
    [LimitFoundAt] DATETIMEOFFSET NULL
         * */

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ApiLimitId { get; set; }
        [Required]
        public string ApiName { get; set; }
        public DateTimeOffset? LimitFoundAt { get; set; }
    }
}
