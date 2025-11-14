using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapColliderSetup : MonoBehaviour
{
    [Header("Setup Settings")]
    [SerializeField] private bool autoSetupOnStart = true;
    
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
        GameObject layer2 = GameObject.Find(GameConstants.Layers.Layer2);
        
        if (layer2 != null)
        {
            TilemapCollider2D collider = layer2.GetComponent<TilemapCollider2D>();
            
            if (collider == null)
            {
                collider = layer2.AddComponent<TilemapCollider2D>();
            }
            
            Rigidbody2D rb = layer2.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = layer2.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                rb.gravityScale = 0f;
            }
            
            layer2.layer = LayerMask.NameToLayer(GameConstants.Tags.Environment);
        }
    }
}
