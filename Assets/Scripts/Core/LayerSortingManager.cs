using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerSortingManager : MonoBehaviour
{
    [Header("Layer Settings")]
    [SerializeField] private bool autoSetupOnStart = true;
    
    [Header("Debug")]
    [SerializeField] private bool showDebugLogs = true;
    
    private void Start()
    {
        if (autoSetupOnStart)
        {
            SetupLayerSorting();
        }
    }
    
    [ContextMenu("Setup Layer Sorting")]
    public void SetupLayerSorting()
    {
        GameObject layer1 = GameObject.Find("Layer1");
        GameObject layer2 = GameObject.Find("Layer2");
        
        if (layer1 != null)
        {
            TilemapRenderer renderer1 = layer1.GetComponent<TilemapRenderer>();
            if (renderer1 != null)
            {
                renderer1.sortingOrder = 0;
                
                if (showDebugLogs)
                {
                    Debug.Log($"[LayerSorting] Layer1 sorting order set to: 0");
                    Debug.Log($"[LayerSorting] Layer1 sorting layer: {renderer1.sortingLayerName}");
                }
            }
            else if (showDebugLogs)
            {
                Debug.LogWarning($"[LayerSorting] Layer1 found but has no TilemapRenderer!");
            }
        }
        else if (showDebugLogs)
        {
            Debug.LogWarning($"[LayerSorting] Layer1 GameObject not found!");
        }
        
        if (layer2 != null)
        {
            TilemapRenderer renderer2 = layer2.GetComponent<TilemapRenderer>();
            if (renderer2 != null)
            {
                renderer2.sortingOrder = 1;
                
                if (showDebugLogs)
                {
                    Debug.Log($"[LayerSorting] Layer2 sorting order set to: 1");
                    Debug.Log($"[LayerSorting] Layer2 sorting layer: {renderer2.sortingLayerName}");
                    Debug.Log($"========================================");
                    Debug.Log($"[LayerSorting] Layer2 will now render ABOVE Layer1");
                    Debug.Log($"========================================");
                }
            }
            else if (showDebugLogs)
            {
                Debug.LogWarning($"[LayerSorting] Layer2 found but has no TilemapRenderer!");
            }
        }
        else if (showDebugLogs)
        {
            Debug.LogWarning($"[LayerSorting] Layer2 GameObject not found!");
        }
    }
}
