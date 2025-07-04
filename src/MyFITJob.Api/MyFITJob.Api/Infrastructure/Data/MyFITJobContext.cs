using Microsoft.EntityFrameworkCore;
using MyFITJob.Api.JobOffers.Domain;

namespace MyFITJob.Api.Infrastructure.Data;
public class MyFITJobContext : DbContext
{
    public MyFITJobContext(DbContextOptions<MyFITJobContext> options) : base(options)
    { }

    public DbSet<JobOffer> JobOffers { get; set; }
    public DbSet<Skill> Skills { get; set; }

    // Si tu as d'autres entités, ajoute-les ici
    // public DbSet<AutreEntite> AutresEntites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobOffer>(entity =>
        {
            entity.ToTable("JobOffers");
            
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion(
                    v => v.Name,
                    v => JobOfferStatus.FromName<JobOfferStatus>(v))
                .HasDefaultValueSql($"'{JobOfferStatus.New.Name}'");
            
            var validStatuses = string.Join(",", JobOfferStatus.GetAll<JobOfferStatus>().Select(s => $"'{s.Name}'"));
            entity.HasCheckConstraint("CK_JobOffers_Status", $"\"Status\" IN ({validStatuses})");
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnUpdate();
            
            entity.Property(e => e.CommentsCount)
                .HasDefaultValue(0);

            // Configuration de la relation many-to-many avec Skills
            entity.HasMany(e => e.Skills)
                  .WithMany(e => e.JobOffers)
                  .UsingEntity(j => j.ToTable("JobOfferSkills"));
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.ToTable("Skills");
            
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnUpdate();

            // Index unique pour éviter les doublons de noms
            entity.HasIndex(e => e.Name).IsUnique();
        });
    }
}
