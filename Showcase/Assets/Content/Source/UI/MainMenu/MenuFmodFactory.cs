using FMOD.Studio;
using Source.Data;
using UnityEngine;
using Helpers.Audio;
using Helpers.PoolSystem;

namespace Source.UI
{
public class MenuFmodFactory : MonoBehaviour, IMenuFmodFactory
{
    [SerializeField] FmodEventsSo _fmodEventsSo;
    
    Pool<PooledFmodEvent> _factoryFmodEvents;

    public void Init()
    {
        _factoryFmodEvents = new FactoryFmodEvents.Builder(_fmodEventsSo.ItemAppear)
                             .SetMaxCount(5)
                             .Build();
    }

    public void Deinit() => _factoryFmodEvents.Clear();

    public EventInstance GetItemAppearFmodEvent() => _factoryFmodEvents.Get().Instance;
}
}