using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CinemaluxAPI.DAL.CinemaluxCatalogue.Models;

#nullable disable

namespace CinemaluxAPI.DAL.CinemaluxCatalogue
{
    public partial class CinemaluxDbContext : DbContext
    {

        private readonly IConfiguration _configuration;
        
        public CinemaluxDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CinemaluxDbContext(DbContextOptions<CinemaluxDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DiscountItem> DiscountItems { get; set; }
        public virtual DbSet<DiscountType> DiscountTypes { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Hall> Halls { get; set; }
        public virtual DbSet<CinemaluxMovie> Movies { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<ReservationType> ReservationTypes { get; set; }
        public virtual DbSet<Screening> Screenings { get; set; }
        public virtual DbSet<Ticket> Tickets { get; set; }
        public virtual DbSet<TicketType> TicketTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
               optionsBuilder.UseSqlServer(_configuration.GetConnectionString("CinemaluxCatalogue"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<DiscountItem>(entity =>
            {
                entity.ToTable("discountItem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.DiscountTypeCode)
                    .HasMaxLength(16)
                    .HasColumnName("discountTypeCode");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.OrderId).IsRequired().HasColumnName("orderId");

                entity.HasOne(d => d.DiscountType)
                    .WithMany(p => p.DiscountItems)
                    .HasForeignKey(d => d.DiscountTypeCode)
                    .HasConstraintName("discountItem_discountType_code_fk");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DiscountItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("discountItem_order_id_fk");
            });

            modelBuilder.Entity<DiscountType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("discountType_pk");

                entity.ToTable("discountType");

                entity.Property(e => e.Code)
                    .HasMaxLength(16)
                    .HasColumnName("code");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description)
                    .HasMaxLength(32)
                    .HasColumnName("description");

                entity.Property(e => e.DiscountPct).HasColumnName("discountPct");

                entity.Property(e => e.ExpiresOn)
                    .HasColumnType("datetime")
                    .HasColumnName("expiresOn");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("employee");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("address");
                
                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.BornAt)
                    .HasColumnType("date")
                    .HasColumnName("bornAt");

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("contactPhone");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("email");

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("key");

                entity.Property(e => e.ManagerId).HasColumnName("managerId");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("password");

                entity.Property(e => e.Role).HasColumnName("role");

                entity.Property(e => e.Salary).HasColumnName("salary");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("surname");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("username");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.InverseManager)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("employee_manager_id_fk");
            });

            modelBuilder.Entity<Hall>(entity =>
            {
                entity.ToTable("hall");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Capacity)
                    .IsRequired()
                    .HasColumnName("capacity");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.Property(e => e.SeatValidityRegex)
                    .HasMaxLength(20)
                    .HasColumnName("seatValidityRegex")
                    .HasDefaultValueSql("('/^([A-Z][0-9][0-9]))$\\g')");
            });

