using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Transform playerTransform;
    private int goblinCount = 3;
    private float goblinSpawnRadius = 5f;
    private int skeletonCount = 3;
    private float skeletonSpawnRadius = 7f;
    private int cropCount = 6;
    private int animalCount = 4;
    private float animalSpawnRadius = 4f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            SpawnCrops();
            SpawnAnimals();
            SpawnGoblins();
            SpawnSkeletons();
        }
    }

    private void InitializeManagers()
    {
        if (CropDataManager.Instance == null)
        {
            GameObject cropDataManagerObj = new GameObject("CropDataManager");
            cropDataManagerObj.AddComponent<CropDataManager>();
        }

        if (WelcomeUIManager.Instance == null)
        {
            GameObject welcomeUIObj = new GameObject("WelcomeUIManager");
            welcomeUIObj.AddComponent<WelcomeUIManager>();
        }

        if (CollectibleUIManager.Instance == null)
        {
            GameObject collectibleUIObj = new GameObject("CollectibleUIManager");
            collectibleUIObj.AddComponent<CollectibleUIManager>();
        }
    }

    private void SpawnCrops()
    {
        CropType[] cropTypes = { CropType.Carrot, CropType.Potato, CropType.Wheat, CropType.Pumpkin, CropType.Cabbage, CropType.Beetroot };
        
        float startX = -6f;
        float endX = 6f;
        float startY = -3f;
        float endY = 3f;
        
        for (int i = 0; i < cropCount; i++)
        {
            float x = startX + (endX - startX) * ((float)i / (cropCount - 1)) + Random.Range(-1f, 1f);
            float y = Random.Range(startY, endY);
            Vector3 spawnPosition = new Vector3(x, y, 0);
            
            CropType cropType = cropTypes[i % cropTypes.Length];
            CreateCrop(spawnPosition, cropType, i + 1);
        }
    }

    private void CreateCrop(Vector3 position, CropType cropType, int index)
    {
        GameObject cropObj = new GameObject($"Crop_{cropType}_{index}");
        cropObj.transform.position = position;
        
        SpriteRenderer spriteRenderer = cropObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 3;
        
        CircleCollider2D collider = cropObj.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        
        CropController cropController = cropObj.AddComponent<CropController>();
        cropController.SetCropType(cropType);
    }

    private void SpawnAnimals()
    {
        AnimalType[] animalTypes = { AnimalType.Cow, AnimalType.Chicken, AnimalType.Pig, AnimalType.Sheep };
        
        for (int i = 0; i < animalCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * animalSpawnRadius;
            Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x + 3f, randomOffset.y, 0);
            
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -8f, 8f);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -4f, 4f);
            
            AnimalType animalType = animalTypes[i % animalTypes.Length];
            CreateAnimal(spawnPosition, animalType, i + 1);
        }
    }

    private void CreateAnimal(Vector3 position, AnimalType animalType, int index)
    {
        GameObject animalObj = new GameObject($"Animal_{animalType}_{index}");
        animalObj.transform.position = position;
        
        SpriteRenderer spriteRenderer = animalObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 4;
        
        CircleCollider2D collider = animalObj.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        
        AnimalController animalController = animalObj.AddComponent<AnimalController>();
        animalController.SetAnimalType(animalType);
    }

    private void SpawnGoblins()
    {
        for (int i = 0; i < goblinCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle * goblinSpawnRadius;
            Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -8f, 8f);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -4f, 4f);
            
            CreateGoblin(spawnPosition, i + 1);
        }
    }

    private void CreateGoblin(Vector3 position, int index)
    {
        GameObject goblinObj = new GameObject($"Goblin_{index}");
        goblinObj.transform.position = position;
        
        SpriteRenderer spriteRenderer = goblinObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 5;
        
        CircleCollider2D collider = goblinObj.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;
        
        NPCController npcController = goblinObj.AddComponent<NPCController>();
        npcController.SetNPCType(NPCType.Goblin);
        npcController.SetNPCName($"Goblin {index}");
    }

    private void SpawnSkeletons()
    {
        for (int i = 0; i < skeletonCount; i++)
        {
            Vector2 randomOffset = Random.insideUnitCircle.normalized * skeletonSpawnRadius;
            randomOffset += Random.insideUnitCircle * 2f;
            Vector3 spawnPosition = playerTransform.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -8f, 8f);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -4f, 4f);
            
            CreateSkeleton(spawnPosition, i + 1);
        }
    }

    private void CreateSkeleton(Vector3 position, int index)
    {
        GameObject skeletonObj = new GameObject($"Skeleton_{index}");
        skeletonObj.transform.position = position;
        skeletonObj.transform.localScale = new Vector3(5f, 5f, 1f);
        
        SpriteRenderer spriteRenderer = skeletonObj.AddComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = 5;
        
        skeletonObj.AddComponent<SkeletonController>();
    }
}
