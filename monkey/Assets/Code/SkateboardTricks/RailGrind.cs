using UnityEngine;

public class RailGrind : MonoBehaviour
{
    [Header("Rail Setup")]
    public Transform railStart;
    public Transform railEnd;

    [Header("Grind Settings")]
    public float grindSpeed = 5f;
    public float pointRate = 100f;
    public float snapDistance = 3f;

    [Header("UI")]
    public GameObject grindPromptUI; // Assign a UI text (e.g., "Press E to Grind")

    private Transform player;
    private Points_SCR pointsSystem;
    private float progress = 0f;
    private int direction = 1;
    private bool isGrinding = false;
    private Vector3 lastGrindDirection;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player != null)
            pointsSystem = player.GetComponent<Points_SCR>();
    

    }
    public void SetPromptActive(bool state)
    {
        if (grindPromptUI != null)
            grindPromptUI.SetActive(state);
    }

    void Update()
    {
        if (player == null) return;

        float distToStart = Vector3.Distance(player.position, railStart.position);
        float distToEnd = Vector3.Distance(player.position, railEnd.position);
        bool inRange = distToStart <= snapDistance || distToEnd <= snapDistance;

        Debug.Log(distToStart + " " + distToEnd + " " + snapDistance + " " + inRange);

        // Show/hide grind prompt for this rail
        if (grindPromptUI != null)
           // grindPromptUI.SetActive(inRange || !isGrinding);

        // Start grinding when pressing 'E' near the rail
        if (inRange && Input.GetKeyDown(KeyCode.E) && !isGrinding)
        {
            if (distToStart < distToEnd)
            {
                StartGrindingAtStart(); // Start from the start of the rail
            }
            else
            {
                StartGrindingAtEnd(); // Start from the end of the rail
            }
        }

        // Grind along the rail
        if (isGrinding)
        {
            GrindAlongRail();

            // Jump off manually
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StopGrinding(jumpOff: true);
            }
        }
    }

    Vector3 ClosestPointOnRail(Vector3 position)
    {
        Vector3 railDir = (railEnd.position - railStart.position).normalized;
        Vector3 toStart = position - railStart.position;
        float projLength = Mathf.Clamp(Vector3.Dot(toStart, railDir), 0f, Vector3.Distance(railStart.position, railEnd.position));
        return railStart.position + railDir * projLength;
    }

    void StartGrindingAtStart()
    {
        isGrinding = true;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Vector3 railDir = (railEnd.position - railStart.position).normalized;
        direction = 1; // Start at the beginning
        progress = 0f;
    }

    void StartGrindingAtEnd()
    {
        isGrinding = true;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        Vector3 railDir = (railEnd.position - railStart.position).normalized;
        direction = -1; // Start at the end
        progress = 1f;
    }

    void GrindAlongRail()
    {
        progress += direction * grindSpeed * Time.deltaTime;
        float t = Mathf.Clamp01(progress);
        Vector3 newPos = Vector3.Lerp(railStart.position, railEnd.position, t);

        // Save the current grind direction
        lastGrindDirection = (railEnd.position - railStart.position).normalized * direction;

        player.position = newPos;

        // Add points over time
        if (pointsSystem != null)
        {
            int pointsToAdd = Mathf.FloorToInt(pointRate * Time.deltaTime);
            pointsSystem.TrickAddPoints(pointsToAdd);
        }

        // End grind at the end of rail
        if (t == 0f || t == 1f)
        {
            StopGrinding();
        }
    }

    void StopGrinding(bool jumpOff = false)
    {
        isGrinding = false;

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;

            Vector3 exitForce;

            if (jumpOff)
            {
                // Manual jump off
                exitForce = lastGrindDirection.normalized * grindSpeed + Vector3.up * 6f;
            }
            else
            {
                // Natural end of rail
                exitForce = lastGrindDirection.normalized * grindSpeed + Vector3.up * 2f;
            }

            rb.AddForce(exitForce, ForceMode.VelocityChange);
        }
    }
}
