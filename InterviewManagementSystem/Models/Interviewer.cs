using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewManagementSystem.Models
{
	public class Interviewer
	{
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Interview> Interviews { get; set; }
    }
}