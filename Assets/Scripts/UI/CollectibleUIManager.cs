using UnityEngine;
using UnityEngine.UI;

public class CollectibleUIManager : MonoBehaviour
{
    public static CollectibleUIManager Instance { get; private set; }

    private Canvas uiCanvas;
    private GameObject collectiblePanel;
    private Text carrotText;
    private Text potatoText;
    private Text wheatText;
    private Text pumpkinText;
    private Text cabbageText;
    private Text beetrootText;
    private Text totalText;

    private bool isSubscribed = false;

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
    }

    private void Start()
    {
        CreateUI();
        TrySubscribeToCropDataManager();
    }

    private void Update()
    {
        if (!isSubscribed)
        {
            TrySubscribeToCropDataManager();
        }
    }

    private void TrySubscribeToCropDataManager()
    {
        if (isSubscribed) return;
        
        if (CropDataManager.Instance != null)
        {
            CropDataManager.Instance.OnCropDataChanged += UpdateUI;
            UpdateUI();
            isSubscribed = true;
        }
    }

    private void OnDestroy()
    {
        if (isSubscribed && CropDataManager.Instance != null)
        {
            CropDataManager.Instance.OnCropDataChanged -= UpdateUI;
        }
    }

    private void CreateUI()
    {
        GameObject canvasObj = new GameObject("CollectibleUICanvas");
        canvasObj.transform.SetParent(transform);
        uiCanvas = canvasObj.AddComponent<Canvas>();
        uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        uiCanvas.sortingOrder = 50;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        collectiblePanel = new GameObject("CollectiblePanel");
        collectiblePanel.transform.SetParent(uiCanvas.transform);

        RectTransform panelRect = collectiblePanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0f, 1f);
        panelRect.anchorMax = new Vector2(0f, 1f);
        panelRect.pivot = new Vector2(0f, 1f);
        panelRect.sizeDelta = new Vector2(220, 280);
        panelRect.anchoredPosition = new Vector2(10, -70);

        Image panelBg = collectiblePanel.AddComponent<Image>();
        panelBg.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);

        CreateTitle();
        CreateCropTexts();
    }

    private void CreateTitle()
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(collectiblePanel.transform);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.sizeDelta = new Vector2(200, 30);
        titleRect.anchoredPosition = new Vector2(0, -20);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "CROPS COLLECTED";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 18;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 0.9f, 0.6f);

        Outline outline = titleObj.AddComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(1, -1);
    }

    private void CreateCropTexts()
    {
        float startY = -50;
        float spacing = 30;

        carrotText = CreateCropText("CarrotText", "Carrot: 0", startY, new Color(1f, 0.5f, 0.2f));
        potatoText = CreateCropText("PotatoText", "Potato: 0", startY - spacing, new Color(0.9f, 0.8f, 0.5f));
        wheatText = CreateCropText("WheatText", "Wheat: 0", startY - spacing * 2, new Color(1f, 0.9f, 0.4f));
        pumpkinText = CreateCropText("PumpkinText", "Pumpkin: 0", startY - spacing * 3, new Color(1f, 0.6f, 0.2f));
        cabbageText = CreateCropText("CabbageText", "Cabbage: 0", startY - spacing * 4, new Color(0.5f, 0.9f, 0.5f));
        beetrootText = CreateCropText("BeetrootText", "Beetroot: 0", startY - spacing * 5, new Color(0.8f, 0.2f, 0.4f));

        GameObject separatorObj = new GameObject("Separator");
        separatorObj.transform.SetParent(collectiblePanel.transform);

        RectTransform sepRect = separatorObj.AddComponent<RectTransform>();
        sepRect.anchorMin = new Vector2(0.5f, 1f);
        sepRect.anchorMax = new Vector2(0.5f, 1f);
        sepRect.sizeDelta = new Vector2(180, 2);
        sepRect.anchoredPosition = new Vector2(0, startY - spacing * 6 + 5);

        Image sepImage = separatorObj.AddComponent<Image>();
        sepImage.color = new Color(1f, 1f, 1f, 0.5f);

        totalText = CreateCropText("TotalText", "TOTAL: 0", startY - spacing * 6 - 15, Color.white);
        totalText.fontStyle = FontStyle.Bold;
        totalText.fontSize = 20;
    }

    private Text CreateCropText(string name, string defaultText, float yPos, Color color)
    {
        GameObject textObj = new GameObject(name);
        textObj.transform.SetParent(collectiblePanel.transform);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0f, 1f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.sizeDelta = new Vector2(-20, 25);
        textRect.anchoredPosition = new Vector2(0, yPos);

        Text text = textObj.AddComponent<Text>();
        text.text = defaultText;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = 16;
        text.alignment = TextAnchor.MiddleLeft;
        text.color = color;

        return text;
    }

    public void UpdateUI()
    {
        if (CropDataManager.Instance == null) return;

        CropSaveData data = CropDataManager.Instance.GetCropData();

        if (carrotText != null) carrotText.text = $"  Carrot: {data.carrotCount}";
        if (potatoText != null) potatoText.text = $"  Potato: {data.potatoCount}";
        if (wheatText != null) wheatText.text = $"  Wheat: {data.wheatCount}";
        if (pumpkinText != null) pumpkinText.text = $"  Pumpkin: {data.pumpkinCount}";
        if (cabbageText != null) cabbageText.text = $"  Cabbage: {data.cabbageCount}";
        if (beetrootText != null) beetrootText.text = $"  Beetroot: {data.beetrootCount}";
        if (totalText != null) totalText.text = $"  TOTAL: {data.GetTotal()}";
    }

    public void AddCrop(CropType cropType)
    {
        if (CropDataManager.Instance != null)
        {
            CropDataManager.Instance.AddCrop(cropType);
        }
    }
}
