using UnityEngine;

public class KillBoxAndReset : MonoBehaviour {
    [SerializeField] private Transform ScateboardTF;
    [SerializeField] private Rigidbody ScateboardRB;
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private SkateboardBehaviour SbBh;
    
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            print("Touched Ground");
            resetLocAndRot();
            //SbBh.PauseMovement();
        }
    }

    private void resetLocAndRot() {
        if (SpawnPoint != null) {
            ScateboardTF.position = SpawnPoint.position;
            ScateboardTF.rotation = SpawnPoint.rotation;
            ScateboardRB.velocity = Vector3.zero;
        }
    }
}
