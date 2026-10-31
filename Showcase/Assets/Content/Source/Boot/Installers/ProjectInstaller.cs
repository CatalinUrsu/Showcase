using System;
using Helpers;
using Zenject;
using Source.UI;
using UnityEngine;
using System.Linq;
using Source.Data;
using Source.Services;
using Helpers.Services;

namespace Source.Boot
{
public class ProjectInstaller : MonoInstaller
{
#region Fields

    [SerializeField] FmodEventsSo _fmodEventsSO;
    [SerializeField] GameBalanceSO _gameBalance;
    [SerializeField] InputManager _inputManagerPrefab;
    
    [Space]
    [SerializeField] ItemInfoWeaponSO[] _weaponsSO;
    [SerializeField] ItemInfoShipSO[] _ShipsSO;

    [Space]
    [SerializeField] VolumeByState[] _volumes;

#endregion

#region Bindings methods

    public override void InstallBindings()
    {
        BindModels();
        BindSoData();
        BindServices();
        BindEntryPoints();
        BindFactories();
    }

    void BindModels()
    {
        // Load or Create save data for models
        Container.BindInstance(GetItemsModel()).AsSingle();
        Container.BindInstance(new ProgressModel(SaveSystem.LoadOrCreate<ProgressSaveData>(ConstSavesPaths.PROGRESS_PATH))).AsSingle();
        Container.BindInstance(new SettingsModel(SaveSystem.LoadOrCreate<SettingsSaveData>(ConstSavesPaths.SETTINGS_PATH))).AsSingle();
        Container.Bind<GameRunModel>().AsSingle();

        Container.BindInterfacesTo<GameRunService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<ShopService>().AsSingle();
        Container.BindInterfacesTo<ProgressionService>().AsSingle();
        Container.BindInterfacesTo<ShopFeedback>().AsSingle().NonLazy();
        Container.Bind<ISessionService>().To<SessionService>().AsSingle();
    }

    void BindSoData()
    {
        Container.Bind<FmodEventsSo>().FromInstance(_fmodEventsSO).AsSingle();
        Container.BindInstance(_gameBalance).AsSingle();
    }

    void BindServices()
    {
        IProgressTrackingService progressTrackingService = new ProgressTrackingService();
        ISceneLoaderService sceneLoaderService = new SceneLoaderService(progressTrackingService);
        IVolumeSwitcherService volumeSwitcherService = new VolumeSwitcherService(_volumes);
        
        Container.Bind<ICinemachineService>().To<CinemachineService>().AsSingle();
        Container.Bind<IVolumeSwitcherService>().FromInstance(volumeSwitcherService).AsSingle();
        Container.Bind<IAudioService>().To<AudioService>().AsSingle();
        Container.Bind<ICameraService>().FromInstance(new CameraService()).AsSingle();
        Container.Bind<IProgressTrackingService>().FromInstance(progressTrackingService).AsSingle();
        Container.Bind<ISceneLoaderService>().FromInstance(sceneLoaderService).AsSingle();
        Container.Bind<ISceneLifecycleService>().To<SceneLifecycleService>().AsSingle();
        Container.Bind<IPauseService>().To<PauseService>().AsSingle();
        Container.Bind<ILocaleService>().To<LocaleService>().AsSingle();
        Container.BindInterfacesTo<GameCommands>().AsSingle();
        Container.Bind<IInputManager>().FromComponentInNewPrefab(_inputManagerPrefab).AsSingle().NonLazy();
    }

    void BindEntryPoints()
    {
        Container.Bind<EntryPointRegistry>().AsSingle();

        // Loading scene lives whole game, so its entry point is global
        Container.Bind<ILoadingEntryPoint>().FromInstance(new LoadingEntryPoint()).AsSingle();
    }

    void BindFactories()
    {
        Container.BindFactory<IShipView, Guid, ShipPresenter, ShipPresenter.Factory>().AsTransient();
        Container.BindFactory<IWeaponView, Guid, WeaponPresenter, WeaponPresenter.Factory>().AsTransient();
        Container.BindFactory<IResetProgressView, ResetProgressPresenter, ResetProgressPresenter.Factory>().AsTransient();
        Container.BindFactory<IGameplayView, GameRunPresenter, GameRunPresenter.Factory>().AsTransient();
        Container.BindFactory<ISettingsView, SettingsPresenter, SettingsPresenter.Factory>().AsTransient();
    }

#endregion

#region private methods

    ItemsModel GetItemsModel()
    {
        var initWeaponsData = _weaponsSO.ToDictionary(weaponSo => weaponSo.IdSo.Guid, weaponSo => weaponSo.InitData);
        var initShipsData = _ShipsSO.ToDictionary(shipSo => shipSo.IdSo.Guid, shipSo => shipSo.InitData);
        var itemsSaveData = SaveSystem.LoadOrCreate<ItemsSaveData>(ConstSavesPaths.ITEMS_PATH);
        return new ItemsModel(itemsSaveData, initWeaponsData, initShipsData);
    }

#endregion
}
}