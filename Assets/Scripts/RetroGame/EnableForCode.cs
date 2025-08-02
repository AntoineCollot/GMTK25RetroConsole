using UnityEngine;

public class EnableForCode : MonoBehaviour
{
    [SerializeField] int code;

    public void OnCodeLoaded(int loadedCode)
    {
        gameObject.SetActive(code == loadedCode);
    }
}
