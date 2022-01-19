using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("post")]
    [Index(nameof(CommunityId), Name = "fk_post_community")]
    [Index(nameof(UserId), Name = "fk_post_user")]
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Images = new HashSet<Image>();
            PostRatings = new HashSet<PostRating>();
            Reports = new HashSet<Report>();
        }

        [Key]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("community_id")]
        [StringLength(50)]
        public string CommunityId { get; set; } = null!;
        [Column("user_id")]
        [StringLength(50)]
        public string? UserId { get; set; }
        [Column("text", TypeName = "text")]
        public string Text { get; set; } = null!;
        [Column("link")]
        [StringLength(2048)]
        public string? Link { get; set; }

        [ForeignKey(nameof(CommunityId))]
        [InverseProperty("Posts")]
        public virtual Community Community { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty("Posts")]
        public virtual User? User { get; set; }
        [InverseProperty(nameof(Comment.Post))]
        public virtual ICollection<Comment> Comments { get; set; }
        [InverseProperty(nameof(Image.Post))]
        public virtual ICollection<Image> Images { get; set; }
        [InverseProperty(nameof(PostRating.Post))]
        public virtual ICollection<PostRating> PostRatings { get; set; }

        [ForeignKey("PostId")]
        [InverseProperty(nameof(Report.Posts))]
        public virtual ICollection<Report> Reports { get; set; }
    }
}
