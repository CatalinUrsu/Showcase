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
    [SerializeField] ItemInfoWeaponSO[] _weaponsSO;
    [SerializeField] ItemInfoShipSO[] _ShipsSO;

    public override void InstallBindings()
    {
        BindSessionModelsControllers();
        BindServices();
        BindSceneContexts();
    }

    void BindSessionModelsControllers()
    {
        // Store init Data into Dict
        var initWeaponsData = _weaponsSO.ToDictionary(weaponSo => weaponSo.IdSo.GUID, weaponSo => weaponSo.InitData);
        var initShipsData = _ShipsSO.ToDictionary(shipSo => shipSo.IdSo.GUID, shipSo => shipSo.InitData);

        // Load or Create save data for models
        var itemsModel = SaveSystem.LoadOrCreate<ItemsModel>(ConstSavesPaths.ITEMS_PATH);
        var progressModel = SaveSystem.LoadOrCreate<ProgressModel>(ConstSavesPaths.PROGRESS_PATH);
        var settingsModel = SaveSystem.LoadOrCreate<SettingsModel>(ConstSavesPaths.SETTINGS_PATH);

        // Create ModelControllers with needed models
        var itemsModelController = new ItemsModelController(itemsModel, initWeaponsData, initShipsData);
        var progressModelController = new ProgressModelController(progressModel);
        var settingsModelController = new SettingsModelController(settingsModel);

        // Create SessionService with needed ModelControllers
        var sessionService = new SessionService(itemsModelController, progressModelController, settingsModelController);

        // Bind Models 
        Container.Bind<IItemsModelController>().FromInstance(itemsModelController).AsSingle();
        Container.Bind<IProgressModelController>().FromInstance(progressModelController).AsSingle();
        Container.Bind<ISettingsModelController>().FromInstance(settingsModelController).AsSingle();

        // Bind SessionService
        Container.Bind<ISessionService>().FromInstance(sessionService).AsSingle();
    }

    void BindServices()
    {
        IProgressTrackingService progressTrackingService = new ProgressTrackingService();
        ISceneLoaderService sceneLoaderService = new SceneLoaderService(progressTrackingService);
        
        Container.Bind<IAudioService>().FromInstance(new AudioService()).AsSingle();
        Container.Bind<ICameraService>().FromInstance(new CameraService()).AsSingle();
        Container.Bind<IProgressTrackingService>().FromInstance(progressTrackingService).AsSingle();
        Container.Bind<ISceneLoaderService>().FromInstance(sceneLoaderService).AsSingle();
    }

    void BindSceneContexts()
    {
        Container.Bind<IMenuContext>().FromInstance(new MenuContext()).AsSingle();
        Container.Bind<ILoadingContext>().FromInstance(new LoadingContext()).AsSingle();
        Container.Bind<IGameplayContext>().FromInstance(new GameplayContext()).AsSingle();
    }
}
}