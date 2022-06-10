using System;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DL
{
    public partial class VolunteersContext : DbContext
    {
        public VolunteersContext()
        {
        }

        public VolunteersContext(DbContextOptions<VolunteersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Drive> Drive { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<DriverRequest> DriverRequest { get; set; }
        public virtual DbSet<PassengerRequest> PassengerRequest { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=srv2\\pupils;Database=Volunteers;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("mbydomain\\324102417")
                .HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<Drive>(entity =>
            {
                entity.ToTable("Drive", "dbo");

                entity.Property(e => e.DriveId).HasColumnName("driveId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.DriverRequestId).HasColumnName("driverRequestId");

                entity.Property(e => e.IsApproved).HasColumnName("isApproved");

                entity.Property(e => e.PassengerRequestId).HasColumnName("passengerRequestId");

                entity.HasOne(d => d.DriverRequest)
                    .WithMany(p => p.Drives)
                    .HasForeignKey(d => d.DriverRequestId)
                    .HasConstraintName("FK_Drive_DriverRequest");

                entity.HasOne(d => d.PassengerRequest)
                    .WithMany(p => p.Drives)
                    .HasForeignKey(d => d.PassengerRequestId)
                    .HasConstraintName("FK_Drive_PassengerRequest");
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.ToTable("Driver", "dbo");

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.DriverLicense)
                    .HasMaxLength(50)
                    .HasColumnName("driverLicense");

                entity.Property(e => e.IsActive).HasColumnName("isActive");

                entity.Property(e => e.IsHandicappedCar).HasColumnName("isHandicappedCar");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Drivers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Driver_User");
            });

            modelBuilder.Entity<DriverRequest>(entity =>
            {
                entity.ToTable("DriverRequest", "dbo");

                entity.Property(e => e.DestinationStreet).HasMaxLength(50);

                entity.Property(e => e.DriverId).HasColumnName("driverId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("date")
                    .HasColumnName("endDate");

                entity.Property(e => e.LeavingHour).HasColumnType("datetime");

                entity.Property(e => e.NumOfSeats).HasColumnName("numOfSeats");

                entity.Property(e => e.SourceStreet).HasMaxLength(50);

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.HasOne(d => d.Driver)
                    .WithMany(p => p.DriverRequests)
                    .HasForeignKey(d => d.DriverId)
                    .HasConstraintName("FK_DriverRequest_Driver");
            });

            modelBuilder.Entity<PassengerRequest>(entity =>
            {
                entity.ToTable("PassengerRequest", "dbo");

                entity.Property(e => e.PassengerRequestId).HasColumnName("passengerRequestId");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.DestinationStreet)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("destinationStreet");

                entity.Property(e => e.IsHandicapped).HasColumnName("isHandicapped");

                entity.Property(e => e.IsValid).HasColumnName("isValid");

                entity.Property(e => e.NumOfSeats).HasColumnName("numOfSeats");

                entity.Property(e => e.SourceStreet)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("sourceStreet");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PassengerRequests)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PassengerRequest_User");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "dbo");

                entity.Property(e => e.PersonId).HasColumnName("personId");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .HasColumnName("fullName");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.IdentityNumber)
                    .HasMaxLength(50)
                    .HasColumnName("identityNumber");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.Salt)
                    .HasMaxLength(50)
                    .HasColumnName("salt");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("RATING", "dbo");

                entity.Property(e => e.RatingId).HasColumnName("RATING_ID");

                entity.Property(e => e.Host)
                    .HasMaxLength(50)
                    .HasColumnName("HOST");

                entity.Property(e => e.Method)
                    .HasMaxLength(10)
                    .HasColumnName("METHOD")
                    .IsFixedLength(true);

                entity.Property(e => e.Path)
                    .HasMaxLength(50)
                    .HasColumnName("PATH");

                entity.Property(e => e.RecordDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Record_Date");

                entity.Property(e => e.Referer)
                    .HasMaxLength(100)
                    .HasColumnName("REFERER");

                entity.Property(e => e.UserAgent).HasColumnName("USER_AGENT");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "dbo");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.PersonId).HasColumnName("personId");

                entity.Property(e => e.TypeId).HasColumnName("typeId");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Person1");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Type");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK_type");

                entity.ToTable("UserType", "dbo");

                entity.Property(e => e.TypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("typeId");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("description");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
