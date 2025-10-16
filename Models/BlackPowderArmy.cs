namespace UmbracoProject1.Models;

public class BlackPowderArmy
{
    public string Name { get; set; } = string.Empty;
    public string Faction { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public int TargetPoints { get; set; }
    public List<ArmyUnit> Units { get; set; } = new();

    public int TotalPoints => Units.Sum(u => u.PointsCost * u.Quantity);
    public int TotalUnitCount => Units.Sum(u => u.Quantity);
}

public class ArmyUnit
{
    public string UnitId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; } = 1;
    public int PointsCost { get; set; }
    public string Notes { get; set; } = string.Empty;
}
