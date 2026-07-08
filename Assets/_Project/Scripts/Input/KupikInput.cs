using UnityEngine;

/// <summary>
/// Klavye (editor testi) ile ekran joystick/butonlarini (Android) tek noktada birlestirir.
/// PlayerController buradan okur; hangi girdi kaynaginin oldugunu bilmesi gerekmez.
///
/// Ekran joystick'i her karede VirtualMove'u gunceller, zipla butonu PressJump() cagirir.
/// </summary>
public static class KupikInput
{
    /// <summary>Ekran joystick'inden gelen yon (-1..1). Klavye ile toplanir.</summary>
    public static Vector2 VirtualMove;

    private static bool virtualJumpQueued;

    /// <summary>Zipla butonu (dokunmatik) buna basar; bir sonraki okumada tuketilir.</summary>
    public static void PressJump()
    {
        virtualJumpQueued = true;
    }

    /// <summary>Klavye + joystick birlesik hareket vektoru. Uzunlugu 1'i asmaz.</summary>
    public static Vector2 Move
    {
        get
        {
            Vector2 keyboard = new Vector2(
                Input.GetAxisRaw("Horizontal"),
                Input.GetAxisRaw("Vertical")
            );

            Vector2 combined = keyboard + VirtualMove;

            if (combined.sqrMagnitude > 1f)
            {
                combined = combined.normalized;
            }

            return combined;
        }
    }

    /// <summary>
    /// Bu karede zipla istegi var mi? (Space tusu VEYA ekran zipla butonu.)
    /// Update icinde bir kez cagrilmali; sanal zipla bayragini tuketir.
    /// </summary>
    public static bool JumpPressedThisFrame()
    {
        bool keyboardJump = Input.GetKeyDown(KeyCode.Space);

        if (virtualJumpQueued)
        {
            virtualJumpQueued = false;
            return true;
        }

        return keyboardJump;
    }

    // Editor'de Play'e her basildiginda statik alanlar sifirdan baslasin.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        VirtualMove = Vector2.zero;
        virtualJumpQueued = false;
    }
}
