using UnityEngine;

public class EvenCoolerCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddSeven();
            Destroy(gameObject);
        }
    }
}
