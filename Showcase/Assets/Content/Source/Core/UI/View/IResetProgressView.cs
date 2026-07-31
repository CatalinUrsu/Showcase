namespace Source
{
public interface IResetProgressView
{
    void OnChangeLvl_handler(bool reachedMinBonusLvl, int progressResetBonus);
}
}