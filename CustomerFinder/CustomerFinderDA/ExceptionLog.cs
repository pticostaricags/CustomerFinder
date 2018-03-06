using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("ExceptionLog")]
    public partial class ExceptionLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long ExceptionLogId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public string Exception { get; set; }
        [Required]
        public DateTimeOffset LoggedDateTime { get; set; }
        [Required]
        public string StackTrace { get; set; }

        public string Url { get; set; }
        [Required]
        public string ApplicationName { get; set; }
        [MaxLength(50)]
        [Required]
        public string ServerIP { get; set; }
        public string RequestData { get; set; }
    }
}
