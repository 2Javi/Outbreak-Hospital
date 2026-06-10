using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
public class Puzzles : MonoBehaviour
{

[Header("References")]
[SerializeField] GameObject player;
[SerializeField] private InputActionReference interactAction;
[SerializeField] private CinemachineCamera puzzleCamera;
private SkinnedMeshRenderer[] playerMeshes;


[Header("Variables")]
[SerializeField] float distance;
[SerializeField] float interactRange = 3f;
private PlayerInput playerInputRef;
public bool isPuzzleOpen = false;
    void Awake()
    {
        playerInputRef = player.GetComponent<PlayerInput>();
        playerMeshes = player.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        distance = Vector3.Distance(player.transform.position,transform.position);

        if (distance < interactRange)
        {
            if (!isPuzzleOpen && interactAction.action.WasPressedThisFrame())
            {
                isPuzzleOpen = true;
                puzzleCamera.gameObject.SetActive(true);
                playerInputRef.DeactivateInput();
                player.GetComponent<MovementStateManager>().enabled = false;
                SkinnedMeshRenderer[] meshes = player.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var mesh in meshes) mesh.enabled = false;
            } 
            else if (isPuzzleOpen && Keyboard.current.eKey.wasPressedThisFrame)
            {
                isPuzzleOpen = false;
                puzzleCamera.gameObject.SetActive(false);
                playerInputRef.ActivateInput();
                player.GetComponent<MovementStateManager>().enabled = true;
                SkinnedMeshRenderer[] meshes = player.GetComponentsInChildren<SkinnedMeshRenderer>();
                foreach (var mesh in meshes) mesh.enabled = true;
            }
        }
    }

    private void OnEnable()
    {
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.Disable();
    }
}
