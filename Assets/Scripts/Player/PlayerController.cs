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
    
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 moveInput;
    private PlayerAnimator playerAnimator;
    private bool isHurt = false;
    private float hurtTimer = 0f;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<PlayerAnimator>();
        
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
    }
    
    private void FixedUpdate()
    {
        if (isHurt)
        {
            hurtTimer -= Time.fixedDeltaTime;
            if (hurtTimer <= 0f)
            {
                isHurt = false;
                if (playerAnimator != null)
                {
                    playerAnimator.StopHurt();
                }
            }
            return;
        }
        
        Vector2 velocity = moveInput * moveSpeed;
        rb.linearVelocity = velocity;
        
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
        
        if (playerAnimator != null)
        {
            playerAnimator.UpdateMovement(moveInput);
        }
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
        isHurt = true;
        hurtTimer = hurtAnimationDuration;
        
        rb.linearVelocity = recoilDirection * recoilForce;
        
        if (playerAnimator != null)
        {
            playerAnimator.PlayHurt();
        }
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
