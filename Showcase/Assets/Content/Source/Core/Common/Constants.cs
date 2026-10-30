using Helpers;
using UnityEngine;

namespace Source
{
public static class ConstSceneNames
{
    public const string INIT_SCENE = "Init";
    public const string LOADING_SCENE = "Loading";
    public const string MENU_SCENE = "Menu";
    public const string GAME_SCENE = "Gameplay";
}

public static class ConstSpriteAssets
{
    public const string SPRITE_TEXT_COIN = "<sprite=\"S_ui_TextSprites\" name=\"Coin\" tint=1>";
    public const string SPRITE_TEXT_DIAMOND = "<sprite=\"S_ui_TextSprites\" name=\"Diamond\" tint=1>";
    public const string SPRITE_TEXT_FIRE_POWER = "<sprite=\"S_ui_TextSprites\" name=\"Fire_Power\" tint=1>";
    public const string SPRITE_TEXT_FIRE_RATE = "<sprite=\"S_ui_TextSprites\" name=\"Fire_Rate\" tint=1>";
}

public static class ConstCameras
{
    public const string CAMERA_UI = "Camera_UI";
}

public static class ConstUIAnimation
{
    public const float SPLASH_SCREEN_ANIM_DUR = .5f;
    public const float UI_ANIM_DUR = .25f;
    public const float ITEM_SPAWN_DELAY = .05f;
    public const float ITEM_AVAILABLE_ALPHA = 1;
    public const float ITEM_NOT_AVAILABLE_ALPHA = .15f;
    public static readonly Vector2 ITEM_ANIM_SIZE = new(.05f, .05f);

    public static float GetAnimDuration(bool skipAnimation) => skipAnimation ? 0 : UI_ANIM_DUR;
}

public static class ConstGameplay
{
    public const string ENEMIES_LOCATION_KEY = "Enemy";
}

public static class ConstEnemy
{
    public const int HIT_ROT_MULTIPLIER = 3;
    public const float HIT_ROT_MAX = 1.5f;
    public const float HIT_ROT_DRAG = .003f;
    public const float HIT_SCALE_DURATION = .5f;
    public static Vector3 HIT_SCALE_PUNCH = new(.25F, .25F, 0);
    public const float IDLE_ROTATION = .3f;
}

public static class ConstFMOD
{
    public const string ITEM_APPEAR_PITCH = "ItemAppearPitch";
    public const string FLY_POWER = "FlyPower";
    public const string MUSIC_STATE = "MusicState";
    public const string VCA_Sound = "vca:/Sounds";
    public const string VCA_Music = "vca:/Music";
    public const string SNAPSHOT_PAUSE = "snapshot:/Pausing";
}

public static class ConstSavesPaths
{
    public static readonly string SETTINGS_PATH = ConstSession.SAVES_FOLDER_PATH + "/settings.json";
    public static readonly string PROGRESS_PATH = ConstSession.SAVES_FOLDER_PATH + "/progress.json";
    public static readonly string ITEMS_PATH = ConstSession.SAVES_FOLDER_PATH + "/items.json";
}
}