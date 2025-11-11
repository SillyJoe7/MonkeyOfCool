using UnityEngine;

public class MazeFog : MonoBehaviour
{
    [Header("Fog Settings")]
    [SerializeField] private Color fogColor = Color.black;
    [SerializeField] private float fogStartDistance = 5f;
    [SerializeField] private float fogEndDistance = 15f;

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogMode = FogMode.Linear;
        RenderSettings.fogStartDistance = fogStartDistance;
        RenderSettings.fogEndDistance = fogEndDistance;
    }
}
