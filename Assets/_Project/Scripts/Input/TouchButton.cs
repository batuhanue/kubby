using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Dokunmatik aksiyon butonu. Parmak degdigi anda (pointer down) tetiklenir;
/// Unity Button'un onClick'ine gore daha hizli/duyarlidir (pointer up beklemez).
/// </summary>
public class TouchButton : MonoBehaviour, IPointerDownHandler
{
    public enum ButtonAction
    {
        Jump
    }

    [Tooltip("Bu butonun hangi aksiyonu tetikleyecegi.")]
    public ButtonAction action = ButtonAction.Jump;

    public void OnPointerDown(PointerEventData eventData)
    {
        switch (action)
        {
            case ButtonAction.Jump:
                KupikInput.PressJump();
                break;
        }
    }
}
