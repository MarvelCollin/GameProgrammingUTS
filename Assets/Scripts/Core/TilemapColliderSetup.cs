using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapColliderSetup : MonoBehaviour
{
    [Header("Setup Settings")]
    [SerializeField] private bool autoSetupOnStart = true;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupColliders();
        }
    }
    
    [ContextMenu("Setup Tilemap Colliders")]
    public void SetupColliders()
    {
        GameObject layer2 = GameObject.Find("Layer2");
        
        if (layer2 != null)
        {
            TilemapCollider2D collider = layer2.GetComponent<TilemapCollider2D>();
            
            if (collider == null)
            {
                collider = layer2.AddComponent<TilemapCollider2D>();
                if (showDebugLogs)
                {
                    Debug.Log("[TilemapCollider] Added TilemapCollider2D to Layer2");
                }
            }
            
            Rigidbody2D rb = layer2.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = layer2.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                rb.gravityScale = 0f;
                
                if (showDebugLogs)
                {
                    Debug.Log("[TilemapCollider] Added Static Rigidbody2D to Layer2");
                }
            }
            
            layer2.layer = LayerMask.NameToLayer("Environment");
            
            if (showDebugLogs)
            {
                Debug.Log("[TilemapCollider] Layer2 setup complete");
                Debug.Log("[TilemapCollider] Layer2 will now have collision on each tile");
            }
        }
        else if (showDebugLogs)
        {
            Debug.LogWarning("[TilemapCollider] Layer2 GameObject not found!");
        }
    }
}
