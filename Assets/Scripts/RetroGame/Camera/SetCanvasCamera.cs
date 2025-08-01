using UnityEngine;

public class SetCanvasCamera : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = RetroGameCamera.Instance.GetCamera(RetroCameraType.UI);
    }
}
