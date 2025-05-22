using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyFITJob.Models;

namespace MyFITJob.DAL;

public class MyFITJobContext : DbContext
{
    public MyFITJobContext(DbContextOptions<MyFITJobContext> options) : base(options)
    { }

    public DbSet<JobOffer> JobOffers { get; set; }

    // Si tu as d'autres entités, ajoute-les ici
    // public DbSet<AutreEntite> AutresEntites { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<JobOffer>(entity =>
        {
            entity.ToTable("JobOffers");
            
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Company).IsRequired();
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
        });
    }
}