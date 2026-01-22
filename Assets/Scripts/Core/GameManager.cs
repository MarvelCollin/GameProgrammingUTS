using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private Transform playerTransform;
    private int goblinCount = 3;
    private float goblinSpawnRadius = 5f;

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
            SpawnGoblins();
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
}
