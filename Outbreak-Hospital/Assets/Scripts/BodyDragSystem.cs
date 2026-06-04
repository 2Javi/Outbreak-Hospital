using UnityEngine;

public class BodyDragSystem : MonoBehaviour
{
    [Header("Drag Settings")]
    public float dragForce = 100f;
    public float dragDistance = 1.5f;
    public float interactRange = 2f;
    public LayerMask enemyCorpseLayer;

    [Header("References")]
    public Transform dragAnchor;

    private HandStateManager handState;
    private Rigidbody currentBody;
    private bool isDragging = false;

    void Awake()
    {
        handState = GetComponent<HandStateManager>();
    }

    void Update()
    {
        HandleInteractInput();

        if (isDragging)
        {
            DragBody();
        }
    }

    void HandleInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isDragging)
            {
                StopDragging();
            }
            else
            {
                TryStartDragging();
            }
        }
    }

    void TryStartDragging()
    {
        if (!handState.bothHandsEmpty)
        {
            Debug.Log("Hands are not empty, cannot drag.");
            return;
        }

        Collider[] nearby = Physics.OverlapSphere(transform.position, interactRange, enemyCorpseLayer);

        if (nearby.Length == 0)
        {
            Debug.Log("No corpse in range.");
            return;
        }

        Rigidbody closestBody = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider col in nearby)
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestBody = col.GetComponent<Rigidbody>();
            }
        }

        if (closestBody != null)
        {
            StartDragging(closestBody);
        }
    }

    void StartDragging(Rigidbody body)
    {
        currentBody = body;
        isDragging = true;
        currentBody.isKinematic = false;
        currentBody.freezeRotation = true;

        // TRIGGER ANIMATION HERE

        Debug.Log("Started dragging: " + currentBody.name);
    }

    void DragBody()
    {
        if (currentBody == null)
        {
            StopDragging();
            return;
        }

        Vector3 targetPosition = dragAnchor.position;
        Vector3 direction = targetPosition - currentBody.position;

        currentBody.linearVelocity = Vector3.ClampMagnitude(direction * dragForce * Time.deltaTime * 60f, 5f);

        Vector3 vel = currentBody.linearVelocity;
        vel.y = 0f;
        currentBody.linearVelocity = vel;
    }

    void StopDragging()
    {
        if (currentBody != null)
        {
            currentBody.freezeRotation = false;
            currentBody.linearVelocity = Vector3.zero;
        }

        currentBody.isKinematic = true;
        currentBody = null;
        isDragging = false;

        // TRIGGER ANIMATION HERE
        Debug.Log("Stopped dragging.");
    }

    public bool IsDragging() => isDragging;
    public Rigidbody GetCurrentBody() => currentBody;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
