using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("login")]
    public partial class Login
    {
        public Login()
        {
            Users = new HashSet<User>();
        }

        [Key]
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; } = null!;
        [Column("password_hash")]
        [StringLength(256)]
        public string PasswordHash { get; set; } = null!;
        [Column("role", TypeName = "int(11)")]
        public int Role { get; set; }

        [InverseProperty(nameof(User.EmailNavigation))]
        public virtual ICollection<User> Users { get; set; }
    }
}
