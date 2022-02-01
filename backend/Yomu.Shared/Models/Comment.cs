using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("comment")]
    [Index(nameof(ParentId), Name = "pk_comment_parent")]
    [Index(nameof(PostId), Name = "pk_comment_post")]
    [Index(nameof(UserId), Name = "pk_comment_user")]
    public partial class Comment
    {
        public Comment()
        {
            InverseParent = new HashSet<Comment>();
            Reports = new HashSet<Report>();
        }

        [Key]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("post_id", TypeName = "int(11)")]
        public int PostId { get; set; }
        [Column("user_id")]
        [StringLength(50)]
        public string? UserId { get; set; }
        [Column("parent_id", TypeName = "int(11)")]
        public int? ParentId { get; set; }
        [Column("message", TypeName = "text")]
        public string Message { get; set; } = null!;

        [JsonIgnore]
        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Comment.InverseParent))]
        public virtual Comment? Parent { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(PostId))]
        [InverseProperty("Comments")]
        public virtual Post? Post { get; set; } = null!;
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Comments")]
        public virtual User? User { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(Comment.Parent))]
        public virtual ICollection<Comment> InverseParent { get; set; }

        [JsonIgnore]
        [ForeignKey("CommentId")]
        [InverseProperty(nameof(Report.Comments))]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
