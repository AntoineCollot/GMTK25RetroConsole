using UnityEngine;

[DefaultExecutionOrder(100)] //After retro game cam happening in late update
public class FollowRetroCamera : MonoBehaviour
{
    void Start()
    {
    }

    void LateUpdate()
    {
        transform.position = RetroGameCamera.Instance.transform.position+Vector3.forward;
    }
}
