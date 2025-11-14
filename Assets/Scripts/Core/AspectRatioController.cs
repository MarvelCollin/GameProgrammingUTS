using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioController : MonoBehaviour
{
    [Header("Target Aspect Ratio")]
    [SerializeField] private float targetAspectWidth = 16f;
    [SerializeField] private float targetAspectHeight = 9f;
    
    [Header("Camera Settings")]
    [SerializeField] private Color letterboxColor = Color.black;
    
    private Camera cam;
    
    private void Awake()
    {
        cam = GetComponent<Camera>();
        ApplyAspectRatio();
    }
    
    private void Start()
    {
        float targetAspect = targetAspectWidth / targetAspectHeight;
    }
    
    private void ApplyAspectRatio()
    {
        float targetAspect = targetAspectWidth / targetAspectHeight;
        float windowAspect = (float)Screen.width / Screen.height;
        float scaleHeight = windowAspect / targetAspect;
        
        if (scaleHeight < 1f)
        {
            Rect rect = cam.rect;
            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1f - scaleHeight) / 2f;
            cam.rect = rect;
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0;
            cam.rect = rect;
        }
        
        cam.backgroundColor = letterboxColor;
    }
    
    private void Update()
    {
        ApplyAspectRatio();
    }
    
    public void SetAspectRatio(float width, float height)
    {
        targetAspectWidth = width;
        targetAspectHeight = height;
        ApplyAspectRatio();
    }
}
