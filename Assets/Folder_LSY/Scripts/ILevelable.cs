public interface ILevelable
{
    int Level { get; }
    int CurrentExp { get; }
    int ExpToNextLevel { get; }
    void AddExp(int amount);
    void LevelUp();
}
