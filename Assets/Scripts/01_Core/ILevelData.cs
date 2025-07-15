public interface ILevelData
{
    int StartLevel { get; }
    int StartExp { get; }
    int BaseExpToLevelUp { get; }
    float StatMultiplierPerLevel { get; }
}