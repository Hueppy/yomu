using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("image")]
    [Index(nameof(PostId), Name = "fk_post_image_post")]
    public partial class Image
    {
        [Key]
        [Column("id")]
        [StringLength(50)]
        public string Id { get; set; } = null!;
        [Column("post_id", TypeName = "int(11)")]
        public int PostId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(PostId))]
        [InverseProperty("Images")]
        public virtual Post Post { get; set; } = null!;
    }
}
