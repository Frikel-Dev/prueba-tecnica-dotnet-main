using PruebaTecnica.BL.BusinessLogic;

namespace PruebaTecnica.DAL.Entities;

public sealed class MaintenanceRequest
{
    public int Id { get; set; }

    public int UnitId { get; set; }

    public int TenantId { get; set; }

    public int? TechnicianId { get; set; }

    public required string Category { get; set; }

    public MaintenancePriorityEnum Priority { get; set; }

    public MaintenanceStatusEnum Status { get; set; }

    public required string Description { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? ClosedAt { get; set; }

    public Unit Unit { get; set; } = null!;

    public Tenant Tenant { get; set; } = null!;

    public Technician? Technician { get; set; }

    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
