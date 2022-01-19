using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("user_community")]
    [Index(nameof(CommunityId), Name = "fk_user_community_community")]
    public partial class UserCommunity
    {
        [Key]
        [Column("user_id")]
        [StringLength(50)]
        public string UserId { get; set; } = null!;
        [Key]
        [Column("community_id")]
        [StringLength(50)]
        public string CommunityId { get; set; } = null!;
        [Column("role", TypeName = "int(11)")]
        public int Role { get; set; }

        [ForeignKey(nameof(CommunityId))]
        [InverseProperty("UserCommunities")]
        public virtual Community Community { get; set; } = null!;
        [ForeignKey(nameof(UserId))]
        [InverseProperty("UserCommunities")]
        public virtual User User { get; set; } = null!;
    }
}
