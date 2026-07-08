using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Ekran uzerinde parmak surukleyerek yon veren sanal joystick (uGUI).
/// Deger her karede KupikInput.VirtualMove'a yazilir.
/// StandaloneInputModule (Old Input) ile calisir; Input System paketi gerekmez.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    [Tooltip("Joystick'in sabit arka plani (parmagin hareket alani).")]
    public RectTransform background;

    [Tooltip("Ortada duran, parmakla hareket eden tutamak.")]
    public RectTransform handle;

    [Header("Settings")]
    [Tooltip("Tutamagin merkezden kac piksel uzaga gidebilecegi.")]
    public float handleRange = 60f;

    [Tooltip("Bu esigin altindaki kucuk itmeleri yok say (olu bolge).")]
    [Range(0f, 0.4f)]
    public float deadZone = 0.1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (background == null)
            return;

        Vector2 localPoint;
        bool ok = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        if (!ok)
            return;

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, handleRange);

        if (handle != null)
            handle.anchoredPosition = clamped;

        Vector2 normalized = clamped / handleRange;

        if (normalized.magnitude < deadZone)
            normalized = Vector2.zero;

        KupikInput.VirtualMove = normalized;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (handle != null)
            handle.anchoredPosition = Vector2.zero;

        KupikInput.VirtualMove = Vector2.zero;
    }
}
