using UnityEngine;

public static class SpriteFactory
{
    public static Sprite LoadSprite(string path)
    {
        Sprite sprite = Resources.Load<Sprite>(path);
        if (sprite == null)
        {
            Debug.LogWarning($"Failed to load sprite at path: {path}");
        }
        return sprite;
    }

    public static Sprite[] LoadSpriteStrip(string path)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(path);
        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning($"Failed to load sprite strip at path: {path}");
        }
        return sprites;
    }

    public static Sprite GetCropSprite(CropType cropType)
    {
        string cropName = cropType.ToString();
        string path = ResourcePaths.Crops.GetFullyGrownCrop(cropName);
        return LoadSprite(path);
    }

    public static Sprite[] GetCropGrowthSprites(CropType cropType)
    {
        string cropName = cropType.ToString();
        Sprite[] sprites = new Sprite[6];
        
        for (int i = 0; i <= 5; i++)
        {
            string path = ResourcePaths.Crops.GetCropPath(cropName, i);
            sprites[i] = LoadSprite(path);
        }
        
        return sprites;
    }

    public static Sprite[] GetAnimalSprites(AnimalType animalType)
    {
        string path = GetAnimalSpritePath(animalType);
        return LoadSpriteStrip(path);
    }

    private static string GetAnimalSpritePath(AnimalType animalType)
    {
        switch (animalType)
        {
            case AnimalType.Cow:
                return ResourcePaths.Animals.GetAnimalPath("cow");
            case AnimalType.Chicken:
                return ResourcePaths.Animals.GetAnimalPathVariant("chicken", "01");
            case AnimalType.Pig:
                return ResourcePaths.Animals.GetAnimalPathVariant("pig", "01");
            case AnimalType.Sheep:
                return ResourcePaths.Animals.GetAnimalPathVariant("sheep", "01");
            case AnimalType.Duck:
                return ResourcePaths.Animals.GetAnimalPathVariant("duck", "01");
            case AnimalType.Bird:
                return ResourcePaths.Animals.GetAnimalPathVariant("bird", "01");
            default:
                return string.Empty;
        }
    }

    public static Sprite[] GetNPCSprites(NPCType npcType)
    {
        string path = GetNPCSpritePath(npcType);
        return LoadSpriteStrip(path);
    }

    private static string GetNPCSpritePath(NPCType npcType)
    {
        switch (npcType)
        {
            case NPCType.Human:
                return ResourcePaths.Characters.Human.Idle;
            case NPCType.Goblin:
                return ResourcePaths.Characters.Goblin.Idle;
            case NPCType.Skeleton:
                return ResourcePaths.Characters.Skeleton.Idle;
            default:
                return string.Empty;
        }
    }
}
