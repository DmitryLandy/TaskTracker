
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BugTracker.Models
{
    public class Projects
    {
        [Key]
        public int ProjectID { get; set; }
         
        [ForeignKey("Id")]
        public string UserID { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Can't exceed 100 Characters")]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        
        [Display(Name = "Project Description")]
        [StringLength(300, ErrorMessage = "Can't exceed 300 Characters")]
        [DataType(DataType.MultilineText)]
        public string ProjectDescription { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Project Start Date")]
        public string ProjectDate { get; set; }

        //public ICollection<Tasks> Tasks { get; set; }

    }
}
