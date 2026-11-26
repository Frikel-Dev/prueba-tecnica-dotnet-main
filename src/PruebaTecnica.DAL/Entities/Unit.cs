using PruebaTecnica.BL.BusinessLogic;

namespace PruebaTecnica.DAL.Entities;

public sealed class Unit
{
    public int Id { get; set; }

    public int PropertyId { get; set; }

    public required string Number { get; set; }

    public int Floor { get; set; }

    public UnitTypeEnum Type { get; set; }

    public Property Property { get; set; } = null!;

    public ICollection<Lease> Leases { get; set; } = new List<Lease>();

    public ICollection<MaintenanceRequest> MaintenanceRequests { get; set; } =
        new List<MaintenanceRequest>();
}
