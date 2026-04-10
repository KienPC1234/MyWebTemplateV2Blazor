using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebTemplateV2.Client.Models
{
    public enum SubmissionType
    {
        SangTac,
        BaiThi,
        ThietKe
    }

    public enum SubjectArea
    {
        Van,
        KTPL
    }

    public class Submission
    {
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public string Author { get; set; } = "Học sinh FPT";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public SubmissionType Type { get; set; }
        
        public SubjectArea Subject { get; set; }
        
        public string? ImagePath { get; set; }
        
        public List<Comment> Comments { get; set; } = new();
        public List<Vote> Votes { get; set; } = new();
    }

    public class Comment
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string Author { get; set; } = "Anonymous";
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class Vote
    {
        public int Id { get; set; }
        public int SubmissionId { get; set; }
        public string UserFingerprint { get; set; } = string.Empty; // To prevent multiple votes
        public int Value { get; set; } = 1; // Usually 1
    }

    public class AdminUser
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Admin";
    }

    public class Publication
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = "Ban biên tập";
        public string PdfPath { get; set; } = string.Empty;
        public string? CoverImagePath { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class AiKnowledge
    {
        public int Id { get; set; }
        public string Context { get; set; } = string.Empty;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
