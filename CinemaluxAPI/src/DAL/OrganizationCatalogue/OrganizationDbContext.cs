using CinemaluxAPI.DAL.OrganizationDbContext.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CinemaluxAPI.DAL.OrganizationDbContext
{
    public partial class OrganizationDbContext : DbContext
    {
        public OrganizationDbContext()
        {
        }

        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cinema> Cinemas { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<DiscountType> DiscountTypes { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieGenre> MovieGenres { get; set; }
        public virtual DbSet<MovieReview> MovieReviews { get; set; }
        public virtual DbSet<OrderType> OrderTypes { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<ReservationType> ReservationTypes { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=34.77.211.138;Initial Catalog=organizationCatalogue;User ID=sa;Password=cinemaluxSQL2021;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<Cinema>(entity =>
            {
                entity.ToTable("cinema");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.CityId).HasColumnName("cityId");

                entity.Property(e => e.ClosingTime)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("closingTime");

                entity.Property(e => e.ContactPhone)
                    .IsRequired()
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("contactPhone");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.LocationAddress)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("locationAddress");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.NumberOfHalls).HasColumnName("numberOfHalls");

                entity.Property(e => e.OpeningTime)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false)
                    .HasColumnName("openingTime");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Cinemas)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cinema_city_id_fk");
            });

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.Property(e => e.Population).HasColumnName("population");

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("postalCode");

                entity.Property(e => e.TelephonePrefix)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("telephonePrefix");
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

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("genre_pk");

                entity.ToTable("genre");

                entity.Property(e => e.Code)
                    .HasMaxLength(16)
                    .HasColumnName("code");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Movie>(entity =>
            {
                entity.ToTable("movie");
                
                entity.HasQueryFilter(e => e.ArchivedAt == null);

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.AgeRating).HasColumnName("ageRating");

                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.BackdropImageUrl).HasColumnName("backdropImageURL");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.Description).HasMaxLength(500).HasColumnName("description");

                entity.Property(e => e.Has3D).HasColumnName("has3D");

                entity.Property(e => e.HasLocalAudio).HasColumnName("hasLocalAudio");

                entity.Property(e => e.HasLocalSubtitles).HasColumnName("hasLocalSubtitles");

                entity.Property(e => e.ImageUrl).HasColumnName("imageURL");

                entity.Property(e => e.ModifiedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("modifiedBy")
                    .HasDefaultValueSql("('SYSTEM')");

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

            modelBuilder.Entity<MovieGenre>(entity =>
            {
                entity.ToTable("movieGenre");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GenreCode)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("genreCode");

                entity.Property(e => e.MovieId).HasColumnName("movieId");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.MovieGenres)
                    .HasForeignKey(d => d.GenreCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("movieGenre_genre_code_fk");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieGenres)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("movieGenre_movie_id_fk");
            });

            modelBuilder.Entity<MovieReview>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("table_name_pk");

                entity.ToTable("movieReview");
                
                entity.HasQueryFilter(e => e.ArchivedAt == null);

                entity.HasQueryFilter(e => e.ArchivedAt == null);
                
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ArchivedAt)
                        .HasColumnType("datetime")
                        .HasColumnName("archivedAt");
    
                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                                    
                                    .HasColumnName("archivedBy");
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("createdBy")
                    .HasDefaultValueSql("('SYSTEM')");

                entity.Property(e => e.MovieId).HasColumnName("movieId");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.Review)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("review");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.MovieReviews)
                    .HasForeignKey(d => d.MovieId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("movieReviews_movie_id_fk");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MovieReviews)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("movieReviews_user_id_fk");
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
                    .HasName("PK__paymentT__357D4CF8D7C40E72");

                entity.ToTable("paymentType");

                entity.Property(e => e.Code)
                    .HasMaxLength(16)
                    .HasColumnName("code");

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

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .HasColumnName("name");
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

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.ToTable("user");

                entity.HasQueryFilter(e => e.ArchivedAt == null && e.IsLocked == false);
                
                entity.Property(e => e.ArchivedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("archivedAt");

                entity.Property(e => e.ArchivedBy)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("archivedBy");

                entity.Property(e => e.ContactPhone)
                    .HasMaxLength(16)
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
                    .HasMaxLength(64)
                    .HasColumnName("email");
                
                entity.Property(e => e.IsLocked)
                    .HasColumnName("isLocked");

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
                    .HasMaxLength(128)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Surname)
                    .HasMaxLength(32)
                    .HasColumnName("surname");

                entity.Property(e => e.Username)
                    .HasMaxLength(16)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
