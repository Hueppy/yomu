using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("community")]
    public partial class Community
    {
        public Community()
        {
            Posts = new HashSet<Post>();
            UserCommunities = new HashSet<UserCommunity>();
        }

        [Key]
        [Column("id")]
        [StringLength(50)]
        public string Id { get; set; } = null!;
        [Column("description", TypeName = "text")]
        public string Description { get; set; } = null!;

        [InverseProperty(nameof(Post.Community))]
        public virtual ICollection<Post> Posts { get; set; }
        [InverseProperty(nameof(UserCommunity.Community))]
        public virtual ICollection<UserCommunity> UserCommunities { get; set; }
    }
}
