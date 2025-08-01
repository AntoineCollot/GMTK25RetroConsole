using UnityEngine;
using UnityEngine.Rendering.UI;

public enum RetroCameraType { Game, UI}
public class RetroGameCamera : MonoBehaviour
{
    [Header("Follow")]
    static readonly Vector3 OFFSET = new Vector3(0, 0, -1);
    float aimAtDuel01;
    const float AIM_AT_DUEL_SMOOTHTIME = 0.5f;
    Vector2 duelCenterPos;

    [Header("Cameras")]
    public Camera UICamera;
    public Camera gameCamera { get; private set; }

    public static RetroGameCamera Instance;

    private void Awake()
    {
        Instance = this;

        gameCamera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        if (PlayerState.Instance == null)
            return;

        Vector2 targetPos = PlayerState.Instance.transform.position;

        //Aim at average point between duelists
        if (DuelManager.Instance.isInDuel)
        {
            duelCenterPos = targetPos + DuelManager.Instance.currentOpponent.Position;
            duelCenterPos *= 0.5f;
            aimAtDuel01 += Time.deltaTime / AIM_AT_DUEL_SMOOTHTIME;
        }
        else
        {
            aimAtDuel01 -= Time.deltaTime / AIM_AT_DUEL_SMOOTHTIME;
        }

        aimAtDuel01 = Mathf.Clamp01(aimAtDuel01);
        targetPos = Curves.QuadEaseInOut(targetPos, duelCenterPos, aimAtDuel01);

        transform.position = new Vector3(targetPos.x, targetPos.y, 0) + OFFSET;
    }

    public Camera GetCamera(RetroCameraType type)
    {
        switch (type)
        {
            case RetroCameraType.Game:
            default:
                return gameCamera;
            case RetroCameraType.UI:
                return UICamera;
        }
    }
}
