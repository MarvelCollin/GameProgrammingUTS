using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIManager : MonoBehaviour
{
    public static GameUIManager Instance { get; private set; }
    
    [Header("UI References")]
    [SerializeField] private Text messageText;
    [SerializeField] private float messageDuration = 2f;
    [SerializeField] private float fadeSpeed = 2f;
    
    private Coroutine currentMessageCoroutine;
    private CanvasGroup canvasGroup;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        if (messageText != null)
        {
            canvasGroup = messageText.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = messageText.gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = 0f;
        }
    }
    
    public void ShowMessage(string message)
    {
        if (messageText == null) return;
        
        if (currentMessageCoroutine != null)
        {
            StopCoroutine(currentMessageCoroutine);
        }
        
        currentMessageCoroutine = StartCoroutine(DisplayMessage(message));
    }
    
    private IEnumerator DisplayMessage(string message)
    {
        messageText.text = message;
        
        canvasGroup.alpha = 1f;
        
        yield return new WaitForSeconds(messageDuration);
        
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        
        canvasGroup.alpha = 0f;
        messageText.text = "";
    }
}
