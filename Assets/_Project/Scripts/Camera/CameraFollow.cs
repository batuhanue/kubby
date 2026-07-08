using UnityEngine;

/// <summary>
/// Izometrik takip kamerasi. Karakteri yumusakca izler ve sabit izometrik acida kalir.
/// Portrait (dikey) ekranda yatay gorus alanini sabit tutar; boylece telefon
/// oraninda bile ayni genislikte oynanis alani gorunur.
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public Vector3 offset = new Vector3(8f, 8f, -8f);
    public float smoothSpeed = 6f;

    [Header("Isometric Angle")]
    public Vector3 isometricEuler = new Vector3(35f, -45f, 0f);

    [Header("Portrait Framing")]
    [Tooltip("Ekranda gorunmesini istedigin yatay genisligin yarisi (dunya birimi). " +
             "Portrait'te ortografik boyut buna gore ayarlanir.")]
    public float halfHorizontalView = 5f;

    [Tooltip("Acikken orthographicSize'i her karede en/boy oranina gore ayarlar.")]
    public bool maintainHorizontalView = true;

    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (cam != null && cam.orthographic && maintainHorizontalView && cam.aspect > 0.01f)
        {
            // orthographicSize = yari dikey yukseklik. Yatay yariyi sabit tutmak icin:
            // yariGenislik = orthoSize * aspect  =>  orthoSize = yariGenislik / aspect
            cam.orthographicSize = halfHorizontalView / cam.aspect;
        }

        if (target == null)
            return;

        Vector3 desiredPosition = target.position + offset;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(isometricEuler);
    }
}
