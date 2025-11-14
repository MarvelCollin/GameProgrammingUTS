using UnityEngine;

public static class GameConstants
{
    public static class Tags
    {
        public const string Player = "Player";
        public const string Environment = "Environment";
    }

    public static class Layers
    {
        public const string Layer1 = "Layer1";
        public const string Layer2 = "Layer2";
    }

    public static class Physics
    {
        public const float DefaultTriggerRadius = 0.5f;
        public const float DefaultScale = 5f;
        public const int PlayerSortingOrder = 10;
    }

    public static class Animation
    {
        public const float DefaultFrameDelay = 0.1f;
        public const float FastFrameDelay = 0.05f;
        public const float SlowFrameDelay = 0.15f;
    }

    public static class UI
    {
        public const float DefaultMessageDuration = 2f;
        public const float DefaultFadeSpeed = 2f;
        public const float WorldCanvasScale = 0.002f;
    }
}
