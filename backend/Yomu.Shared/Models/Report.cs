using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("report")]
    [Index(nameof(SenderId), Name = "fk_report_sender")]
    public partial class Report
    {
        public Report()
        {
            Comments = new HashSet<Comment>();
            Posts = new HashSet<Post>();
            Users = new HashSet<User>();
        }

        [Key]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("sender_id")]
        [StringLength(50)]
        public string? SenderId { get; set; }
        [Column("comment", TypeName = "text")]
        public string Comment { get; set; } = null!;
        [Column("reason", TypeName = "int(11)")]
        public int Reason { get; set; }
        [Column("send_at", TypeName = "datetime")]
        public DateTime SendAt { get; set; }

        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(User.Reports))]
        public virtual User? Sender { get; set; }

        [ForeignKey("ReportId")]
        [InverseProperty("Reports")]
        public virtual ICollection<Comment> Comments { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty(nameof(Post.Reports))]
        public virtual ICollection<Post> Posts { get; set; }
        [ForeignKey("ReportId")]
        [InverseProperty(nameof(User.ReportsNavigation))]
        public virtual ICollection<User> Users { get; set; }
    }
}
