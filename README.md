  <h1 align="center">
Showcase
</h1>
<h3>Showcase project, created purely to demonstrate some of my skills related to architecture and unity stuffs, so making it visually beautiful was not the goal :)
<br>- Unity version - Unity 6.1.8f1
<br>- FMOD Engine for audio control
<br>- To start -> open Init Scene
</h3>

<div align="center">
<img src="https://i.postimg.cc/pLkfvypN/Showcase-Rec-ezgif-com-video-to-gif-converter.gif" alt="Game Capture" width="250">
</div>


# Contents:
- [1. Submodules 🧰](#1-submodules-)
- [2. Architecture ⚙️](#2-architecture-)
  - [2.1 State Machine ↔️](#21-state-machine-)
    - [2.1.1 InitState 📍](#211-initstate-)
    - [2.1.2 MenuState & GameplayState 🎮](#212-menustate-and-gameplaystate-)
  - [2.2 Zenject 💉](#22-zenject-)
  - [2.3 UniTask 🚦](#2-3-unitask-)
  - [2.4 UniRx 🚀](#2-4-unirx-)
- [3. Localization 🌐](#3-localization-)
- [4. Addressables 📦](#4-addressables-)
- [5. Optimization 🔧](#5-optimization-)
  - [5.1 UI 📺](#5-1-ui-)
  - [5.2 Textures 🖼️](#5-2-textures-)
  - [5.3 Audio 🎚️](#5-3-audio-)
  - [5.4 Profiling 🎛️](#5-4-profiling-)
- [6. Extra 🗃️](#6-extra-)
- [7. Notes 📜](#7-notes-)


# 1. Submodules 🧰
Because some of the scripts, prefabs and other assets can be used in other projects, I decided to create several submodules and use them as packages.
- <i><b><a href="https://github.com/CatalinUrsu/Tool_Helpers">Helpers</a></b></i>
- <i><b><a href="https://github.com/CatalinUrsu/Tool_IdleNumber">IdleNumber</a></b></i>


# 2. Architecture ⚙️
The project architecture is based on several pillars <i>(StateMachine, DI, MVP, Async Implementation)</i>
Naturally, a project of such a small scale can do without them and this may seem excessive,but this is still a demonstration project.
<br><br>App starts from [AppInit](Showcase/Assets/Content/Source/Global/AppInit.cs), this is a single entry point,
which allows you to control the order of execution of subsequent logic.
```csharp
    async void Awake()
    {
#if !UNITY_EDITOR
        Application.targetFrameRate = 60;
#endif

        _initValuesForSaves.LoadSavedItems();
        _serviceCamera.RegisterMainCamera(_cameraMain);
        _serviceCamera.RegisterCamera(ConstCameras.CAMERA_UI, _cameraUI);
        SetDebugViews();

        await UniTask.WhenAll(LoadFMODBanks(),
                              SetSingletons());

        SetStateMachine();
    }
```


## 2.1 State Machine ↔️
App has several states driven by StateMachine.
For more control during state changing, the process is async
```csharp
    public async UniTask Enter<TState>();
    public async UniTask Enter<TState, TPayload>(TPayload payload)
```

<br>There are two types of states - <b>[Simple](Showcase/Packages/Helpers/StateMachine/Interfaces/IStateEnter.cs)</b> and
<b>[Payload](Showcase/Packages/Helpers/StateMachine/Interfaces/IStateEnterPayload.cs)</b> with two main methods - <b>Enter()</b> and <b>Exit()</b>
```csharp
    public interface IStateEnter
    {
        UniTask Enter();
        UniTask Exit();
    }

    public interface IStateEnterPayload<TPayload>
    {
        UniTask Enter(TPayload payload);
        UniTask Exit();
    }
```

### 2.1.1 InitState 📍
When StateMachine is created, it firstly will enter [InitState](Showcase/Assets/Content/Source/StateMachine/States/StateInit.cs). 
```csharp
    public async UniTaskVoid Enter()
    {
        _audioService.Init();
        await LoadAndShowSplashScreen();
        await StatesMachine.Enter<StateMenu>();
    }
```


### 2.1.2 MenuState And GameplayState 🎮
Since [MenuState](Showcase/Assets/Content/Source/StateMachine/States/StateMenu.cs). and
[GameplayState](Showcase/Assets/Content/Source/StateMachine/States/StateGameplay.cs). have similar logic <i>(Load scene and content, unload scene, set music state)</i> so I decided to move the similar
functionality to the base class - [BaseState](Showcase/Assets/Content/Source/StateMachine/States/StateBase.cs).

<br><h4>OnEnterState</h4>
- Set music state.
- Show Loading screen.
- Load scene using [SceneLoaderService](Showcase/Packages/Helpers/Services/SceneLoaderService.cs) and [SceneLoadParams](Showcase/Packages/Helpers/Services/SceneLoadInfo/SceneLoadParams.cs).
- Load content from the scene using [EntryPoint](Showcase/Packages/Helpers/StateMachine/Interfaces/IEntryPoint.cs).
```csharp 
    public override async UniTaskVoid Enter()
    {
        using (InputManager.Instance.LockInputSystem())
        {
            var sceneLoadParams = new SceneLoadParams.Builder(ConstSceneNames.MENU_SCENE)
                                  .SetPrompt("SceneMenu")
                                  .SetActiveOnLoad(true)
                                  .Build();

            SetMusicState(EMusicStates.Idle);

            await LoadingContent(sceneLoadParams, "ContentMenu");
            await ShowingContent();
        }
    }
```

<br><h4>OnExitState</h4>
- Show Loading screen.
- Unload all audio instances of current scene to release the memory.
- Unload content from the scene and unload the scene.
- Register unloading tasks to LoadingProgressService to track them.
```csharp
    protected async UniTask UnloadingContent(string sceneName)
    {
        await _serviceSplashScreen.ShowPage();

        FmodExtensions.ReleaseInstanceByScene(sceneName);
        var unloadingTasks = _entryPoint.Exit().ContinueWith(() => _sceneLoaderService.UnloadScene(sceneName));
        _loadingProgressService.RegisterUnloadingTasks(unloadingTasks).Forget();
    }
```


## Zenject 💉
Zenject is used as a dependency injection framework in the app to manage the dependencies between various components and services efficiently.
Here are the key benefits and purposes of using Zenject:
- Helps in breaking dependencies between concrete classes. By using interfaces or abstract classes, different implementations can be
  swapped easily without modifying the dependent code.
- It allows for the easy instantiation and management of services, making it straightforward to inject shared functionality (like audio or data services) into classes that need them.
- Provides mechanisms for managing the lifecycle of objects, such as singletons or transient instances, ensuring proper instantiation and cleanup.


## UniTask 🚦
- UniTask provides a way to perform asynchronous operations in a more efficient manner, allowing for tasks such as loading assets, waiting for conditions,
  or performing time-based behaviors without blocking the main thread.
- It is optimized for performance compared to traditional coroutines. It avoids some of the overhead
  associated with Unity's coroutine system, making it suitable for performance-critical applications.
- UniTask provides built-in support for task cancellation, allowing for more control over long-running operations, which can be particularly useful in cases where
  an operation needs to be aborted based on game events, for example - <b>when a player exits a scene or cancels an action.</b>
```csharp
    async UniTaskVoid EnableShield()
    {
        if (!_collider.enabled)
        {
            CancelShieldCTS();
            _shieldCTS = new CancellationTokenSource();
        }

        try
        {
            _collider.enabled = false;
            await _playerAppearence.AnimateShield(_shieldCTS.Token);
            _collider.enabled = true;
        }
        catch (OperationCanceledException)
        {
            Debug.Log("Shield animation was canceled");
        }
    }

    void CancelShieldCTS()
    {
        if (_shieldCTS == null) return;
        _shieldCTS.Cancel();
        _shieldCTS.Dispose();
    }
```


## UniRx 🚀
- UniRx provides a set of powerful LINQ-style query operators, which makes it easy to perform complex operations on collections of data or
  event streams, such as filtering, mapping, and aggregating
```csharp
    playerInputHanler.AddComponent<ObservableDragTrigger>()
                     .OnDragAsObservable()
                     .Where(_ => ControllIsEnable && _isClicked)
                     .Select(pointer => pointer.position)
                     .Subscribe(OnDrag_handler);
```

- It offers methods to optimize performance using its own update methods, which can be more efficient than Unity's standard Update() method. This helps
  avoid performance issues related to frequent updates.
```csharp
    Observable.EveryFixedUpdate()
              .Where(_ => _isClicked)
              .Subscribe(_ => MovePlayer(rb))
              .AddTo(gameObject);
```

- It includes a ReactiveProperty class that allows for easy binding of data properties to UI elements. This supports the Model-View-Presenter (MVP) pattern
  by efficiently managing data changes and notifying the UI when updates occur
```csharp
    public ReactiveProperty<bool> IsBought { get; private set; }
    public ReactiveProperty<bool> IsSelected { get; private set; }
    public ReactiveProperty<IdleNumber> BuyPrice { get; private set; }
    public ReactiveProperty<IdleNumber> UpgradePrice { get; private set; }
```


# Localization 🌐:
For localization I used Unity’s localization package. Nice tool to set the text depends on the language, also a very handy thing is localization’s
formatters (choose, list, plural). It's pretty easy to set up "smart values" dynamically and in runtime update text if some value is changed.
<br><img src="https://i.postimg.cc/wjXSZ4nf/Localization.png" alt="Localization" width="600">


# Addressables 📦:
Handy system for managing the loading and unloading of assets efficiently. It allows to organize, package, and dynamically load assets in a flexible way,
which is particularly useful for optimizing memory usage and performance, like FMOD banks and textures loading. Also it helps to minimize asset duplication.
However, they introduce their own limitations. I had to write a separate editor script to configure sprite-atlases during the build.
<br><img src="https://i.postimg.cc/pdPgHYH2/Addressables.png" alt="Addressables" width="600">

# Optimization 🔧:
Even though this project didn't need it, I decided to work on it here too. By optimizing various aspects of the application (such as UI, textures,
audio, and code execution), the overall performance of the application increases. This leads to smoother gameplay, faster load times, and more responsive interactions.
For mobile and portable devices, optimizing applications can lead to reduced battery consumption. Efficient rendering, lower CPU usage, and optimized background
processes can extend battery life for users.


## UI 📺:
To minimize UI batches, textures are packed into Sprite-Atlases. Also I’m using 9-slicing to have large variety of forms of the same texture. I removed “masked”
and “raycast hit” from UI elements that doesn’t need them to remove them from calculation. (it’s not much, but is fair job)


## Textures 🖼️:
To minimize memory usage, I’m trying to compress textures as good as possible (or at least as good as my knowledge allows). In most cases I use “RGBA Crunched ETC2”
or “ASTC” and increase compression as much as possible without losing quality. Also   import textures that are POT, to have better compression, it is
not possible, that putting that texture into Sprite-Atlas resolve that issue. For better compression I imported textures with POT size, but if is not possible
than this issue is easily solved by putting textures into Sprite-atlases. Since Sprite-Atlas V2 need all textures to be uncompressed (it uncompress them anyway in build),
I removed all compression from textures and applied it only on Sprite-Atlases.
<br><img src="https://i.postimg.cc/cHS7jXym/Sprite-Atlas.png" alt="Sprite Atlas" width="300">


## Audio 🎚️:
All audio settings are on FMOD side. To optimize memory space on unity side, I’m using banks that are loaded and unloaded depending on game state (MenuBank for menu scene,
GameBank for game scene). All these banks are loaded and unloaded using Addressables. For more information about Audio Optimization,
check <i><b><a href="https://github.com/CatalinUrsu/Tool_Helpers">Helpers Repository</a></b></i>


## Profiling 🎛️:
Profiling in game engines is a critical process used to analyze the performance of a game during development. It helps developers identify bottlenecks, optimize
resource usage, and ensure a smooth gameplay experience. Unity provides several tools for profiling, including the Profiler, Memory Profiler, and Frame Debugger.

- <h3>Profiler :</h3>  Base tool to check the load of the system (cpu, memory, video, ui. Also a handy tool to check multi-threads jobs execution and addressables. In this project
  I didn’t use it too much, because of the simplicity of the project. The downside is that the data in the editor is not accurate, since the system also takes into
  account actions inside the editor, so for a more correct check you need to profile the final app
  <br><img src="https://i.postimg.cc/2yr08MF3/Profiler.png" alt="Profiler" width="600">

- <h3>MemoryProfiler :</h3>
  I used this tool more often, since in the newer unity version is very useful and more accurate. I used it to check if there’s no lack of memory during scen
  e transitions or if there are some asset duplications.
  <br><img src="https://i.postimg.cc/264HYrWP/Memory-Profiler.png" alt="MemoryProfiler" width="600">

- <h3>FrameDebugger :</h3>
  Because thi is a simple 2D project, most of the batches were on UI. So to check UI optimization results. Also it helps me to find out that SRP doesn’t fully
  support 2D on 2022 version, so I had to update it
  <br><img src="https://i.postimg.cc/qRfmWzNs/Frame-Debug.png" alt="FrameDebugger" width="600">


# Extra 🗃️:
- <h3>Editor scripts :</h3>
  While using addressables with sprite atlases, I had to write my own editor script for Pre and Post processBuild for SpriteAtlases to exclude them from
  build during “build” (sorry for the tautology 🙂).
```csharp
    public static void SetAllIncludeInBuild(bool enable)
    {
        SpriteAtlas[] spriteAtlases = LoadSpriteAtlases();

        foreach (SpriteAtlas atlas in spriteAtlases) 
            SetIncludeInBuild(atlas, enable);
    }

    static void SetIncludeInBuild(SpriteAtlas spriteAtlas, bool enable)
    {
        SerializedObject so = new SerializedObject(spriteAtlas);
        SerializedProperty atlasEditorData = so.FindProperty("m_EditorData");
        SerializedProperty includeInBuild = atlasEditorData.FindPropertyRelative("bindAsDefault");
        includeInBuild.boolValue = enable;
        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(spriteAtlas);
        AssetDatabase.Refresh();
    }

    static SpriteAtlas[] LoadSpriteAtlases()
    {
        string[] findAssets = AssetDatabase.FindAssets($"t: {nameof(SpriteAtlas)}");

        return findAssets.Length == 0
            ? Array.Empty<SpriteAtlas>()
            : findAssets
              .Select(AssetDatabase.GUIDToAssetPath)
              .Select(AssetDatabase.LoadAssetAtPath<SpriteAtlas>)
              .ToArray();
    }
```
Another helpful editor script is [GitPackagesResolver](showcase/Packages/Helpers/Editor/GitPackagesResolver.cs) , since I used some packages from git, like
(UniRX, UniTask, AssetRelationView), I need some dependency for my own packages. So this script import needed packages at app Initialization.

- <h3>Asset Relation View :</h3>
  Very helpful tool that helped me to maintain the project clean. Also is very handy when I was checking where some or other file is used, to minimize
  asset duplication (important thing while using addressables)
  <br><img src="https://i.postimg.cc/rpGWFmhC/Asset-Viewer.png" alt="Asset Relation Viewer" width="600">


# Notes 📜:
- Because of using Addressables for android platform, the “use existing build” play mode on addressables broke shaders, so for normal play / play-test need to use
  “use asset database” play mode.
- While using “use asset database” play mode, Fmod “bank import type” need to be set to streaming assets, so to not change it every time manually, I made an editor
  [script](showcase/Packages/Helpers/Editor/FmodSettingsFix/FmodSettingsOverrideOnPlay.cs) that do this for me each time on start play mode (for that I hade to add this editor script to ScriptExecutionOrder
