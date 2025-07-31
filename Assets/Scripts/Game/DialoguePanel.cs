using System.Collections;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] float characterTime;
    TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayLine(string line)
    {

    }

    IEnumerator Typewritting()
    {

    }
}
