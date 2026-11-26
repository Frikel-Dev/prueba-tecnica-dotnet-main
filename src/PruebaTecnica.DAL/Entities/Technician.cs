using PruebaTecnica.BL.BusinessLogic;

namespace PruebaTecnica.DAL.Entities;

public sealed class Technician
{
    public int Id { get; set; }

    public required string FullName { get; set; }

    public TechnicianSpecialtyEnum Specialty { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<MaintenanceRequest> AssignedRequests { get; set; } =
        new List<MaintenanceRequest>();

    public ICollection<Visit> Visits { get; set; } = new List<Visit>();
}
