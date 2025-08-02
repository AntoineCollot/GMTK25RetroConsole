using UnityEngine;

[DefaultExecutionOrder(50)]
public class UpdateShaderCameraPos : MonoBehaviour
{
    const string VAR_NAME = "_CameraWPos";

    void LateUpdate()
    {
        Shader.SetGlobalVector(VAR_NAME,transform.position);
    }
}
