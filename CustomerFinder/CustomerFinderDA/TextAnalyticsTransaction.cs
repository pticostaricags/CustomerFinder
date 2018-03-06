using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerFinderDA
{
    [Table("TextAnalyticsTransaction")]
    public class TextAnalyticsTransaction
    {
        /*
         * [TextAnalyticsTransactionId] BIGINT NOT NULL CONSTRAINT PK_TEXTANALYTICSTRANSACTION PRIMARY KEY IDENTITY, 
    [TransactionType] NVARCHAR(100) NOT NULL, 
    [CreatedAt] DATETIMEOFFSET NOT NULL 
         **/
         [Key]
        public long TextAnalyticsTransactionId { get; set; }
        public string TransactionType { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
