using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WorldSpaceUI : MonoBehaviour, IMessageObserver
{
    [Header("UI Settings")]
    [SerializeField] private Canvas worldCanvas;
    [SerializeField] private Text messageText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);
    [SerializeField] private float displayDuration = 2f;
    [SerializeField] private float fadeSpeed = 2f;
    
    private bool isShowing = false;
    private Coroutine fadeCoroutine;
    
    private void Awake()
    {
        if (worldCanvas == null || messageText == null || canvasGroup == null)
        {
            CreateUIElements();
        }
    }
    
    public void OnMessageReceived(string message)
    {
    }

    public void ShowMessageDirect(string message)
    {
        ShowMessage(message);
    }
    
    private void CreateUIElements()
    {
        GameObject canvasObj = new GameObject("WorldSpaceCanvas");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = offset;
        canvasObj.transform.localScale = Vector3.one * GameConstants.UI.WorldCanvasScale;
        
        worldCanvas = canvasObj.AddComponent<Canvas>();
        worldCanvas.renderMode = RenderMode.WorldSpace;
        
        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(200, 50);
        
        canvasGroup = canvasObj.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        
        GameObject textObj = new GameObject("MessageText");
        textObj.transform.SetParent(canvasObj.transform);
        textObj.transform.localPosition = Vector3.zero;
        textObj.transform.localScale = Vector3.one;
        
        messageText = textObj.AddComponent<Text>();
        messageText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        messageText.fontSize = 24;
        messageText.alignment = TextAnchor.MiddleCenter;
        messageText.color = Color.white;
        messageText.horizontalOverflow = HorizontalWrapMode.Overflow;
        messageText.verticalOverflow = VerticalWrapMode.Overflow;
        
        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;
        
        Outline outline = textObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2, -2);
    }
    
    public void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            
            fadeCoroutine = StartCoroutine(FadeMessage());
        }
    }
    
    private IEnumerator FadeMessage()
    {
        isShowing = true;
        canvasGroup.alpha = 1f;
        
        yield return new WaitForSeconds(displayDuration);
        
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        
        canvasGroup.alpha = 0;
        isShowing = false;
    }
    
    public void SetOffset(Vector3 newOffset)
    {
        offset = newOffset;
        if (worldCanvas != null)
        {
            worldCanvas.transform.localPosition = offset;
        }
    }
    
    public bool IsShowing()
    {
        return isShowing;
    }
}
