using UnityEngine;

public class SpinAndBob : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 100f, 0);

    [Header("Bob Settings")]
    [SerializeField] private float bobHeight = 0.25f;   // How far up/down it moves
    [SerializeField] private float bobSpeed = 2f;       // How fast it bobs

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Spin
        transform.Rotate(rotationSpeed * Time.deltaTime);

        // Bob up and down using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
