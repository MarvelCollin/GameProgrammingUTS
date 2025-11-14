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
        triggerCollider.radius = GameConstants.Physics.DefaultTriggerRadius;
        
        transform.localScale = new Vector3(GameConstants.Physics.DefaultScale, GameConstants.Physics.DefaultScale, 1f);
        
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
            Sprite[] sprites = SpriteFactory.LoadSpriteStrip(ResourcePaths.Characters.Goblin.Idle);
            if (sprites != null && sprites.Length > 0)
            {
                spriteRenderer.sprite = sprites[0];
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameConstants.Tags.Player))
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
                MessageBroadcaster.Instance.SendMessageToObject(gameObject, message);
                Debug.Log(message);
            }
        }
    }
}
