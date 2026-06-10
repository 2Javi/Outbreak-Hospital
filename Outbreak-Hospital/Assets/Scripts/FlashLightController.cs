using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[RequireComponent(typeof(Animator))]
public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight State")]
    public bool isEquipped = false;

    [Header("References")]
    [SerializeField] private Light flashlightLight;
    [SerializeField] private Animator animator;
    [SerializeField] private Rig flashlightRig;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private Camera playerCamera;

    [Header("Animator")]
    [SerializeField] private string flashlightLayerName = "Flashlight";
    private int flashlightLayerIndex;

    [Header("Battery")]
    [SerializeField] private float maxBattery = 100f;
    [SerializeField] private float drainRate = 2f;
    private float currentBattery;

    [Header("Light Settings")]
    [SerializeField] private float lightRange = 15f;
    [SerializeField] private float spotAngle = 35f;
    [SerializeField] private float lightIntensity = 2f;
    [SerializeField] private float aimTargetDistance = 10f;
    [SerializeField] private bool wasEquipped = false;


    [Header("Flicker")]
    [SerializeField] private float flickerThreshold = 0.1f;
    [SerializeField] private float flickerSpeed = 0.05f;
    private float flickerTimer = 0f;

    void Start()
    {
        currentBattery = maxBattery;
        flashlightLayerIndex = animator.GetLayerIndex(flashlightLayerName);

        if (flashlightLight != null)
        {
            flashlightLight.range = lightRange;
            flashlightLight.spotAngle = spotAngle;
            flashlightLight.intensity = lightIntensity;
            flashlightLight.enabled = false;
        }

        SetFlashlightActive(false);
    }

    void Update()
    {

    if (isEquipped != wasEquipped)
    {
        SetFlashlightActive(isEquipped);
        wasEquipped = isEquipped;
    }

    if (isEquipped)
    {
        HandleBatteryDrain();
        UpdateAimTarget();
    }

    }

    private void SetFlashlightActive(bool active)
    {

        if (flashlightLight != null)
            flashlightLight.enabled = active && currentBattery > 0f;

        if (animator != null && flashlightLayerIndex >= 0)
            animator.SetLayerWeight(flashlightLayerIndex, active ? 1f : 0f);

        if (flashlightRig != null)
            flashlightRig.weight = active ? 1f : 0f;
    }

    public void Equip()
    {
        isEquipped = true;
        SetFlashlightActive(true);
    }

    public void Unequip()
    {
        isEquipped = false;
        SetFlashlightActive(false);
    }

    private void HandleBatteryDrain()
    {
        if (currentBattery <= 0f)
        {
            SetFlashlightActive(false);
            return;
        }

        currentBattery -= drainRate * Time.deltaTime;
        currentBattery = Mathf.Clamp(currentBattery, 0f, maxBattery);

        if (flashlightLight != null)
        {
            float batteryPercent = currentBattery / maxBattery;

            if (batteryPercent <= flickerThreshold)
            {
                flickerTimer -= Time.deltaTime;
                if (flickerTimer <= 0f)
                {
                    flashlightLight.intensity = lightIntensity * Random.Range(0.0f, 0.4f);
                    flickerTimer = flickerSpeed * Random.Range(0.5f, 1.5f);
                }
            }
            else
            {
                flashlightLight.intensity = lightIntensity * Mathf.Clamp(batteryPercent, 0.1f, 1f);
            }
        }
    }

    public void AddBattery(float amount)
    {
        currentBattery = Mathf.Clamp(currentBattery + amount, 0f, maxBattery);
        SetFlashlightActive(isEquipped);
    }

    public float GetBatteryPercent()
    {
        return currentBattery / maxBattery;
    }

    private void UpdateAimTarget()
    {
        if (aimTarget == null || playerCamera == null) return;

        aimTarget.position = playerCamera.transform.position
                           + playerCamera.transform.forward * aimTargetDistance;
    }
}