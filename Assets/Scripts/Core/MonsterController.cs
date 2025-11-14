using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class MonsterController : MonoBehaviour
{
    [Header("Monster Settings")]
    [SerializeField] private float recoilForce = 5f;
    
    private SpriteRenderer spriteRenderer;
    private WorldSpaceUI worldSpaceUI;
    private CircleCollider2D triggerCollider;
    private MonsterAnimator monsterAnimator;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        triggerCollider = GetComponent<CircleCollider2D>();
        worldSpaceUI = GetComponent<WorldSpaceUI>();
        monsterAnimator = GetComponent<MonsterAnimator>();
        
        if (worldSpaceUI == null)
        {
            worldSpaceUI = gameObject.AddComponent<WorldSpaceUI>();
            worldSpaceUI.SetOffset(new Vector3(0, -0.25f, 0));
        }
        
        if (monsterAnimator == null)
        {
            Animator animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = gameObject.AddComponent<Animator>();
            }
            monsterAnimator = gameObject.AddComponent<MonsterAnimator>();
        }
        
        triggerCollider.isTrigger = true;
        triggerCollider.radius = 0.5f;
        
        transform.localScale = new Vector3(5f, 5f, 1f);
        
        LoadSprite();
    }
    
    private void Start()
    {
        if (monsterAnimator != null)
        {
            monsterAnimator.PlayIdleAnimation();
        }
    }
    
    private void LoadSprite()
    {
        if (spriteRenderer.sprite == null)
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sunnyside_World_Assets/Characters/Goblin/PNG/spr_idle_strip9");
            if (sprites != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[0];
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                Vector2 recoilDirection = (other.transform.position - transform.position).normalized;
                player.TakeDamage(recoilDirection, recoilForce);
                
                if (monsterAnimator != null)
                {
                    monsterAnimator.PlayAttackAnimation();
                }
                
                string message = "Player is hurt";
                
                if (worldSpaceUI != null)
                {
                    worldSpaceUI.ShowMessage(message);
                }
                
                GameUIManager.Instance?.ShowMessage(message);
                Debug.Log(message);
            }
        }
    }
}
