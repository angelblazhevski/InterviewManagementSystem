using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InterviewManagementSystem.Models
{
	public class Candidate
	{
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string CVPath { get; set; }

        [Required]
        public CandidateStatus Status { get; set; } = CandidateStatus.Applied;
        // Navigation
        public int JobPositionId { get; set; }
        public JobPosition JobPosition { get; set; }

        public ICollection<Interview> Interviews { get; set; }
        public enum CandidateStatus
        {
            Applied,
            Accepted,
            Rejected
        }
    }
}