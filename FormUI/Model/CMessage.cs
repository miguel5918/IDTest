using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormUI.Model
{
    public class CMessage
    {
         
        public int MessageId { get; set; }

        
        public DateTime CreatedAt { get; set; }

        
        public required string Recipient { get; set; }

        
        public required string Message { get; set; }
    }
}
