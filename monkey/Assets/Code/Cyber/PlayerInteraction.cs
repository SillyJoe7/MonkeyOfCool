using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private LayerMask interactLayer; // Doors, monkeys, etc.

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            // 🔹 Score-based door
            DoorScore scoreDoor = hit.collider.GetComponentInParent<DoorScore>();
            if (scoreDoor != null)
            {
                scoreDoor.TryInteract();
                return;
            }

            // 🔹 Question-based door
            DoorInteraction questionDoor = hit.collider.GetComponentInParent<DoorInteraction>();
            if (questionDoor != null)
            {
                questionDoor.TryInteract();
                return;
            }

            // 🔹 Monkey password interaction
            MonkeyInteraction monkey = hit.collider.GetComponentInParent<MonkeyInteraction>();
            if (monkey != null)
            {
                monkey.TryInteract();
                return;
            }
        }
    }
}
