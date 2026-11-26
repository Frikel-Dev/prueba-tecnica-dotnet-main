namespace PruebaTecnica.DAL.Entities;

public sealed class Visit
{
    public int Id { get; set; }

    public int MaintenanceRequestId { get; set; }

    public int TechnicianId { get; set; }

    public DateTimeOffset VisitedAt { get; set; }

    public required string Notes { get; set; }

    public decimal Cost { get; set; }

    public MaintenanceRequest MaintenanceRequest { get; set; } = null!;

    public Technician Technician { get; set; } = null!;
}
