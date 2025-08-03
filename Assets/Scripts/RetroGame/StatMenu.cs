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

        if (isOn)
        {
            if (PlayerState.Instance.CurrentHP > 0)
                playerHPText.text = PlayerState.Instance.CurrentHP.ToString("00");
            else
                playerHPText.text = "XX";
            playerStrText.text = PlayerState.Instance.Strength.ToString("00");
        }

        opponentStats.SetActive(isInDuel);

        if (isInDuel)
        {
            int hp = DuelManager.Instance.currentOpponent.CurrentHP;
            if (hp > 0)
                opponentHPText.text = hp.ToString("00");
            else
                opponentHPText.text = "XX";
            opponentStrText.text = DuelManager.Instance.currentOpponent.Strength.ToString("00");
        }
    }
}
