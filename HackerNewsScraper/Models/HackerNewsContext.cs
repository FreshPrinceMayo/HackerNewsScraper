using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HackerNewsScraper.Models
{
    public partial class HackerNewsContext : DbContext
    {
        public HackerNewsContext()
        {
        }

        public HackerNewsContext(DbContextOptions<HackerNewsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogArticle> BlogArticle { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<CommentBlog> CommentBlog { get; set; }
        public virtual DbSet<CommentText> CommentText { get; set; }
        public virtual DbSet<CommentUrl> CommentUrl { get; set; }
        public virtual DbSet<Story> Story { get; set; }
        public virtual DbSet<StoryBlog> StoryBlog { get; set; }
        public virtual DbSet<StoryProcessed> StoryProcessed { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=localhost;Database=HackerNews;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogArticle>(entity =>
            {
                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.BlogArticle)
                    .HasForeignKey(d => d.BlogId)
                    .HasConstraintName("FK_BlogArticle_Blog");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.CommentId).ValueGeneratedNever();

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.Comment)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comments_Comments");
            });

            modelBuilder.Entity<CommentBlog>(entity =>
            {
                entity.HasKey(e => new { e.CommentId, e.BlogId });
            });

            modelBuilder.Entity<CommentText>(entity =>
            {
                entity.HasKey(e => e.CommentId);

                entity.Property(e => e.CommentId).ValueGeneratedNever();

                entity.HasOne(d => d.Comment)
                    .WithOne(p => p.CommentText)
                    .HasForeignKey<CommentText>(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentText_CommentText");
            });

            modelBuilder.Entity<CommentUrl>(entity =>
            {
                entity.Property(e => e.Url).IsRequired();

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentUrl)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentUrl_Comment");
            });

            modelBuilder.Entity<Story>(entity =>
            {
                entity.Property(e => e.StoryId).ValueGeneratedNever();

                entity.Property(e => e.Type).HasMaxLength(50);
            });

            modelBuilder.Entity<StoryBlog>(entity =>
            {
                entity.HasKey(e => new { e.StoryId, e.BlogId });

                entity.HasOne(d => d.Blog)
                    .WithMany(p => p.StoryBlog)
                    .HasForeignKey(d => d.BlogId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoryBlog_Blog");

                entity.HasOne(d => d.Story)
                    .WithMany(p => p.StoryBlog)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoryBlog_Story");
            });

            modelBuilder.Entity<StoryProcessed>(entity =>
            {
                entity.HasOne(d => d.Story)
                    .WithMany(p => p.StoryProcessed)
                    .HasForeignKey(d => d.StoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StoryProcessed_Story");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
