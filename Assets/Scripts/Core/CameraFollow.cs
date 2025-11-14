using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Follow Target")]
    [SerializeField] private Transform target;
    
    [Header("Camera Smoothing")]
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    
    [Header("Camera Boundaries")]
    [SerializeField] private bool useBoundaries = true;
    [SerializeField] private float minX = -50f;
    [SerializeField] private float maxX = 50f;
    [SerializeField] private float minY = -50f;
    [SerializeField] private float maxY = 50f;
    
    private Camera cam;
    private float cameraHalfWidth;
    private float cameraHalfHeight;
    
    private void Awake()
    {
        cam = GetComponent<Camera>();
        FindPlayerTarget();
    }
    
    private void FindPlayerTarget()
    {
        if (target != null)
        {
            return;
        }
        
        GameObject player = GameObject.FindGameObjectWithTag(GameConstants.Tags.Player);
        if (player != null)
        {
            target = player.transform;
            return;
        }
        
        player = GameObject.Find(GameConstants.Tags.Player);
        if (player != null)
        {
            target = player.transform;
            return;
        }
    }
    
    private void Start()
    {
        if (target == null)
        {
            FindPlayerTarget();
        }
        
        CalculateCameraBounds();
    }
    
    private void LateUpdate()
    {
        if (target == null) return;
        
        Vector3 targetPosition = target.position;
        Vector3 desiredPosition = targetPosition + offset;
        
        if (useBoundaries)
        {
            float minCameraX = minX + cameraHalfWidth;
            float maxCameraX = maxX - cameraHalfWidth;
            float minCameraY = minY + cameraHalfHeight;
            float maxCameraY = maxY - cameraHalfHeight;
            
            if (minCameraX < maxCameraX)
            {
                desiredPosition.x = Mathf.Clamp(desiredPosition.x, minCameraX, maxCameraX);
            }
            else
            {
                desiredPosition.x = (minX + maxX) / 2f;
            }
            
            if (minCameraY < maxCameraY)
            {
                desiredPosition.y = Mathf.Clamp(desiredPosition.y, minCameraY, maxCameraY);
            }
            else
            {
                desiredPosition.y = (minY + maxY) / 2f;
            }
        }
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        smoothedPosition.z = offset.z;
        transform.position = smoothedPosition;
    }
    
    private void CalculateCameraBounds()
    {
        cameraHalfHeight = cam.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * cam.aspect;
    }
    
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    
    public void SetBoundaries(float minXBound, float maxXBound, float minYBound, float maxYBound)
    {
        minX = minXBound;
        maxX = maxXBound;
        minY = minYBound;
        maxY = maxYBound;
    }
    
    public void SetSmoothSpeed(float speed)
    {
        smoothSpeed = speed;
    }
    
    [ContextMenu("Reset Boundaries to Large (50x50)")]
    private void ResetBoundariesToLarge()
    {
        minX = -50f;
        maxX = 50f;
        minY = -50f;
        maxY = 50f;
    }
    
    [ContextMenu("Disable Boundaries")]
    private void DisableBoundaries()
    {
        useBoundaries = false;
    }
    
    [ContextMenu("Enable Boundaries")]
    private void EnableBoundaries()
    {
        useBoundaries = true;
    }
    
    private void OnValidate()
    {
        if (cam != null)
        {
            CalculateCameraBounds();
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (!useBoundaries) return;
        
        Gizmos.color = Color.yellow;
        Vector3 topLeft = new Vector3(minX, maxY, 0);
        Vector3 topRight = new Vector3(maxX, maxY, 0);
        Vector3 bottomLeft = new Vector3(minX, minY, 0);
        Vector3 bottomRight = new Vector3(maxX, minY, 0);
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
