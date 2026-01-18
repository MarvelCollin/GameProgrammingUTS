using UnityEngine;
using UnityEngine.UI;

public class ControlsUI : MonoBehaviour
{
    private Canvas canvas;
    private GameObject controlsPanel;
    
    private void Start()
    {
        CreateControlsUI();
    }
    
    private void CreateControlsUI()
    {
        canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("ControlsCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        controlsPanel = new GameObject("ControlsPanel");
        controlsPanel.transform.SetParent(canvas.transform, false);
        
        RectTransform panelRect = controlsPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(0, 0);
        panelRect.pivot = new Vector2(0, 0);
        panelRect.anchoredPosition = new Vector2(20, 20);
        panelRect.sizeDelta = new Vector2(200, 120);
        
        Image bgImage = controlsPanel.AddComponent<Image>();
        bgImage.color = new Color(0, 0, 0, 0.6f);
        
        VerticalLayoutGroup layout = controlsPanel.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(15, 15, 10, 10);
        layout.spacing = 5;
        layout.childAlignment = TextAnchor.UpperLeft;
        layout.childControlWidth = true;
        layout.childControlHeight = true;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        
        CreateControlText("Controls", 16, FontStyle.Bold);
        CreateControlText("WASD - Move", 14, FontStyle.Normal);
        CreateControlText("LMB / Enter - Attack", 14, FontStyle.Normal);
        CreateControlText("E - Dig", 14, FontStyle.Normal);
    }
    
    private void CreateControlText(string text, int fontSize, FontStyle style)
    {
        GameObject textObj = new GameObject(text);
        textObj.transform.SetParent(controlsPanel.transform, false);
        
        Text textComponent = textObj.AddComponent<Text>();
        textComponent.text = text;
        textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        textComponent.fontSize = fontSize;
        textComponent.fontStyle = style;
        textComponent.color = Color.white;
        textComponent.alignment = TextAnchor.MiddleLeft;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(170, 20);
    }
}
