using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    static readonly Vector3 OFFSET = new Vector3(0, 0, -1);
    float aimAtDuel01;
    const float AIM_AT_DUEL_SMOOTHTIME = 0.5f;
    Vector2 duelCenterPos;

    void LateUpdate()
    {
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

        transform.position = new Vector3(targetPos.x, targetPos.y,0) + OFFSET;
    }
}
