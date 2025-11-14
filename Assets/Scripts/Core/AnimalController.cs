using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class AnimalController : MonoBehaviour
{
    [Header("Animal Settings")]
    [SerializeField] private AnimalType animalType = AnimalType.Cow;
    [SerializeField] private string animalSound = "MOO";
    
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D triggerCollider;
    private float lastSoundTime = 0f;
    private float soundCooldown = 1f;
    private WorldSpaceUI worldSpaceUI;
    private AnimalAnimator animalAnimator;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<CircleCollider2D>();
        worldSpaceUI = GetComponent<WorldSpaceUI>();
        animalAnimator = GetComponent<AnimalAnimator>();
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, -0.4f, 0));
        }
        
        if (animalAnimator == null)
        {
            animalAnimator = gameObject.AddComponent<AnimalAnimator>();
        }
        
        triggerCollider.isTrigger = true;
        triggerCollider.radius = 0.5f;
        
        transform.localScale = new Vector3(5f, 5f, 1f);
        
        SetAnimalSound();
        LoadSprite();
    }
    
    private void LoadSprite()
    {
        if (spriteRenderer.sprite == null)
        {
            string spritePath = GetAnimalSpritePath();
            if (!string.IsNullOrEmpty(spritePath))
            {
                Sprite[] sprites = Resources.LoadAll<Sprite>(spritePath);
                if (sprites != null && sprites.Length > 0)
                {
                    spriteRenderer.sprite = sprites[0];
                }
            }
        }
    }
    
    private void SetAnimalSound()
    {
        switch (animalType)
        {
            case AnimalType.Cow:
                animalSound = "MOO";
                break;
            case AnimalType.Chicken:
                animalSound = "CLUCK CLUCK";
                break;
            case AnimalType.Pig:
                animalSound = "OINK";
                break;
            case AnimalType.Sheep:
                animalSound = "BAA";
                break;
            case AnimalType.Duck:
                animalSound = "QUACK";
                break;
            case AnimalType.Bird:
                animalSound = "CHIRP";
                break;
        }
    }
    
    public string GetAnimalSpritePath()
    {
        switch (animalType)
        {
            case AnimalType.Cow:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_cow_strip4";
            case AnimalType.Chicken:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_chicken_01_strip4";
            case AnimalType.Pig:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_pig_01_strip4";
            case AnimalType.Sheep:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_sheep_01_strip4";
            case AnimalType.Duck:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_duck_01_strip4";
            case AnimalType.Bird:
                return "Sunnyside_World_Assets/Elements/Animals/spr_deco_bird_01_strip4";
            default:
                return "";
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastSoundTime >= soundCooldown)
            {
                lastSoundTime = Time.time;
                
                if (animalAnimator != null)
                {
                    animalAnimator.PlayInteractionAnimation();
                }
                
                if (worldSpaceUI != null)
                {
                    worldSpaceUI.ShowMessage(animalSound);
                }
                
                GameUIManager.Instance?.ShowMessage(animalSound);
                Debug.Log(animalSound);
            }
        }
    }
}

public enum AnimalType
{
    Cow,
    Chicken,
    Pig,
    Sheep,
    Duck,
    Bird
}
