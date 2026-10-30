# AGENTS Guide (Showcase)

## Existing AI Guidance
- No existing agent-rule files were found (`AGENTS.md`, `AGENT.md`, `CLAUDE.md`, `.cursorrules`, `.github/copilot-instructions.md`, etc.).

## Big Picture Architecture
- Unity 6 project (`ProjectSettings/ProjectVersion.txt` -> `6000.5.1f1`) with code split by feature assemblies under `Assets/Content/Source/{Boot,Core,Data,Game,Services,UI}`.
- Composition root is Zenject/Extenject: global bindings in `Assets/Content/Source/Boot/Installers/ProjectInstaller.cs`.
- Scene-scoped wiring is done by installers (`MenuInstaller`, `LoadingInstaller`, `GameplayInstaller`) that populate context objects (`MenuContext`, `LoadingContext`, `GameplayContext`).
- App bootstrap lives in `Assets/Content/Source/Boot/AppInit.cs`: register cameras, build state machine, load FMOD banks via Addressables, then enter `InitState`.

## Runtime Data/Control Flow
- Startup path: `AppInit.Awake()` -> `LoadFMODBanks()` -> `StatesMachine.Enter<InitState>()`.
- `InitState` initializes audio/settings and loads splash scene (`LOADING_SCENE`) before transitioning to `MenuState`.
- `StateBase` centralizes async scene load/unload + progress tracking (`IProgressTrackingService`) and FMOD scene instance cleanup.
- Typical scene transition pattern is in `MenuState`: subscribe UI events, `LoadScene(SceneLoadParams.Builder...)`, init context, then on exit show splash + deinit + unload.

## Project-Specific Coding Patterns
- Async is UniTask-first (`Cysharp.Threading.Tasks`), including transitions and setup (`await UniTask.WhenAll(...)`).
- DI bindings are explicit interface-first (`Container.Bind<IAudioService>().To<AudioService>().AsSingle()`).
- Persistent session models are created with `SaveSystem.LoadOrCreate(...)` using `ConstSavesPaths` (`settings.bin`, `progress.bin`, `items.bin`).
- Presenter pattern uses Zenject factories (e.g., `ShipPresenter.Factory : PlaceholderFactory<IShipView, Guid, ShipPresenter>` in `Assets/Content/Source/Data/Presenters/ShipPresenter.cs`).
- Reactive updates use `R3` subscriptions with disposables in presenters (`ShipPresenter`, `GameRunPresenter`).
- Input is guarded during transitions via `using (InputManager.Instance.LockInputSystem())` before state changes.

## Integrations and Boundaries
- FMOD is loaded from Addressable `AssetReference`s in `AppInit`; event/bank helpers come from `Packages/Helpers` (`BankLoader`, `FmodExtensions`, `FactoryFmodEvents`).
- Scene loading supports both build-index and Addressables through `ISceneLoaderService` + `SceneLoadParams` (builder pattern).
- Core constants (`ConstSceneNames`, `ConstFMOD`, `ConstCameras`, `ConstSavesPaths`) are centralized in `Assets/Content/Source/Core/Common/Constants.cs`.

## Developer Workflow Reality (This Repo)
- No CI workflows or shell build/test scripts are present in-repo.
- Dependency bootstrap is editor-driven:
  - Auto on editor load via `[InitializeOnLoad]` in `Packages/Helpers/Editor/Bootstrap/PackagesResolver.cs`.
  - Manual fallback menu actions: `Tools/Helpers/Wizard/Resolve All Packages` and `Tools/Helpers/Wizard/Resolve Nuget`.
- When adding code, prefer updating Unity source + installers/contexts rather than editing generated `.csproj` files directly.

## Practical Guidance for AI Agents
- If adding a new service, bind it in `ProjectInstaller` and inject by interface into states/facades.
- If adding scene-specific runtime objects, register them through the corresponding scene installer + context class, and clear references on destroy.
- If adding a new game/app state, follow `IStateEnter`/`StateBase` conventions and route transitions through `StatesMachine`.
- If adding UI logic, keep behavior in presenters/facades and keep views passive (observe existing `Source.Data` presenter patterns).

