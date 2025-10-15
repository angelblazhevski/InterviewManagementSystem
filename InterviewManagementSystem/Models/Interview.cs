using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewManagementSystem.Models
{
	public class Interview
	{
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public string Notes { get; set; }

        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }

        public int InterviewerId { get; set; }
        public Interviewer Interviewer { get; set; }
    }
}