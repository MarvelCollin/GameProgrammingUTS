using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("World Boundaries")]
    [SerializeField] private float minX = -20f;
    [SerializeField] private float maxX = 20f;
    [SerializeField] private float minY = -20f;
    [SerializeField] private float maxY = 20f;
    
    [Header("Collision Settings")]
    [SerializeField] private Vector2 colliderSize = new Vector2(0.2f, 0.2f);
    [SerializeField] private Vector2 colliderOffset = Vector2.zero;
    
    [Header("Debug")]
    [SerializeField] private bool showCollisionBorder = false;
    [SerializeField] private Color borderColor = Color.red;
    [SerializeField] private float borderWidth = 0.05f;
    
    [Header("Combat Settings")]
    [SerializeField] private float hurtAnimationDuration = 0.5f;
    
    [Header("Collection")]
    [SerializeField] private int cropCount = 0;

    public float MinX => minX;
    public float MaxX => maxX;
    public float MinY => minY;
    public float MaxY => maxY;
    
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 moveInput;
    private PlayerAnimator playerAnimator;
    private IPlayerState currentState;
    private PlayerNormalState normalState;
    private PlayerHurtState hurtState;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        
        normalState = new PlayerNormalState(this, rb, playerAnimator, moveSpeed);
        hurtState = new PlayerHurtState(rb, playerAnimator, hurtAnimationDuration);
        currentState = normalState;
        
        SetupCollider();
    }
    
    private void SetupCollider()
    {
        if (boxCollider != null)
        {
            boxCollider.isTrigger = false;
            
            Vector2 adjustedSize = colliderSize;
            Vector3 scale = transform.localScale;
            if (scale.x != 0) adjustedSize.x /= Mathf.Abs(scale.x);
            if (scale.y != 0) adjustedSize.y /= Mathf.Abs(scale.y);
            
            boxCollider.size = adjustedSize;
            boxCollider.offset = colliderOffset;
            
            if (showCollisionBorder)
            {
                CreateVisualBorder();
            }
        }
    }
    
    private void Start()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }
    

    
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        currentState.UpdateMovement(moveInput);
    }

    private void ChangeState(IPlayerState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
    
    private void FixedUpdate()
    {
        if (currentState == hurtState && hurtState.IsComplete())
        {
            ChangeState(normalState);
        }
        
        currentState.FixedUpdate();
    }
    
    private void CreateVisualBorder()
    {
        GameObject borderObj = new GameObject("PlayerCollisionBorder");
        borderObj.transform.SetParent(transform);
        borderObj.transform.localPosition = (Vector3)colliderOffset;
        
        LineRenderer lineRenderer = borderObj.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = borderColor;
        lineRenderer.endColor = borderColor;
        lineRenderer.startWidth = borderWidth;
        lineRenderer.endWidth = borderWidth;
        lineRenderer.positionCount = 5;
        lineRenderer.useWorldSpace = false;
        lineRenderer.sortingOrder = 100;
        
        float halfWidth = colliderSize.x / 2f;
        float halfHeight = colliderSize.y / 2f;
        
        lineRenderer.SetPosition(0, new Vector3(-halfWidth, -halfHeight, 0));
        lineRenderer.SetPosition(1, new Vector3(halfWidth, -halfHeight, 0));
        lineRenderer.SetPosition(2, new Vector3(halfWidth, halfHeight, 0));
        lineRenderer.SetPosition(3, new Vector3(-halfWidth, halfHeight, 0));
        lineRenderer.SetPosition(4, new Vector3(-halfWidth, -halfHeight, 0));
    }
    
    public void TakeDamage(Vector2 recoilDirection, float recoilForce)
    {
        ChangeState(hurtState);
        hurtState.ApplyKnockback(recoilDirection, recoilForce);
    }
    
    public void CollectCrop()
    {
        cropCount++;
    }
    
    public int GetCropCount()
    {
        return cropCount;
    }

}
