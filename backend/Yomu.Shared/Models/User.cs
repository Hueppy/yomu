using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("user")]
    [Index(nameof(Email), Name = "fk_user_login")]
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            MessageReceivers = new HashSet<Message>();
            MessageSenders = new HashSet<Message>();
            PostRatings = new HashSet<PostRating>();
            Posts = new HashSet<Post>();
            Reports = new HashSet<Report>();
            UserCommunities = new HashSet<UserCommunity>();
            Befrienders = new HashSet<User>();
            Blockees = new HashSet<User>();
            Blockers = new HashSet<User>();
            Friends = new HashSet<User>();
            ReportsNavigation = new HashSet<Report>();
        }

        [Key]
        [Column("id")]
        [StringLength(50)]
        public string Id { get; set; } = null!;
        [Column("email")]
        [StringLength(50)]
        public string Email { get; set; } = null!;
        [Column("banned_until", TypeName = "datetime")]
        public DateTime BannedUntil { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(Email))]
        [InverseProperty(nameof(Login.Users))]
        public virtual Login? EmailNavigation { get; set; } = null!;
        [JsonIgnore]
        [InverseProperty(nameof(Comment.User))]
        public virtual ICollection<Comment> Comments { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(Message.Receiver))]
        public virtual ICollection<Message> MessageReceivers { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(Message.Sender))]
        public virtual ICollection<Message> MessageSenders { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(PostRating.User))]
        public virtual ICollection<PostRating> PostRatings { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(Post.User))]
        public virtual ICollection<Post> Posts { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(Report.Sender))]
        public virtual ICollection<Report> Reports { get; set; }
        [JsonIgnore]
        [InverseProperty(nameof(UserCommunity.User))]
        public virtual ICollection<UserCommunity> UserCommunities { get; set; }

        [JsonIgnore]
        [ForeignKey("FriendId")]
        [InverseProperty(nameof(User.Friends))]
        public virtual ICollection<User> Befrienders { get; set; }
        [JsonIgnore]
        [ForeignKey("BlockerId")]
        [InverseProperty(nameof(User.Blockers))]
        public virtual ICollection<User> Blockees { get; set; }
        [JsonIgnore]
        [ForeignKey("BlockeeId")]
        [InverseProperty(nameof(User.Blockees))]
        public virtual ICollection<User> Blockers { get; set; }
        [JsonIgnore]
        [ForeignKey("BefrienderId")]
        [InverseProperty(nameof(User.Befrienders))]
        public virtual ICollection<User> Friends { get; set; }
        [JsonIgnore]
        [ForeignKey("UserId")]
        [InverseProperty(nameof(Report.Users))]
        public virtual ICollection<Report> ReportsNavigation { get; set; }
    }
}
