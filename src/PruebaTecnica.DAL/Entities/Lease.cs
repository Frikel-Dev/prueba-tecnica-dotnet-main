namespace PruebaTecnica.DAL.Entities;

public sealed class Lease
{
    public int Id { get; set; }

    public int UnitId { get; set; }

    public int TenantId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public decimal MonthlyRent { get; set; }

    public Unit Unit { get; set; } = null!;

    public Tenant Tenant { get; set; } = null!;
}
