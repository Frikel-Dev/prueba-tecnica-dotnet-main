namespace PruebaTecnica.DAL.Entities;

public sealed class Property
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Address { get; set; }

    public required string City { get; set; }

    public ICollection<Unit> Units { get; set; } = new List<Unit>();
}
