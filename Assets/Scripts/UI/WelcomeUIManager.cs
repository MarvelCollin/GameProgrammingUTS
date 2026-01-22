using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WelcomeUIManager : MonoBehaviour
{
    public static WelcomeUIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private Button startButton;
    [SerializeField] private Button tutorialButton;

    private Canvas mainCanvas;
    private GameObject welcomePanelInstance;
    private GameObject tutorialButtonInstance;
    private bool isGameStarted = false;

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

        CreateUI();
    }

    private void Start()
    {
        ShowWelcomeUI();
    }

    private void Update()
    {
        if (isGameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleWelcomeUI();
        }
    }

    private void CreateUI()
    {
        if (FindObjectOfType<EventSystem>() == null)
        {
            GameObject eventSystemObj = new GameObject("EventSystem");
            eventSystemObj.AddComponent<EventSystem>();
            eventSystemObj.AddComponent<StandaloneInputModule>();
        }

        GameObject canvasObj = new GameObject("MainUICanvas");
        canvasObj.transform.SetParent(transform);
        mainCanvas = canvasObj.AddComponent<Canvas>();
        mainCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        mainCanvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasObj.AddComponent<GraphicRaycaster>();

        CreateWelcomePanel();
        CreateTutorialButton();
    }

    private void CreateWelcomePanel()
    {
        welcomePanelInstance = new GameObject("WelcomePanel");
        welcomePanelInstance.transform.SetParent(mainCanvas.transform);

        RectTransform panelRect = welcomePanelInstance.AddComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelBg = welcomePanelInstance.AddComponent<Image>();
        panelBg.color = new Color(0, 0, 0, 0.85f);

        GameObject contentPanel = new GameObject("ContentPanel");
        contentPanel.transform.SetParent(welcomePanelInstance.transform);

        RectTransform contentRect = contentPanel.AddComponent<RectTransform>();
        contentRect.anchorMin = new Vector2(0.5f, 0.5f);
        contentRect.anchorMax = new Vector2(0.5f, 0.5f);
        contentRect.sizeDelta = new Vector2(700, 550);
        contentRect.anchoredPosition = Vector2.zero;

        Image contentBg = contentPanel.AddComponent<Image>();
        contentBg.color = new Color(0.2f, 0.15f, 0.1f, 1f);

        Outline outline = contentPanel.AddComponent<Outline>();
        outline.effectColor = new Color(0.4f, 0.3f, 0.2f, 1f);
        outline.effectDistance = new Vector2(3, -3);

        CreateTitle(contentPanel.transform);
        CreateStudentInfo(contentPanel.transform);
        CreateControlsInfo(contentPanel.transform);
        CreateStartButton(contentPanel.transform);
    }

    private void CreateTitle(Transform parent)
    {
        GameObject titleObj = new GameObject("Title");
        titleObj.transform.SetParent(parent);

        RectTransform titleRect = titleObj.AddComponent<RectTransform>();
        titleRect.anchorMin = new Vector2(0.5f, 1f);
        titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.sizeDelta = new Vector2(600, 60);
        titleRect.anchoredPosition = new Vector2(0, -40);

        Text titleText = titleObj.AddComponent<Text>();
        titleText.text = "WELCOME TO GARDEN GAME";
        titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        titleText.fontSize = 36;
        titleText.fontStyle = FontStyle.Bold;
        titleText.alignment = TextAnchor.MiddleCenter;
        titleText.color = new Color(1f, 0.9f, 0.7f);
        titleText.raycastTarget = false;

        Outline titleOutline = titleObj.AddComponent<Outline>();
        titleOutline.effectColor = Color.black;
        titleOutline.effectDistance = new Vector2(2, -2);
    }

    private void CreateStudentInfo(Transform parent)
    {
        GameObject infoObj = new GameObject("StudentInfo");
        infoObj.transform.SetParent(parent);

        RectTransform infoRect = infoObj.AddComponent<RectTransform>();
        infoRect.anchorMin = new Vector2(0.5f, 1f);
        infoRect.anchorMax = new Vector2(0.5f, 1f);
        infoRect.sizeDelta = new Vector2(600, 50);
        infoRect.anchoredPosition = new Vector2(0, -100);

        Text infoText = infoObj.AddComponent<Text>();
        infoText.text = "Made by: Student Name\nStudent ID: 2702280352";
        infoText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        infoText.fontSize = 20;
        infoText.alignment = TextAnchor.MiddleCenter;
        infoText.color = new Color(0.9f, 0.85f, 0.7f);
        infoText.raycastTarget = false;
    }

    private void CreateControlsInfo(Transform parent)
    {
        GameObject controlsObj = new GameObject("ControlsInfo");
        controlsObj.transform.SetParent(parent);

        RectTransform controlsRect = controlsObj.AddComponent<RectTransform>();
        controlsRect.anchorMin = new Vector2(0.5f, 0.5f);
        controlsRect.anchorMax = new Vector2(0.5f, 0.5f);
        controlsRect.sizeDelta = new Vector2(600, 280);
        controlsRect.anchoredPosition = new Vector2(0, 20);

        Text controlsText = controlsObj.AddComponent<Text>();
        controlsText.text = "=== CONTROLS ===\n\n" +
                           "WASD / Arrow Keys - Move Player\n\n" +
                           "Left Click / Z - Attack\n" +
                           "   > Damages skeletons (3 hits to kill)\n" +
                           "   > Shows heartbreak on animals/goblins\n\n" +
                           "Right Click / X - Dig\n" +
                           "   > Dig up planted crops to harvest\n\n" +
                           "ESC / ? Button - Show this menu\n\n" +
                           "=== OBJECTIVE ===\n" +
                           "Collect crops, interact with animals,\n" +
                           "and defeat skeletons!";
        controlsText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        controlsText.fontSize = 18;
        controlsText.alignment = TextAnchor.MiddleCenter;
        controlsText.color = Color.white;
        controlsText.lineSpacing = 1.1f;
        controlsText.raycastTarget = false;
    }

    private void CreateStartButton(Transform parent)
    {
        GameObject buttonObj = new GameObject("StartButton");
        buttonObj.transform.SetParent(parent);

        RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0f);
        buttonRect.anchorMax = new Vector2(0.5f, 0f);
        buttonRect.sizeDelta = new Vector2(200, 50);
        buttonRect.anchoredPosition = new Vector2(0, 50);

        Image buttonImage = buttonObj.AddComponent<Image>();
        buttonImage.color = new Color(0.3f, 0.6f, 0.3f);

        Button button = buttonObj.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.3f, 0.6f, 0.3f);
        colors.highlightedColor = new Color(0.4f, 0.7f, 0.4f);
        colors.pressedColor = new Color(0.2f, 0.5f, 0.2f);
        button.colors = colors;
        button.onClick.AddListener(OnStartButtonClicked);

        GameObject buttonTextObj = new GameObject("Text");
        buttonTextObj.transform.SetParent(buttonObj.transform);

        RectTransform textRect = buttonTextObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text buttonText = buttonTextObj.AddComponent<Text>();
        buttonText.text = "START GAME";
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 24;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        buttonText.raycastTarget = false;
    }

    private void CreateTutorialButton()
    {
        tutorialButtonInstance = new GameObject("TutorialButton");
        tutorialButtonInstance.transform.SetParent(mainCanvas.transform);

        RectTransform buttonRect = tutorialButtonInstance.AddComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0f, 1f);
        buttonRect.anchorMax = new Vector2(0f, 1f);
        buttonRect.sizeDelta = new Vector2(50, 50);
        buttonRect.anchoredPosition = new Vector2(35, -35);

        Image buttonImage = tutorialButtonInstance.AddComponent<Image>();
        buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

        Button button = tutorialButtonInstance.AddComponent<Button>();
        ColorBlock colors = button.colors;
        colors.normalColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        colors.highlightedColor = new Color(0.3f, 0.3f, 0.3f, 0.9f);
        colors.pressedColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        button.colors = colors;
        button.onClick.AddListener(OnTutorialButtonClicked);

        GameObject buttonTextObj = new GameObject("Text");
        buttonTextObj.transform.SetParent(tutorialButtonInstance.transform);

        RectTransform textRect = buttonTextObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        Text buttonText = buttonTextObj.AddComponent<Text>();
        buttonText.text = "?";
        buttonText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        buttonText.fontSize = 32;
        buttonText.fontStyle = FontStyle.Bold;
        buttonText.alignment = TextAnchor.MiddleCenter;
        buttonText.color = Color.white;
        buttonText.raycastTarget = false;

        tutorialButtonInstance.SetActive(false);
    }

    private void OnStartButtonClicked()
    {
        HideWelcomeUI();
        isGameStarted = true;
        Time.timeScale = 1f;
    }

    private void OnTutorialButtonClicked()
    {
        ToggleWelcomeUI();
    }

    public void ShowWelcomeUI()
    {
        welcomePanelInstance.SetActive(true);
        tutorialButtonInstance.SetActive(false);
        Time.timeScale = 0f;
    }

    public void HideWelcomeUI()
    {
        welcomePanelInstance.SetActive(false);
        tutorialButtonInstance.SetActive(true);
        Time.timeScale = 1f;
    }

    public void ToggleWelcomeUI()
    {
        if (welcomePanelInstance.activeSelf)
        {
            HideWelcomeUI();
        }
        else
        {
            ShowWelcomeUI();
        }
    }

    public bool IsWelcomeUIActive()
    {
        return welcomePanelInstance != null && welcomePanelInstance.activeSelf;
    }
}
