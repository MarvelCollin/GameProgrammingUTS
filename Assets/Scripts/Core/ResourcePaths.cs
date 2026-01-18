public static class ResourcePaths
{
    private const string BaseAssetsPath = "Sunnyside_World_Assets";

    public static class Characters
    {
        private const string CharactersPath = BaseAssetsPath + "/Characters";

        public static class Human
        {
            private const string HumanPath = CharactersPath + "/Human";
            public const string Idle = HumanPath + "/IDLE/base_idle_strip9";
            public const string Hurt = HumanPath + "/HURT/base_hurt_strip8";
            public const string Attack = HumanPath + "/ATTACK/base_attack_strip10";
            public const string AttackTool = HumanPath + "/ATTACK/tools_attack_strip10";
            public const string Dig = HumanPath + "/DIG/base_dig_strip13";
            public const string DigTool = HumanPath + "/DIG/tools_dig_strip13";
        }

        public static class Goblin
        {
            private const string GoblinPath = CharactersPath + "/Goblin/PNG";
            public const string Idle = GoblinPath + "/spr_idle_strip9";
            public const string Attack = GoblinPath + "/spr_attack_strip10";
        }

        public static class Skeleton
        {
            private const string SkeletonPath = CharactersPath + "/Skeleton/PNG";
            public const string Idle = SkeletonPath + "/skeleton_idle_strip6";
            public const string Attack = SkeletonPath + "/skeleton_attack_strip7";
        }
    }

    public static class Crops
    {
        private const string CropsPath = BaseAssetsPath + "/Elements/Crops";

        public static string GetCropPath(string cropName, int stage)
        {
            return $"{CropsPath}/{cropName.ToLower()}_0{stage}";
        }

        public static string GetFullyGrownCrop(string cropName)
        {
            return GetCropPath(cropName, 5);
        }
    }

    public static class Animals
    {
        private const string AnimalsPath = BaseAssetsPath + "/Elements/Animals";

        public static string GetAnimalPath(string animalName)
        {
            return $"{AnimalsPath}/spr_deco_{animalName.ToLower()}_strip4";
        }

        public static string GetAnimalPathVariant(string animalName, string variant)
        {
            return $"{AnimalsPath}/spr_deco_{animalName.ToLower()}_{variant}_strip4";
        }
    }
}
