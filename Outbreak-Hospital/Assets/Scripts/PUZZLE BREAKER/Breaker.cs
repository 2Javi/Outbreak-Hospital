using System.Collections;
using UnityEngine;

public class Breaker : MonoBehaviour
{
    public enum BreakerID { B201, B202, B203, B204, B205, B206, B207, B208 }
    
    [Header("Variables")]
    public BreakerID breakerID;
    public bool isUp;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float upAngle = 19f;
    [SerializeField] private float downAngle = -19f;
    [SerializeField] private bool isFlipping;

    void OnMouseDown()
    {
        isUp = !isUp;
        StartCoroutine(FlipBreaker());
    }

    IEnumerator FlipBreaker()
    {
        
        isFlipping = true;
        Quaternion startRot = transform.localRotation;
        Quaternion targetRot = Quaternion.Euler(isUp ? upAngle : downAngle,0,0);
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            transform.localRotation = Quaternion.Lerp(startRot,targetRot,t);
            yield return null;
        }
       transform.localRotation = targetRot;
    }
}
