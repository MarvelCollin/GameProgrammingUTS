using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldBoundaryManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private CameraFollow cameraFollow;
    
    [Header("Auto-detect")]
    [SerializeField] private bool autoDetectOnStart = true;
    [SerializeField] private string tilemapLayerName = "Layer1";
    
    private void Start()
    {
        if (autoDetectOnStart)
        {
            CalculateAndSetBoundaries();
        }
    }
    
    [ContextMenu("Calculate Boundaries from Tilemap")]
    public void CalculateAndSetBoundaries()
    {
        GameObject layerObj = GameObject.Find(tilemapLayerName);
        
        if (layerObj == null)
        {
            Debug.LogWarning($"Tilemap '{tilemapLayerName}' not found!");
            return;
        }
        
        Tilemap tilemap = layerObj.GetComponent<Tilemap>();
        
        if (tilemap == null)
        {
            Debug.LogWarning($"No Tilemap component found on '{tilemapLayerName}'!");
            return;
        }
        
        tilemap.CompressBounds();
        BoundsInt bounds = tilemap.cellBounds;
        
        float minX = bounds.xMin;
        float maxX = bounds.xMax;
        float minY = bounds.yMin;
        float maxY = bounds.yMax;
        
        if (playerController != null)
        {
            playerController.SetBoundaries(minX, maxX, minY, maxY);
            Debug.Log($"Player boundaries set to: X({minX} to {maxX}), Y({minY} to {maxY})");
        }
        
        if (cameraFollow != null)
        {
            cameraFollow.SetBoundaries(minX, maxX, minY, maxY);
            Debug.Log($"Camera boundaries set to: X({minX} to {maxX}), Y({minY} to {maxY})");
        }
        
        float width = maxX - minX;
        float height = maxY - minY;
        Debug.Log($"World size: Width={width}, Height={height}");
    }
}
