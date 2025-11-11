using UnityEngine;

public class AwesomeCollectible : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreManager.instance.AddFive();
            Destroy(gameObject);
        }
    }
}
