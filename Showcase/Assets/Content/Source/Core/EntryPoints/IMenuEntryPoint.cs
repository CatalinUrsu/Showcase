using Helpers.Services;

namespace Source
{
public interface IMenuEntryPoint: IEntryPoint
{
    IPlayerFacade PlayerFacade { get; }
    IMenuUIFacade UIFacade { get; }
}
}
