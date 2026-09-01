# Showcase

Unity 6 pet project focused on architecture quality: state-driven scene lifecycle, dependency injection, reactive UI, async orchestration, and audio/content pipelines.

- Unity version: `6000.5.1f1` (from `Showcase/ProjectSettings/ProjectVersion.txt`)
- Primary goal: demonstrate production-style structure on a small game scope
- LinkedIn: [Catalin Ursu](https://www.linkedin.com/in/catalin-ursu-b19429167/)

# Game Capture
https://github.com/user-attachments/assets/3a0edbf8-be96-42ff-b5b5-1bd159e958eb


## Contents
- [Project Highlights](#project-highlights)
- [Tech Stack](#tech-stack)
- [How It Works](#how-it-works)
- [Project Structure](#project-structure)
- [Quick Start](#quick-start)
- [Core Architecture Patterns](#core-architecture-patterns)
- [Integrations](#integrations)
- [Performance and Optimization](#performance-and-optimization)
- [Challenges Solved](#challenges-solved)
- [Architecture Decisions](#architecture-decisions)
<br/><br/>

## <b><u>Project Highlights</u></b>
- Single-entry bootstrap through `Showcase/Assets/Content/Source/Boot/AppInit.cs`
- <b><i>Async</i></b> state machine transitions (`InitState -> MenuState <-> GameState`)
- Explicit <b><i>DI composition root with Extenject</i></b> in `Showcase/Assets/Content/Source/Boot/Installers/ProjectInstaller.cs`
- Scene-scoped runtime wiring via installers + context interfaces (`IMenuContext`, `ILoadingContext`, `IGameplayContext`)
- Presenter-based UI flow with reactive state updates <b><i>(R3)</i></b>
- <b><i>FMOD banks</i></b> loading and scene-based audio behavior integrated with <b><i>Addressables</i></b>
<br/><br/>


## <b><u>Tech Stack</u></b>
- Engine: Unity 6
- Language: C#
- Dependency Injection: Extenject (Zenject)
- Async: Cysharp UniTask
- Reactive: Cysharp R3
- Audio: FMOD Studio
- Asset management: Unity Addressables
- Localization: Unity Localization package
- Reusable Submodules
  - Helpers: https://github.com/CatalinUrsu/Tool_Helpers
  - IdleNumber: https://github.com/CatalinUrsu/Tool_IdleNumber
<br/><br/>

## <b><u>How It Works</u></b>
### <b><u>Runtime flow</u></b>
1. `AppInit.Awake()` registers cameras, configures systems, initializes state machine, and loads FMOD master banks.
2. State machine enters `InitState`.
3. `InitState` initializes audio volumes from saved settings, loads loading scene, and shows splash.
4. Transition to `MenuState`.
5. `MenuState` loads menu scene content, hooks menu events, and waits for user actions.
6. Start game transitions to `GameState`, which loads gameplay scene, initializes run state, configures camera, and activates gameplay UI.
7. Returning to menu triggers cleanup, save, unload, and splash-driven transition.

### <b><u>State transitions (simplified)</u></b>
```text
AppInit -> InitState -> MenuState <-> GameState
```
Input is locked during transitions (`InputManager.Instance.LockInputSystem()`) to avoid race conditions and accidental double actions.
<br/><br/>

## <b><u>Project Structure</u></b>
Main source root: `Showcase/Assets/Content/Source`

```text
Boot/
  AppInit.cs
  Installers/        (ProjectInstaller, MenuInstaller, GameplayInstaller, LoadingInstaller)
  SceneContexts/     (MenuContext, GameplayContext, LoadingContext)
  StateMachine/      (InitState, MenuState, GameState)

Core/
  Common/            (constants, DDOL)
  Data/              (interfaces)
  Enums/
  SceneContext/      (context interfaces)
  UI/                (view interfaces)

Data/
  Controllers/
  Models/
  Presenters/
  SO/

Services/
  Audio/
  Cinemachine/
  Session/

UI/
  MainMenu/
  Gameplay/
  Facades/
  Views/
```
<br/><br/>

## <b><u>Quick Start</u></b>
1. Open Unity Hub and add project folder: `Showcase/Showcase`.
2. Open with Unity Editor `6000.5.1f1` (or closest compatible Unity 6 version).
3. Let editor auto-resolve packages on load (`[InitializeOnLoad]` in `PackagesResolver`).
4. If packages are missing, use:
   - `Tools -> Helpers -> Wizard -> Resolve All Packages`
   - `Tools -> Helpers -> Wizard -> Resolve Nuget`
5. Open scene: `Showcase/Assets/Content/Scenes/Init.unity`.
6. Press Play.
<br/><br/>

## <b><u>Core Architecture Patterns</u></b>
### <b><u>1) Composition Root with DI</u></b>
Global bindings are defined in `Showcase/Assets/Content/Source/Boot/Installers/ProjectInstaller.cs`.
Use this pattern when introducing a new global service or persistent domain controller that must be shared across states/scenes.

- Binds model controllers and services as singletons by interface.
- Creates session models via `SaveSystem.LoadOrCreate(...)`.
- Registers presenter factories (`ShipPresenter`, `WeaponPresenter`, `ResetProgressPresenter`, `GameRunPresenter`).

```csharp
// Service binding: use this for audio, scene loading, camera service, etc..
Container.Bind<IAudioService>().To<AudioService>().AsSingle();

// Model-controller binding: create controller after load/create save model,
// then bind by interface so presenters/states depend on abstraction only.
var progressModel = SaveSystem.LoadOrCreate<ProgressModel>(ConstSavesPaths.PROGRESS_PATH);
var progressModelController = new ProgressModelController(progressModel);
Container.Bind<IProgressModelController>().FromInstance(progressModelController).AsSingle();
```

### <b><u>2) Scene lifecycle via states</b></u>
States live in `Showcase/Assets/Content/Source/Boot/StateMachine` and implement `IStateEnter`.

- `InitState`: bootstrap services + loading scene setup.
- `MenuState`: menu scene load/init, user entry flow.
- `GameState`: gameplay scene load/init, runtime listeners, pause/resume, save on exit.

### <b><u>3) Async-first orchestration</u></b>
UniTask is used across scene loads, transitions, and content activation.

- Non-blocking transitions
- Coordinated parallel work with `UniTask.WhenAll(...)`
- Predictable flow sequencing during scene enter/exit

### <b><u>4) Reactive presenter pattern (R3)</u></b>
R3 is used for observable state and UI updates.

- Models expose reactive values
- Presenters subscribe and push updates into passive views
- Disposables are managed to avoid leaks in long sessions

```csharp
// Presenter subscribes to model changes once and stores subscriptions.
readonly CompositeDisposable _disposable = new();

_gameRunModelController.IModel.ProgressRef
    .Subscribe(progress => _viewGameplay.SetProgressSlider(progress))
    .AddTo(_disposable);

_gameRunModelController.IModel.CollectedCoinsRef
    .Subscribe(coins => _viewGameplay.SetCollectedCoins(coins))
    .AddTo(_disposable);

// Called on scene/presenter deinit to prevent retained subscriptions.
public void Dispose() => _disposable.Clear();
```

Implementation note: subscribe in presenter constructor/init, dispose in presenter `Dispose()` or scene deinit.

### <b><u>5) Persistent session mode</u></b>
Project uses JSON save files for core progression and settings.

- Paths centralized in `Showcase/Assets/Content/Source/Core/Common/Constants.cs` (`settings.json`, `progress.json`, `items.json`)
- Save/load operations coordinated through `SessionService`
<br/><br/>

## <b><u>Integrations</u></b>
### <b><u>FMOD</u></b>
- Master banks are initialized in `Showcase/Assets/Content/Source/Boot/AppInit.cs`.
- Audio runtime service is `Showcase/Assets/Content/Source/Services/Audio/AudioService.cs`.
- Uses VCAs and snapshots for runtime volume states and pause behavior.

```csharp
// Music state switch (example from MenuState).
_audioService.MusicInstance.SetParameter(ConstFMOD.MUSIC_STATE, EMusicStates.Idle.ToString());

// VCA setup/use (example from AudioService).
_soundVCA = RuntimeManager.GetVCA(ConstFMOD.VCA_Sound);
_musicVCA = RuntimeManager.GetVCA(ConstFMOD.VCA_Music);
_soundVCA.setVolume(soundVolume);
_musicVCA.setVolume(musicVolume);

// Pause snapshot (example from GameState).
_audioService.PauseSnapshot.start();
_audioService.PauseSnapshot.stop(STOP_MODE.ALLOWFADEOUT);
```

```csharp
// FMOD bank loading at bootstrap (AppInit).
async UniTask LoadFMODBanks()
{
    await _bankLoaderMasterStrings.Init();
    await _bankLoaderMaster.Init();
}
```

```csharp
// FMOD pooled event factory usage (MenuFmodFactory / PlayerWeapons style).
_itemAppearPool = new FactoryFmodEvents.Builder(_fmodEventsSo.ItemAppear)
                  .SetPreloadCount(3)
                  .SetMaxCount(5)
                  .Build();

_itemAppearPool.Get().Instance.start();
```

### <b><u>Addressables</u></b>
- Scene loading uses `SceneLoadParams` with `.SetIsAddressable(true)` in states.
- Runtime asset loading is used for gameplay content (for example enemy prefab key usage).

```csharp
// Scene load through builder + lifecycle service (MenuState / GameState pattern).
var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.MENU_SCENE)
    .SetTip("Load Menu Scene")
    .SetIsAddressable(true)
    .SetActiveOnLoad(true)
    .Build();

await _sceneLifecycleService.LoadAndInitScene(sceneLoadParams, _menuContext);
```

### <b><u>Localization</u></b>
- Unity Localization package is used for locale-based UI text.
- Locale switch logic is wired in settings panel (`TabPanelSettings` using `LocalizationSettings.SelectedLocale`).

### <b><u>Cinemachine</u></b>
- Gameplay camera state is switched by `ICinemachineService` from state layer.
- Camera references are registered during bootstrap in `AppInit`.
<br/><br/>

## <b><u>Performance and Optimization</u></b>
This project is intentionally small, but performance practices are implemented as if it were production content.

- Transition safety: input is locked during state switches to prevent re-entrancy and duplicated actions.
- Scene lifecycle discipline: centralized load/deinit/unload flow improves memory predictability during transitions.
- Asset strategy: Addressables reduce static memory pressure, while FMOD banks are explicitly loaded/unloaded by runtime context.
- UI batching: Sprite Atlases and 9-slicing reduce draw calls and improve UI reuse.
- UI cost trimming: unnecessary mask and raycast checks are removed from non-interactive elements.
- Texture pipeline: POT textures are preferred, compression is pushed with quality checks, and compression is applied at atlas level for Sprite Atlas V2 workflow.

### <b><u>Profiling workflow</u></b>
- Unity Profiler: used to monitor CPU, memory, UI, and Addressables behavior; final-build profiling is preferred over Editor-only readings.
- Memory Profiler: used to validate memory stability during scene transitions and detect asset duplication.
- Frame Debugger: used to verify UI batch optimization and rendering-side improvements.
<br><img src="https://i.postimg.cc/cHS7jXym/Sprite-Atlas.png" alt="Sprite Atlas" width="300">
<br><img src="https://i.postimg.cc/2yr08MF3/Profiler.png" alt="Profiler" width="600">
<br><img src="https://i.postimg.cc/264HYrWP/Memory-Profiler.png" alt="MemoryProfiler" width="600">
<br><img src="https://i.postimg.cc/qRfmWzNs/Frame-Debug.png" alt="FrameDebugger" width="600">
<br/><br/>

## <b><u>Challenges Solved</u></b>
- Built a small-scope game with enterprise-like architecture while preserving readability.
- Combined multiple frameworks (Extenject, UniTask, R3, FMOD, Addressables) into one coherent runtime flow.
- Designed state-driven scene lifecycle to keep transitions stable and testable.
- Kept views passive and moved behavior into presenters/controllers for maintainability.
  <br/><br/>

## <b><u>Architecture Decisions</u></b>
- **State machine for navigation** instead of direct scene chaining for explicit control over enter/exit and cleanup.
- **Interface-first DI bindings** to reduce coupling and make services replaceable.
- **Reactive model-to-view updates** to keep UI consistent with gameplay/session state.
- **Async scene orchestration** to avoid blocking operations and simplify transition sequencing.
- **Context objects per scene** to isolate scene-level references from global scope.
