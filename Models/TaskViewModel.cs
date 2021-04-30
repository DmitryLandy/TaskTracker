using BugTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models
{
    public class TaskViewModel
    {
        public IEnumerable<Projects> projRes { get; set; }
        public IEnumerable<Tasks> taskRes { get; set; }
        public IEnumerable<Tasks> taskResFiltered { get; set; }
        public int tasksCompleted { get; set; }
        public int tasksUncompleted { get; set; }
        public int taskCount { get; set; }
        public int projCount { get; set; }
    }
}
