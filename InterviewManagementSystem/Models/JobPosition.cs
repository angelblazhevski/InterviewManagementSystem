using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewManagementSystem.Models
{
	public class JobPosition
	{
        public int Id { get; set; }

        [Required]
        [Display(Name = "Title of the job")]
        public string Title { get; set; }

        public string Requirements { get; set; }

        public string Status { get; set; } = "Open";

        public int MaxPositionsOpen { get; set; }

        public ICollection<Candidate> Candidates { get; set; }
    }
}