using UnityEngine;

public class BackflipOfCool : MonoBehaviour
{
    public float rotationAmount = 360f;
    public float duration = 2f;
    public Vector3 rotationAxis = Vector3.up;

    private float elapsed = 0f;
    private float totalRotation = 0f;
    private bool rotating = false;

    [Header("Points System")]
    public Points_SCR points_SCR;  // Assign this in the Inspector
    private void Start()
    {
        if (points_SCR == null)
        {
            points_SCR = FindObjectOfType<Points_SCR>();
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && !rotating)
        {
            StartRotation();
        }

        if (rotating)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float rotationThisFrame = rotationAmount * t - totalRotation;
            transform.Rotate(rotationAxis, rotationThisFrame);
            totalRotation += rotationThisFrame;

            if (totalRotation >= rotationAmount)
            {
                rotating = false;
            }
        }
    }

    private void StartRotation()
    {
        elapsed = 0f;
        totalRotation = 0f;
        rotating = true;

        if (points_SCR != null)
        {
            points_SCR.TrickAddPoints(7);
        }
        else
        {
            Debug.LogWarning("Points_SCR not assigned to BackflipOfCool.");
        }
    }
}
