using UnityEngine;

public class RailGrindManager : MonoBehaviour
{
    public RailGrind[] allRails;
    public float activationDistance = 10f;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        allRails = FindObjectsOfType<RailGrind>();
    }

    void Update()
    {
        if (player == null) return;

        RailGrind nearestRail = null;
        float nearestDist = Mathf.Infinity;

        foreach (var rail in allRails)
        {
            float distToStart = Vector3.Distance(player.position, rail.railStart.position);
            float distToEnd = Vector3.Distance(player.position, rail.railEnd.position);
            float minDist = Mathf.Min(distToStart, distToEnd);

            if (minDist < activationDistance && minDist < nearestDist)
            {
                nearestDist = minDist;
                nearestRail = rail;
            }
        }

        // Enable prompt on nearest rail only
        foreach (var rail in allRails)
        {
            bool isNearest = rail == nearestRail;
            rail.SetPromptActive(isNearest);
        }
    }
}
