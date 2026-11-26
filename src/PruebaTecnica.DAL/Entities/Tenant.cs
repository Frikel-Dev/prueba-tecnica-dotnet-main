namespace PruebaTecnica.DAL.Entities;

public sealed class Tenant
{
    public int Id { get; set; }

    public required string FullName { get; set; }

    public required string Email { get; set; }

    public required string Phone { get; set; }

    public ICollection<Lease> Leases { get; set; } = new List<Lease>();

    public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } =
        new List<MaintenanceRequest>();
}
