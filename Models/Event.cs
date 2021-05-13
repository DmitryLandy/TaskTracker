using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [ForeignKey("Id")]
        public string UserID { get; set; }
        [Required]
        public String Subject { get; set; }
        public string Description { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime Stop { get; set; }
        public string ThemeColor { get; set; }
        [Required]
        public bool IsFullDay { get; set; }
    }
}
