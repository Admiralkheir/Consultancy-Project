namespace OfficeAppointmentSoftware.DAL.Contexts
{
    using Microsoft.EntityFrameworkCore;
    using OfficeAppointmentSoftware.Entities.Entities;

    public partial class AppointmentDBContext : DbContext
    {
        public AppointmentDBContext(DbContextOptions<AppointmentDBContext> options) : base(options)
        {
        }

        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Banner> Banner { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Expert> Expert { get; set; }
        public virtual DbSet<Features> Features { get; set; }
        public virtual DbSet<Plans> Plans { get; set; }
        public virtual DbSet<Works> Works { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .HasMany(e => e.Appointment)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Client>()
                .HasMany(e => e.Contact)
                .WithOne(e => e.Client)
                .HasForeignKey(e => e.ClientID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Client)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Expert)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .HasMany(e => e.Contact)
                .WithOne(e => e.Company)
                .HasForeignKey(e => e.CompanyID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Contact>()
                .Property(e => e.PhoneNumber1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.PhoneNumber2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Expert>()
                .Property(e => e.Profession)
                .IsUnicode(false);

            modelBuilder.Entity<Expert>()
                .HasMany(e => e.Contact)
                .WithOne(e => e.Expert)
                .HasForeignKey(e => e.ExpertID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Expert>()
                .HasMany(e => e.Appointment)
                .WithOne(e => e.Expert)
                .HasForeignKey(e => e.ExpertID)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder.Entity<Plans>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Plans>()
                .HasMany(e => e.Company)
                .WithOne(e => e.Plans)
                .HasForeignKey(e => e.PlanID)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
