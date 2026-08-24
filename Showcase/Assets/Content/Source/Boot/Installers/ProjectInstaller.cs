using System;
using Helpers;
using Zenject;
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

    [SerializeField] ItemInfoWeaponSO[] _weaponsSO;
    [SerializeField] ItemInfoShipSO[] _ShipsSO;
    [SerializeField] FmodEventsSo _fmodEventsSO;

#endregion

#region Bindings methods

    public override void InstallBindings()
    {
        BindSessionModelsControllers();
        BindSoData();
        BindServices();
        BindSceneContexts();
        BindFactories();
    }

    void BindSessionModelsControllers()
    {
        // Load or Create save data for models
        var itemsModelController = GetItemsModelController();
        var progressModelController = GetProgressModelController();
        var settingsModelController = GetSettingsModelController();
        var gameRunModelController = new GameRunModelController(itemsModelController, progressModelController);
        var sessionService = new SessionService(itemsModelController, progressModelController, settingsModelController);

        // Bind Models 
        Container.Bind<IItemsModelController>().FromInstance(itemsModelController).AsSingle();
        Container.Bind<IProgressModelController>().FromInstance(progressModelController).AsSingle();
        Container.Bind<ISettingsModelController>().FromInstance(settingsModelController).AsSingle();
        Container.Bind<IGameRunModelController>().FromInstance(gameRunModelController).AsSingle();
        Container.Bind<ISessionService>().FromInstance(sessionService).AsSingle();
    }

    void BindSoData()
    {
        Container.Bind<FmodEventsSo>().FromInstance(_fmodEventsSO).AsSingle();
    }

    void BindServices()
    {
        IProgressTrackingService progressTrackingService = new ProgressTrackingService();
        ISceneLoaderService sceneLoaderService = new SceneLoaderService(progressTrackingService);
        
        Container.Bind<IAudioService>().To<AudioService>().AsSingle();
        Container.Bind<ICameraService>().FromInstance(new CameraService()).AsSingle();
        Container.Bind<IProgressTrackingService>().FromInstance(progressTrackingService).AsSingle();
        Container.Bind<ISceneLoaderService>().FromInstance(sceneLoaderService).AsSingle();
        Container.Bind<ISceneLifecycleService>().To<SceneLifecycleService>().AsSingle();
    }

    void BindSceneContexts()
    {
        Container.Bind<IMenuContext>().FromInstance(new MenuContext()).AsSingle();
        Container.Bind<ILoadingContext>().FromInstance(new LoadingContext()).AsSingle();
        Container.Bind<IGameplayContext>().FromInstance(new GameplayContext()).AsSingle();
    }

    void BindFactories()
    {
        Container.BindFactory<IShipView, Guid, ShipPresenter, ShipPresenter.Factory>().AsTransient();
        Container.BindFactory<IWeaponView, Guid, WeaponPresenter, WeaponPresenter.Factory>().AsTransient();
        Container.BindFactory<IResetProgressView, ResetProgressPresenter, ResetProgressPresenter.Factory>().AsTransient();
        Container.BindFactory<IGameplayView, GameRunPresenter, GameRunPresenter.Factory>().AsTransient();
    }

#endregion

#region private methods

    ItemsModelController GetItemsModelController()
    {
        var initWeaponsData = _weaponsSO.ToDictionary(weaponSo => weaponSo.IdSo.Guid, weaponSo => weaponSo.InitData);
        var initShipsData = _ShipsSO.ToDictionary(shipSo => shipSo.IdSo.Guid, shipSo => shipSo.InitData);
        var itemsModel = SaveSystem.LoadOrCreate<ItemsModel>(ConstSavesPaths.ITEMS_PATH);
        return new ItemsModelController(itemsModel, initWeaponsData, initShipsData);
    }

    ProgressModelController GetProgressModelController()
    {
        var progressModel = SaveSystem.LoadOrCreate<ProgressModel>(ConstSavesPaths.PROGRESS_PATH);
        return new ProgressModelController(progressModel);
    }

    SettingsModelController GetSettingsModelController()
    {
        var settingsModel = SaveSystem.LoadOrCreate<SettingsModel>(ConstSavesPaths.SETTINGS_PATH);
        return new SettingsModelController(settingsModel);
    }

#endregion
}
}