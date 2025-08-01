using TMPro;
using UnityEngine;

public class StatMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject panel;
    [SerializeField] GameObject opponentStats;
    [SerializeField] StartMenu startMenu;
    bool isOn;

    [Header("Stats")]
    [SerializeField] TextMeshProUGUI playerHPText;
    [SerializeField] TextMeshProUGUI playerStrText;
    [SerializeField] TextMeshProUGUI opponentHPText;
    [SerializeField] TextMeshProUGUI opponentStrText;

    void Start()
    {
    }

    void LateUpdate()
    {
        bool isInDuel = DuelManager.Instance.isInDuel;
        isOn = startMenu.isOn || isInDuel;
        panel.SetActive(isOn);

        if(isOn)
        {
            playerHPText.text = PlayerState.Instance.CurrentHP.ToString("00");
            playerStrText.text = PlayerState.Instance.Strength.ToString("00");
        }

        opponentStats.SetActive(isInDuel);

        if(isInDuel)
        {
            opponentHPText.text = DuelManager.Instance.currentOpponent.CurrentHP.ToString("00");
            opponentStrText.text = DuelManager.Instance.currentOpponent.Strength.ToString("00");
        }
    }
}
