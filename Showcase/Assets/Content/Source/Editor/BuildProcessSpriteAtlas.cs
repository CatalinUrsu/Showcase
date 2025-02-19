﻿using UnityEngine;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Source.Editor
{
public class BuildProcessSpriteAtlas : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder { get; } = 0;

    public void OnPreprocessBuild(BuildReport report)
    {
        Debug.Log("----- SpriteAtlas PreprocessBuild start");

        Debug.Log("When loading the sprite atlases via Addressable, we should avoid they getting build into the app. " +
                  "Otherwise, the sprite atlases will get loaded into memory twice.");

        SpriteAtlasUtils.SetAllIncludeInBuild(false);

        Debug.Log("----- SpriteAtlas PreprocessBuild end");
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("When loading the sprite atlases via Addressable, we should avoid they getting build into the app. " +
                  "Otherwise, the sprite atlases will get loaded into memory twice.");

        Debug.Log("Set the `IncludeInBuild` flag to `true` for all sprite atlases after finishing the App build.");
        SpriteAtlasUtils.SetAllIncludeInBuild(true);
    }
}
}