using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerSortingManager : MonoBehaviour
{
    [Header("Layer Settings")]
    [SerializeField] private bool autoSetupOnStart = true;
    
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
            }
        }
        
        if (layer2 != null)
        {
            TilemapRenderer renderer2 = layer2.GetComponent<TilemapRenderer>();
            if (renderer2 != null)
            {
                renderer2.sortingOrder = 1;
            }
        }
    }
}
