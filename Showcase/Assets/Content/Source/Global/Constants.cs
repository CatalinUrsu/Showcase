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

public static class ConstUIAnimation
{
    public const float SPLASH_SCREEN_ANIM_DUR = .5f;
    public const float UI_ANIM_DUR = .25f;
    public const float ITEM_SPAWN_DELAY = .1f;
    public const float ITEM_AVAILABLE_ALPHA = 1;
    public const float ITEM_NOT_AVAILABLE_ALPHA = .15f;
    public static readonly Vector2 ITEM_ANIM_SIZE = new(10f, 10f);

    public static float GetAnimDuration(bool skipAnimation) => skipAnimation ? 0 : UI_ANIM_DUR;
}

public static class ConstUpgradeItems
{
    public const float SHIP_PRICE_MULTIPLIER = 1.2f;
    public const float SHIP_BONUS_INCREASE = .25f;
    public const float WEAPON_PRICE_MULTIPLIER = 1.2f;
    public const float WEAPON_POWER_UPGRADE = 1;
    public const float WEAPON_FIRE_RATE_UPGRADE = -.005f;
    public const float WEAPON_FIRE_RATE_MIN = .2f;
    public const float RESET_PROGRESS_MULTIPLIER = 2.36f;
    public const int RESET_PROGRESS_MIN_LVL = 5;
}

public static class ConstGameplay
{
    public const int PROGRESS_TARGET = 10;
    public const int BULLETS_SPAWN_COUNT = 20;
    public const int ENEMIES_SPAWN_COUNT = 10;
    public const float ENEMIES_SPAWN_FREQUENCY = .75f;
    public const float ENEMY_HP_MULTIPLIER = 1.3f;
    public const float COINS_LVL_BONUS_MULTIPLIER = .25f;
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
}