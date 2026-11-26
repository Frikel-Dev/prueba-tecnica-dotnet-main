using Microsoft.EntityFrameworkCore;
using PruebaTecnica.BL.BusinessLogic;
using PruebaTecnica.DAL.Entities;

namespace PruebaTecnica.DAL.Data;

public sealed class PropertyManagementDbContext(
    DbContextOptions<PropertyManagementDbContext> options) : DbContext(options)
{
    public DbSet<Property> Properties => Set<Property>();

    public DbSet<Unit> Units => Set<Unit>();

    public DbSet<Tenant> Tenants => Set<Tenant>();

    public DbSet<Lease> Leases => Set<Lease>();

    public DbSet<Technician> Technicians => Set<Technician>();

    public DbSet<MaintenanceRequest> MaintenanceRequests => Set<MaintenanceRequest>();

    public DbSet<Visit> Visits => Set<Visit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Property>(entity =>
        {
            entity.ToTable("Properties");
            entity.HasKey(property => property.Id);
            entity.Property(property => property.Name).HasMaxLength(150).IsRequired();
            entity.Property(property => property.Address).HasMaxLength(300).IsRequired();
            entity.Property(property => property.City).HasMaxLength(100).IsRequired();
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.ToTable("Units", table =>
                table.HasCheckConstraint(
                    "CK_Units_Type",
                    "[Type] IN (N'Department', N'Local')"));
            entity.HasKey(unit => unit.Id);
            entity.Property(unit => unit.Number).HasMaxLength(20).IsRequired();
            entity.Property(unit => unit.Type)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
            entity.HasIndex(unit => new { unit.PropertyId, unit.Number }).IsUnique();
            entity.HasOne(unit => unit.Property)
                .WithMany(property => property.Units)
                .HasForeignKey(unit => unit.PropertyId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.ToTable("Tenants");
            entity.HasKey(tenant => tenant.Id);
            entity.Property(tenant => tenant.FullName).HasMaxLength(200).IsRequired();
            entity.Property(tenant => tenant.Email).HasMaxLength(320).IsRequired();
            entity.Property(tenant => tenant.Phone).HasMaxLength(30).IsRequired();
            entity.HasIndex(tenant => tenant.Email).IsUnique();
        });

        modelBuilder.Entity<Lease>(entity =>
        {
            entity.ToTable("Leases", table =>
            {
                table.HasCheckConstraint(
                    "CK_Leases_EndDateAfterStartDate",
                    "[EndDate] IS NULL OR [EndDate] >= [StartDate]");
                table.HasCheckConstraint(
                    "CK_Leases_MonthlyRentNonNegative",
                    "[MonthlyRent] >= 0");
            });
            entity.HasKey(lease => lease.Id);
            entity.Property(lease => lease.MonthlyRent).HasPrecision(18, 2);
            entity.HasIndex(lease => new { lease.UnitId, lease.StartDate });
            entity.HasIndex(lease => lease.TenantId);
            entity.HasOne(lease => lease.Unit)
                .WithMany(unit => unit.Leases)
                .HasForeignKey(lease => lease.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(lease => lease.Tenant)
                .WithMany(tenant => tenant.Leases)
                .HasForeignKey(lease => lease.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Technician>(entity =>
        {
            entity.ToTable("Technicians", table =>
                table.HasCheckConstraint(
                    "CK_Technicians_Specialty",
                    "[Specialty] IN (N'Plumbing', N'Electricity', N'General')"));
            entity.HasKey(technician => technician.Id);
            entity.Property(technician => technician.FullName).HasMaxLength(200).IsRequired();
            entity.Property(technician => technician.Specialty)
                .HasConversion<string>()
                .HasMaxLength(32)
                .IsRequired();
            entity.Property(technician => technician.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<MaintenanceRequest>(entity =>
        {
            entity.ToTable("MaintenanceRequests", table =>
            {
                table.HasCheckConstraint(
                    "CK_MaintenanceRequests_Priority",
                    "[Priority] IN (N'Low', N'Medium', N'High', N'Urgent')");
                table.HasCheckConstraint(
                    "CK_MaintenanceRequests_Status",
                    "[Status] IN (N'Open', N'Assigned', N'InProgress', N'Closed', N'Cancelled')");
                table.HasCheckConstraint(
                    "CK_MaintenanceRequests_ClosedAtMatchesStatus",
                    "([Status] = N'Closed' AND [ClosedAt] IS NOT NULL) OR " +
                    "([Status] <> N'Closed' AND [ClosedAt] IS NULL)");
            });
            entity.HasKey(request => request.Id);
            entity.Property(request => request.Category).HasMaxLength(100).IsRequired();
            entity.Property(request => request.Priority)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();
            entity.Property(request => request.Status)
                .HasConversion<string>()
                .HasMaxLength(16)
                .IsRequired();
            entity.Property(request => request.Description).HasMaxLength(2000).IsRequired();
            entity.HasIndex(request => new
            {
                request.Status,
                request.Priority,
                request.CreatedAt
            });
            entity.HasIndex(request => request.TenantId);
            entity.HasOne(request => request.Unit)
                .WithMany(unit => unit.MaintenanceRequests)
                .HasForeignKey(request => request.UnitId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(request => request.Tenant)
                .WithMany(tenant => tenant.MaintenanceRequests)
                .HasForeignKey(request => request.TenantId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(request => request.Technician)
                .WithMany(technician => technician.AssignedRequests)
                .HasForeignKey(request => request.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.ToTable("Visits", table =>
                table.HasCheckConstraint(
                    "CK_Visits_CostNonNegative",
                    "[Cost] >= 0"));
            entity.HasKey(visit => visit.Id);
            entity.Property(visit => visit.Notes).HasMaxLength(2000).IsRequired();
            entity.Property(visit => visit.Cost).HasPrecision(18, 2);
            entity.HasIndex(visit => new { visit.MaintenanceRequestId, visit.VisitedAt });
            entity.HasIndex(visit => visit.TechnicianId);
            entity.HasOne(visit => visit.MaintenanceRequest)
                .WithMany(request => request.Visits)
                .HasForeignKey(visit => visit.MaintenanceRequestId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(visit => visit.Technician)
                .WithMany(technician => technician.Visits)
                .HasForeignKey(visit => visit.TechnicianId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
