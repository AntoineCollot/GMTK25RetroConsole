using UnityEngine;

public class Cartridge : MonoBehaviour
{
    [SerializeField] RealConsole console;
    [SerializeField] float exitOffset;
    Vector3 localPosOrigin;

    [SerializeField] float smooth = 0.3f;
    float refSmooth;

    private void Start()
    {
        localPosOrigin = transform.localPosition;
    }

    void Update()
    {
        Vector3 targetPos = localPosOrigin;
        float y = targetPos.y;
        if (console.isCartridgeOut)
            y += exitOffset;

        targetPos.y = Mathf.SmoothDamp(transform.localPosition.y, y, ref refSmooth, smooth);
        transform.localPosition = targetPos;
    }
}
