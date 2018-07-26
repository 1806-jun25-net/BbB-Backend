using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BbB.Data
{
    public partial class BbBContext : DbContext
    {
        public BbBContext()
        {
        }

        public BbBContext(DbContextOptions<BbBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ArchiveDrive> ArchiveDrive { get; set; }
        public virtual DbSet<ArchiveItem> ArchiveItem { get; set; }
        public virtual DbSet<ArchiveOrder> ArchiveOrder { get; set; }
        public virtual DbSet<ArchiveUserJoin> ArchiveUserJoin { get; set; }
        public virtual DbSet<Destination> Destination { get; set; }
        public virtual DbSet<Drive> Drive { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<DriverReview> DriverReview { get; set; }
        public virtual DbSet<MenuItem> MenuItem { get; set; }
        public virtual DbSet<Msg> Msg { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<UserJoin> UserJoin { get; set; }
        public virtual DbSet<UserPickup> UserPickup { get; set; }
        public virtual DbSet<UserReview> UserReview { get; set; }
        public virtual DbSet<Usr> Usr { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArchiveDrive>(entity =>
            {
                entity.Property(e => e.Dtime)
                    .HasColumnName("DTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dtype)
                    .HasColumnName("DType")
                    .HasMaxLength(16);

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.ArchiveDrive)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_DestinationId");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.ArchiveDrive)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_DriverId");
            });

            modelBuilder.Entity<ArchiveItem>(entity =>
            {
                entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ItemName).HasMaxLength(32);

                entity.Property(e => e.Msg).HasMaxLength(255);

                entity.HasOne(d => d.ArchiveOrder)
                    .WithMany(p => p.ArchiveItem)
                    .HasForeignKey(d => d.ArchiveOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArchiveItem");
            });

            modelBuilder.Entity<ArchiveOrder>(entity =>
            {
                entity.HasOne(d => d.ArchiveDrive)
                    .WithMany(p => p.ArchiveOrder)
                    .HasForeignKey(d => d.ArchiveDriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_ArchiveOrder");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ArchiveOrder)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_ArchiveOrder");
            });

            modelBuilder.Entity<ArchiveUserJoin>(entity =>
            {
                entity.HasKey(e => new { e.ArchiveDriveId, e.UserId });

                entity.HasOne(d => d.ArchiveDrive)
                    .WithMany(p => p.ArchiveUserJoin)
                    .HasForeignKey(d => d.ArchiveDriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_ArchiveUserJoin");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ArchiveUserJoin)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_ArchiveUserJoin");
            });

            modelBuilder.Entity<Destination>(entity =>
            {
                entity.Property(e => e.StreetAddress).HasMaxLength(30);

                entity.Property(e => e.Title).HasMaxLength(16);
            });

            modelBuilder.Entity<Drive>(entity =>
            {
                entity.Property(e => e.Dtime)
                    .HasColumnName("DTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Dtype)
                    .HasColumnName("DType")
                    .HasMaxLength(16);

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.Drive)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_Drive");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.Drive)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_Drive");
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(e => e.MeetLoc).HasMaxLength(16);

                entity.Property(e => e.Rating).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Driver)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Driver");
            });

            modelBuilder.Entity<DriverReview>(entity =>
            {
                entity.HasOne(d => d.ArchiveDrive)
                    .WithMany(p => p.DriverReview)
                    .HasForeignKey(d => d.ArchiveDriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK3_JoinerReview");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.DriverReview)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_JoinerReview");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DriverReview)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_JoinerReview");
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.Property(e => e.Cost).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ItemName).HasMaxLength(32);

                entity.HasOne(d => d.Destination)
                    .WithMany(p => p.MenuItem)
                    .HasForeignKey(d => d.DestinationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DestinationId");
            });

            modelBuilder.Entity<Msg>(entity =>
            {
                entity.Property(e => e.Dtime)
                    .HasColumnName("DTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Msg1)
                    .HasColumnName("Msg")
                    .HasMaxLength(255);

                entity.HasOne(d => d.Receiver)
                    .WithMany(p => p.MsgReceiver)
                    .HasForeignKey(d => d.ReceiverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_Msg");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MsgSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_Msg");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(e => new { e.OrderId, e.ItemId });

                entity.Property(e => e.Msg).HasMaxLength(255);

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_OrderItem");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItem)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_OrderItem");
            });

            modelBuilder.Entity<UserJoin>(entity =>
            {
                entity.HasKey(e => new { e.DriveId, e.UserId });

                entity.HasOne(d => d.Drive)
                    .WithMany(p => p.UserJoin)
                    .HasForeignKey(d => d.DriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_UserJoin");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserJoin)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_UserJoin");
            });

            modelBuilder.Entity<UserPickup>(entity =>
            {
                entity.HasOne(d => d.Drive)
                    .WithMany(p => p.UserPickup)
                    .HasForeignKey(d => d.DriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_UserPickup");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPickup)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_UserPickup");
            });

            modelBuilder.Entity<UserReview>(entity =>
            {
                entity.HasOne(d => d.ArchiveDrive)
                    .WithMany(p => p.UserReview)
                    .HasForeignKey(d => d.ArchiveDriveId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK3_UserReview");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.UserReview)
                    .HasForeignKey(d => d.DriverId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK1_UserReview");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserReview)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK2_UserReview");
            });

            modelBuilder.Entity<Usr>(entity =>
            {
                entity.HasIndex(e => e.EmailAddress)
                    .HasName("UQ__Usr__49A1474090295A25")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("UQ__Usr__C9F284568CDB8FF3")
                    .IsUnique();

                entity.Property(e => e.Company).HasMaxLength(20);

                entity.Property(e => e.Credit).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.Pass)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(e => e.Rating).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(16);
            });
        }
    }
}
