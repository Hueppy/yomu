using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Yomu.Shared.Models
{
    [Table("message")]
    [Index(nameof(ReceiverId), Name = "fk_message_receiver")]
    [Index(nameof(SenderId), Name = "fk_message_sender")]
    public partial class Message
    {
        [Key]
        [Column("id", TypeName = "int(11)")]
        public int Id { get; set; }
        [Column("sender_id")]
        [StringLength(50)]
        public string SenderId { get; set; } = null!;
        [Column("receiver_id")]
        [StringLength(50)]
        public string ReceiverId { get; set; } = null!;
        [Column("message", TypeName = "text")]
        public string Message1 { get; set; } = null!;
        [Column("send_at", TypeName = "datetime")]
        public DateTime SendAt { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        [InverseProperty(nameof(User.MessageReceivers))]
        public virtual User Receiver { get; set; } = null!;
        [ForeignKey(nameof(SenderId))]
        [InverseProperty(nameof(User.MessageSenders))]
        public virtual User Sender { get; set; } = null!;
    }
}
