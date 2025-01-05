using System;
using System.Collections.Generic;
using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public partial class MessageDbContext : DbContext
{
    public MessageDbContext()
    {
    }

    public MessageDbContext(DbContextOptions<MessageDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Messages> Messages { get; set; }

    public virtual DbSet<MessageSending> MessageSendings { get; set; }

    public virtual DbSet<TwilioCredentials> TwilioCredentials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Messages>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("PK__Messages__C87C0C9C18713958");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Message).HasColumnName("Message");
            entity.Property(e => e.Recipient).HasMaxLength(255);
        });

        modelBuilder.Entity<MessageSending>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MessageS__3214EC078AB164E5");

            entity.Property(e => e.ConfirmationCode).HasMaxLength(50);
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Message).WithMany(p => p.MessageSendings)
                .HasForeignKey(d => d.MessageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MessageSe__Messa__286302EC");
        });

        modelBuilder.Entity<TwilioCredentials>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TwilioCr__3214EC07B8C302F4");

            entity.Property(e => e.Accountid)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AuthToken)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FromNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
