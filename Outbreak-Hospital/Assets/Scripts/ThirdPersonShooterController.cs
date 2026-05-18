using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera aimVirtualCamera;
    public bool aim;

    private void Update()
    {
        if (aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
        }
    }

    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }
    public void AimInput(bool newAimState)
    {
        aim = newAimState;
    }

}


