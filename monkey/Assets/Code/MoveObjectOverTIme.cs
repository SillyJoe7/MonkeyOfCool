using UnityEngine;

public class MoveObjectOverTime : MonoBehaviour
{
    private bool isMoving = false;  // To check if the object is moving
    public float speed = 5f;        // Speed of movement (public so it can be adjusted in the Inspector)
    public Vector3 distance = new Vector3(10f, 0f, 0f); // Distance to move (public so it can be adjusted in the Inspector)

    private Vector3 initialPosition;  // To store the initial position of the object
    private Vector3 targetPosition;   // The target position to move towards

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;  // Store the initial position
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            // Move the object towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // If the object reaches the target position, stop moving
            if (transform.position == targetPosition)
            {
                isMoving = false;
            }
        }
    }

    // Public method to toggle the movement
    public void ToggleMove()
    {
        isMoving = !isMoving;

        if (isMoving)
        {
            // Set the target position relative to the initial position
            targetPosition = initialPosition + distance;
        }
        else
        {
            // If movement is toggled off, reset the target position back to the initial position
            targetPosition = initialPosition;
        }
    }
}
