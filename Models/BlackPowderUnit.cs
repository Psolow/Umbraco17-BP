namespace UmbracoProject1.Models;

public class BlackPowderUnit
{
    public string Name { get; set; } = string.Empty;
    public UnitType Type { get; set; }
    public UnitSize Size { get; set; }
    public int PointsCost { get; set; }
    public int Clash { get; set; }
    public int SustainedFire { get; set; }
    public int ShortRangeFire { get; set; }
    public int LongRangeFire { get; set; }
    public int MoraleSave { get; set; }
    public int Stamina { get; set; }
    public string SpecialRules { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public enum UnitType
{
    Infantry,
    Cavalry,
    Artillery,
    LightCavalry,
    HeavyCavalry,
    Dragoons,
    LightInfantry,
    HeavyInfantry
}

public enum UnitSize
{
    Tiny,
    Small,
    Standard,
    Large,
    VeryLarge
}
