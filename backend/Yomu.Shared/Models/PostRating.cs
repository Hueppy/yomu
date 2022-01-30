using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("post_rating")]
    [Index(nameof(UserId), Name = "fk_post_rating_user")]
    public partial class PostRating
    {
        [Key]
        [Column("post_id", TypeName = "int(11)")]
        public int PostId { get; set; }
        [Key]
        [Column("user_id")]
        [StringLength(50)]
        public string UserId { get; set; } = null!;
        [Column("rating", TypeName = "int(11)")]
        public Rating Rating { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PostId))]
        [InverseProperty("PostRatings")]
        public virtual Post Post { get; set; } = null!;
        [JsonIgnore]
        [ForeignKey(nameof(UserId))]
        [InverseProperty("PostRatings")]
        public virtual User User { get; set; } = null!;
    }
}
