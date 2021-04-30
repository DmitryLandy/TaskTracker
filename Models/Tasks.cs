using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class Tasks
    {
        [Key]
        public int TaskID { get; set; }

        [ForeignKey("Id")]
        public int ProjectID { get; set; }       

        [Required]
        [StringLength(50)]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }

        [Required]
        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Task Description")]
        public string TaskDescription { get; set; }        

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Task Start Date")]
        public string TaskStartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Task Deadline")]
        public string TaskDeadline { get; set; }

        [Required]
        [Display(Name = "Task Completion Status")]
        public bool Completed { get; set; }
             
    }
}
