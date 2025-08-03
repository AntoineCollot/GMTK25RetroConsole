using System.Linq;
using UnityEngine;

public class EnableForCode : MonoBehaviour
{
    [SerializeField] int[] codes;

    private void Start()
    {
        OnCodeLoaded(RetroGameManager.loadedCode);
    }

    public void OnCodeLoaded(int loadedCode)
    {
        gameObject.SetActive(codes.Contains(loadedCode));
    }
}
