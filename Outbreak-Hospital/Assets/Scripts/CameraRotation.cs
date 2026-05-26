using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Transform player;
    [SerializeField] private float sensitivity = 1f;
    [SerializeField] private float topClamp = 70f;
    [SerializeField] private float bottomClamp = -30f;
    [SerializeField]
    private float aimSensitivityMultiplier = 0.4f;
    [SerializeField] private ThirdPersonShooterController playerAim;

    private float _yaw;
    private float _pitch;

    private void LateUpdate()
    {
        float currentSensitivity = sensitivity;
        if (playerAim.aim) currentSensitivity *= aimSensitivityMultiplier;

        float mouseX = Input.GetAxis("Mouse X") * currentSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * currentSensitivity;

        _yaw += mouseX;
        _pitch -= mouseY;
        _pitch = Mathf.Clamp(_pitch, bottomClamp, topClamp);




        cameraTarget.rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        PlayerRotation();
    }

    private void PlayerRotation()
    {
        player.rotation = Quaternion.Euler(0f, _yaw, 0f);
        cameraTarget.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
    }
}