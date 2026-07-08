using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Tek tikla mobil dokunmatik arayuzu kurar: Canvas + EventSystem + sanal joystick
/// (sol alt) + zipla butonu (sag alt). Bilesenler otomatik baglanir.
/// Menu: Kupik > Setup > Create Mobile Controls (Portrait)
/// </summary>
public static class KupikMobileUISetup
{
    private const string CanvasName = "Kupik_MobileUI";

    [MenuItem("Kupik/Setup/Create Mobile Controls (Portrait)")]
    public static void CreateMobileControls()
    {
        // Ayni UI zaten varsa tekrar olusturma.
        GameObject existing = GameObject.Find(CanvasName);
        if (existing != null)
        {
            Selection.activeGameObject = existing;
            EditorUtility.DisplayDialog(
                "Kupik",
                "Mobil UI zaten sahnede mevcut (" + CanvasName + ").",
                "Tamam"
            );
            return;
        }

        EnsureEventSystem();

        GameObject canvasGO = CreateCanvas();
        CreateJoystick(canvasGO.transform);
        CreateJumpButton(canvasGO.transform);

        Undo.RegisterCreatedObjectUndo(canvasGO, "Create Kupik Mobile UI");
        Selection.activeGameObject = canvasGO;

        Debug.Log("Kupik mobil kontroller olusturuldu. Portrait (1080x1920) icin ayarlandi.");
    }

    private static void EnsureEventSystem()
    {
        if (Object.FindObjectOfType<EventSystem>() != null)
            return;

        GameObject es = new GameObject("EventSystem",
            typeof(EventSystem),
            typeof(StandaloneInputModule));

        Undo.RegisterCreatedObjectUndo(es, "Create EventSystem");
    }

    private static GameObject CreateCanvas()
    {
        GameObject canvasGO = new GameObject(CanvasName,
            typeof(Canvas),
            typeof(CanvasScaler),
            typeof(GraphicRaycaster));

        Canvas canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1080f, 1920f); // portrait
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        return canvasGO;
    }

    private static void CreateJoystick(Transform parent)
    {
        // Arka plan (parmagin hareket alani) - sol alt kose.
        GameObject bg = NewImage("Joystick_Background", parent,
            GetSprite("UI/Skin/Background.psd"),
            new Color(1f, 1f, 1f, 0.35f));

        RectTransform bgRect = bg.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.zero;
        bgRect.pivot = new Vector2(0.5f, 0.5f);
        bgRect.sizeDelta = new Vector2(300f, 300f);
        bgRect.anchoredPosition = new Vector2(280f, 320f);

        // Tutamak (parmakla hareket eden).
        GameObject handle = NewImage("Joystick_Handle", bg.transform,
            GetSprite("UI/Skin/Knob.psd"),
            new Color(1f, 1f, 1f, 0.85f));

        RectTransform handleRect = handle.GetComponent<RectTransform>();
        handleRect.anchorMin = new Vector2(0.5f, 0.5f);
        handleRect.anchorMax = new Vector2(0.5f, 0.5f);
        handleRect.pivot = new Vector2(0.5f, 0.5f);
        handleRect.sizeDelta = new Vector2(130f, 130f);
        handleRect.anchoredPosition = Vector2.zero;

        VirtualJoystick joystick = bg.AddComponent<VirtualJoystick>();
        joystick.background = bgRect;
        joystick.handle = handleRect;
        joystick.handleRange = 110f;
        joystick.deadZone = 0.1f;
    }

    private static void CreateJumpButton(Transform parent)
    {
        GameObject btn = NewImage("JumpButton", parent,
            GetSprite("UI/Skin/Knob.psd"),
            new Color(0.35f, 0.75f, 1f, 0.85f));

        RectTransform rect = btn.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(1f, 0f);
        rect.anchorMax = new Vector2(1f, 0f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = new Vector2(240f, 240f);
        rect.anchoredPosition = new Vector2(-260f, 340f);

        TouchButton touch = btn.AddComponent<TouchButton>();
        touch.action = TouchButton.ButtonAction.Jump;

        // Buton uzerine "JUMP" yazisi.
        GameObject labelGO = new GameObject("Label", typeof(RectTransform), typeof(Text));
        labelGO.transform.SetParent(btn.transform, false);

        RectTransform labelRect = labelGO.GetComponent<RectTransform>();
        labelRect.anchorMin = Vector2.zero;
        labelRect.anchorMax = Vector2.one;
        labelRect.offsetMin = Vector2.zero;
        labelRect.offsetMax = Vector2.zero;

        Text label = labelGO.GetComponent<Text>();
        label.text = "JUMP";
        label.alignment = TextAnchor.MiddleCenter;
        label.fontSize = 40;
        label.color = Color.white;
        label.raycastTarget = false;
        label.font = AssetDatabase.GetBuiltinExtraResource<Font>("Arial.ttf");
    }

    private static GameObject NewImage(string name, Transform parent, Sprite sprite, Color color)
    {
        GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
        go.transform.SetParent(parent, false);

        Image img = go.GetComponent<Image>();
        img.sprite = sprite;
        img.color = color;
        img.type = Image.Type.Simple;

        return go;
    }

    private static Sprite GetSprite(string builtinPath)
    {
        return AssetDatabase.GetBuiltinExtraResource<Sprite>(builtinPath);
    }
}