            modelBuilder.Entity<CinemaluxMovie>(entity =>
            {
                entity.ToTable("movie");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AgeRating).HasColumnName("ageRating");
                entity.Property(e => e.Genres).HasColumnName("genres");
                entity.Property(e => e.AverageRating).HasColumnName("averageRating");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.BackdropImageURL).HasColumnName("backdropImageURL");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).HasMaxLength(360).HasColumnName("description");

                entity.Property(e => e.Has3D).HasColumnName("has3D");

                entity.Property(e => e.HasLocalAudio).HasColumnName("hasLocalAudio");

                entity.Property(e => e.HasLocalSubtitles).HasColumnName("hasLocalSubtitles");

                entity.Property(e => e.ImageURL).HasColumnName("imageURL");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.OMovieId).HasColumnName("oMovieId");

                entity.Property(e => e.OverviewLinks)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasColumnName("overviewLinks");

                entity.Property(e => e.ProfitPercentageShare).HasColumnName("profitPercentageShare");

                entity.Property(e => e.ReleaseYear)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .HasColumnName("releaseYear");

                entity.Property(e => e.RunningTimeInMinutes).HasColumnName("runningTimeInMinutes");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("title");

                entity.Property(e => e.VideoLinks)
                    .IsUnicode(false)
                    .HasColumnName("videoLinks");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("order");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.EmployeeId).HasColumnName("employeeId");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.PaymentTypeCode)
                    .HasMaxLength(16)
                    .HasColumnName("paymentTypeCode");

                entity.Property(e => e.ReservationId).HasColumnName("reservationId");

                entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.EmployeeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("order_employee_id_fk");

                entity.HasOne(d => d.PaymentType)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentTypeCode)
                    .HasConstraintName("order_paymentType_code_fk");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("order_reservation_id_fk");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("orderItem");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.ModifiedAt)
                    .IsRequired()
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.OrderTypeCode)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("orderTypeCode");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("orderItem_order_id_fk");

                entity.HasOne(d => d.OrderType)
                    .WithMany(p => p.OrderItems)
                    .HasForeignKey(d => d.OrderTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("orderItem_orderType_code_fk");
            });

            modelBuilder.Entity<OrderType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("orderType_pk");

                entity.ToTable("orderType");

                entity.Property(e => e.Code)
                    .HasMaxLength(16)
                    .HasColumnName("code");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("description");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<PaymentType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("paymentType_pk");

                entity.ToTable("paymentType");

                entity.Property(e => e.Code)
                    .HasMaxLength(16)
                    .HasColumnName("code");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .HasColumnName("name");
            });

              modelBuilder.Entity<Reservation>(entity =>
            {
                entity.ToTable("reservation");

                entity.Property(e => e.Id).HasColumnName("id");
                
                entity.Property(e => e.ScreeningId)
                    .IsRequired()
                    .HasColumnName("screeningId");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("contactPhone");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsPaid).HasColumnName("isPaid");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.Property(e => e.ReservationTypeCode)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("reservationTypeCode");

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("surname");

                entity.HasOne(d => d.ReservationType)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ReservationTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("reservation_reservationType_code_fk");
                
                entity.HasOne(d => d.Screening)
                    .WithMany(p => p.Reservations)
                    .HasForeignKey(d => d.ScreeningId)
                    .HasConstraintName("reservation_screening_id_fk");
            });

            modelBuilder.Entity<ReservationType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("reservationType_pk");

                entity.ToTable("reservationType");

                entity.Property(e => e.Code)
                    .HasMaxLength(16)
                    .HasColumnName("code");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Screening>(entity =>
            {
                entity.ToTable("screening");

                entity.Property(e => e.Id).HasColumnName("id");

                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.DefaultTicketTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("DefaultTicketTypeCode");

                entity.Property(e => e.HallId).HasColumnName("hallId");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");
                
                entity.Property(e => e.Is3D).IsRequired().HasColumnName("is3D");
                
                entity.Property(e => e.HasLocalAudio).IsRequired().HasColumnName("hasLocalAudio");
                
                entity.Property(e => e.HasLocalSubtitles).IsRequired().HasColumnName("hasLocalSubtitles");

                entity.Property(e => e.MovieId).HasColumnName("movieId");

                entity.Property(e => e.ScreeningTime).HasColumnName("screeningTime");

                entity.HasOne(d => d.DefaultTicketType)
                    .WithMany(p => p.Screenings)
                    .HasForeignKey(d => d.DefaultTicketTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("screening_ticketType_fk");

                entity.HasOne(d => d.Hall)
                    .WithMany(p => p.Screenings)
                    .HasForeignKey(d => d.HallId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("screening_hall_id_fk");

                entity.HasOne(d => d.CinemaluxMovie)
                    .WithMany(p => p.Screenings)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("screening_movie_id_fk");
            });


            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.ToTable("ticket");

                entity.Property(e => e.Id).HasColumnName("id");
                
                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.IsUsed).HasColumnName("isUsed");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.OrderId).HasColumnName("orderId");

                entity.Property(e => e.ReservationId).HasColumnName("reservationId");

                entity.Property(e => e.ScreeningId).HasColumnName("screeningId");

                entity.Property(e => e.SeatLabel)
                    .HasMaxLength(5)
                    .HasColumnName("seatLabel");

                entity.Property(e => e.TicketTypeCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ticketTypeCode");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticket_order_id_fk");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ReservationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("ticket_reservation_id_fk");

                entity.HasOne(d => d.Screening)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.ScreeningId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticket_screening_id_fk");

                entity.HasOne(d => d.TicketType)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.TicketTypeCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ticket_ticketType_id_fk");
            });

            modelBuilder.Entity<TicketType>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("ticketType_pk")
                    .IsClustered(false);

                entity.ToTable("ticketType");

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .HasColumnName("code");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                // Soft delete
                entity.HasQueryFilter(e => e.ArchivedAt == null);

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description)
                    .HasMaxLength(20)
                    .HasColumnName("description");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
