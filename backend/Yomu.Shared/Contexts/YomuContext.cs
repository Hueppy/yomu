using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Yomu.Shared.Models;

namespace Yomu.Shared.Contexts
{
    public partial class YomuContext : DbContext
    {
        public YomuContext()
        {
        }

        public YomuContext(DbContextOptions<YomuContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Community> Communities { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Login> Logins { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostRating> PostRatings { get; set; } = null!;
        public virtual DbSet<Report> Reports { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserCommunity> UserCommunities { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            var server = Environment.GetEnvironmentVariable("DB_SERVER") ?? "localhost";
            var user = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "example";
            var database = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "yomu";
            
            optionsBuilder.UseMySql($"server={server};user={user};password={password};database={database}", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.5-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("pk_comment_parent");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("pk_comment_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("pk_comment_user");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("fk_post_image_post");
            });

            modelBuilder.Entity<Login>(entity =>
            {
                entity.HasKey(e => e.Email)
                    .HasName("PRIMARY");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MessageReceivers)
                    .HasForeignKey(d => d.ReceiverId)
                    .HasConstraintName("fk_message_receiver");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSenders)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("fk_message_sender");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasOne(d => d.Community)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CommunityId)
                    .HasConstraintName("fk_post_community");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_post_user");
            });

            modelBuilder.Entity<PostRating>(entity =>
            {
                entity.HasKey(e => new { e.PostId, e.UserId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostRatings)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("fk_post_rating_post");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PostRatings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_post_rating_user");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_report_sender");

                entity.HasMany(d => d.Comments)
                    .WithMany(p => p.Reports)
                    .UsingEntity<Dictionary<string, object>>(
                        "ReportComment",
                        l => l.HasOne<Comment>().WithMany().HasForeignKey("CommentId").HasConstraintName("fk_report_comment_comment"),
                        r => r.HasOne<Report>().WithMany().HasForeignKey("ReportId").HasConstraintName("fk_report_comment_report"),
                        j =>
                        {
                            j.HasKey("ReportId", "CommentId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("report_comment");

                            j.HasIndex(new[] { "CommentId" }, "fk_report_comment_comment");

                            j.IndexerProperty<int>("ReportId").HasColumnType("int(11)").HasColumnName("report_id");

                            j.IndexerProperty<int>("CommentId").HasColumnType("int(11)").HasColumnName("comment_id");
                        });

                entity.HasMany(d => d.Posts)
                    .WithMany(p => p.Reports)
                    .UsingEntity<Dictionary<string, object>>(
                        "ReportPost",
                        l => l.HasOne<Post>().WithMany().HasForeignKey("PostId").HasConstraintName("fk_report_post_post"),
                        r => r.HasOne<Report>().WithMany().HasForeignKey("ReportId").HasConstraintName("fk_report_post_report"),
                        j =>
                        {
                            j.HasKey("ReportId", "PostId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("report_post");

                            j.HasIndex(new[] { "PostId" }, "fk_report_post_post");

                            j.IndexerProperty<int>("ReportId").HasColumnType("int(11)").HasColumnName("report_id");

                            j.IndexerProperty<int>("PostId").HasColumnType("int(11)").HasColumnName("post_id");
                        });

                entity.HasMany(d => d.Users)
                    .WithMany(p => p.ReportsNavigation)
                    .UsingEntity<Dictionary<string, object>>(
                        "ReportUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserId").HasConstraintName("fk_report_user_user"),
                        r => r.HasOne<Report>().WithMany().HasForeignKey("ReportId").HasConstraintName("fk_report_user_report"),
                        j =>
                        {
                            j.HasKey("ReportId", "UserId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("report_user");

                            j.HasIndex(new[] { "UserId" }, "fk_report_user_user");

                            j.IndexerProperty<int>("ReportId").HasColumnType("int(11)").HasColumnName("report_id");

                            j.IndexerProperty<string>("UserId").HasMaxLength(50).HasColumnName("user_id");
                        });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasOne(d => d.EmailNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Email)
                    .HasConstraintName("fk_user_login");

                entity.HasMany(d => d.Befrienders)
                    .WithMany(p => p.Friends)
                    .UsingEntity<Dictionary<string, object>>(
                        "Friend",
                        l => l.HasOne<User>().WithMany().HasForeignKey("BefrienderId").HasConstraintName("fk_friend_befriender"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("FriendId").HasConstraintName("fk_friend_friend"),
                        j =>
                        {
                            j.HasKey("BefrienderId", "FriendId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("friend");

                            j.HasIndex(new[] { "FriendId" }, "fk_friend_friend");

                            j.IndexerProperty<string>("BefrienderId").HasMaxLength(50).HasColumnName("befriender_id");

                            j.IndexerProperty<string>("FriendId").HasMaxLength(50).HasColumnName("friend_id");
                        });

                entity.HasMany(d => d.Blockees)
                    .WithMany(p => p.Blockers)
                    .UsingEntity<Dictionary<string, object>>(
                        "Block",
                        l => l.HasOne<User>().WithMany().HasForeignKey("BlockeeId").HasConstraintName("fk_block_blockee"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("BlockerId").HasConstraintName("fk_block_blocker"),
                        j =>
                        {
                            j.HasKey("BlockerId", "BlockeeId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("block");

                            j.HasIndex(new[] { "BlockeeId" }, "fk_block_blockee");

                            j.IndexerProperty<string>("BlockerId").HasMaxLength(50).HasColumnName("blocker_id");

                            j.IndexerProperty<string>("BlockeeId").HasMaxLength(50).HasColumnName("blockee_id");
                        });

                entity.HasMany(d => d.Blockers)
                    .WithMany(p => p.Blockees)
                    .UsingEntity<Dictionary<string, object>>(
                        "Block",
                        l => l.HasOne<User>().WithMany().HasForeignKey("BlockerId").HasConstraintName("fk_block_blocker"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("BlockeeId").HasConstraintName("fk_block_blockee"),
                        j =>
                        {
                            j.HasKey("BlockerId", "BlockeeId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("block");

                            j.HasIndex(new[] { "BlockeeId" }, "fk_block_blockee");

                            j.IndexerProperty<string>("BlockerId").HasMaxLength(50).HasColumnName("blocker_id");

                            j.IndexerProperty<string>("BlockeeId").HasMaxLength(50).HasColumnName("blockee_id");
                        });

                entity.HasMany(d => d.Friends)
                    .WithMany(p => p.Befrienders)
                    .UsingEntity<Dictionary<string, object>>(
                        "Friend",
                        l => l.HasOne<User>().WithMany().HasForeignKey("FriendId").HasConstraintName("fk_friend_friend"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("BefrienderId").HasConstraintName("fk_friend_befriender"),
                        j =>
                        {
                            j.HasKey("BefrienderId", "FriendId").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("friend");

                            j.HasIndex(new[] { "FriendId" }, "fk_friend_friend");

                            j.IndexerProperty<string>("BefrienderId").HasMaxLength(50).HasColumnName("befriender_id");

                            j.IndexerProperty<string>("FriendId").HasMaxLength(50).HasColumnName("friend_id");
                        });
            });

            modelBuilder.Entity<UserCommunity>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.CommunityId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.HasOne(d => d.Community)
                    .WithMany(p => p.UserCommunities)
                    .HasForeignKey(d => d.CommunityId)
                    .HasConstraintName("fk_user_community_community");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCommunities)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_user_community_user");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
