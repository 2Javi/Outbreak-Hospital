using UnityEngine;

[CreateAssetMenu(fileName = "Mask", menuName = "Mask Item")]
public class MaskData : ItemData
{
[SerializeField] MaskTiers tier;
public float maxDurability;
}
