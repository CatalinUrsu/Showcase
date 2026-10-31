using R3;

namespace Source.Data
{
public interface IProgressionService
{
    Observable<Unit> Ascended { get; }

    bool CanAscend { get; }
    int AscendBonus { get; }
    int AscendMinLvl { get; }

    bool TryAscend();
}
}
