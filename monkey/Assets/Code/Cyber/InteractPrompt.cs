using UnityEngine;
using TMPro;

public class InteractPrompt : MonoBehaviour
{
    public TMP_Text promptText;         // Drag your InteractPrompt here
    public float detectRange = 3f;      // How close to detect a door
    public LayerMask doorLayer;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        promptText.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckForDoor();
    }

    void CheckForDoor()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectRange, doorLayer))
        {
            promptText.gameObject.SetActive(true);
        }
        else
        {
            promptText.gameObject.SetActive(false);
        }
    }
}
