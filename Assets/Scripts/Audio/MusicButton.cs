using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour
{
    [SerializeField] Sprite onSprite;
    [SerializeField] Sprite offSprite;
    Image icon;
    [SerializeField] bool isOn;

    // Start is called before the first frame update
    void Start()
    {
		icon = GetComponent<Image>();
        GetComponent<Button>().onClick.AddListener(OnClick);
        SetValue(isOn);
    }

    void OnClick()
    {
        SetValue(!isOn);
    }

    void SetValue(bool value)
    {
        isOn = value;
        MusicManager.Instance.Mute(!isOn);

        if (isOn)
            icon.sprite = onSprite;
        else
            icon.sprite = offSprite;
    }
}
