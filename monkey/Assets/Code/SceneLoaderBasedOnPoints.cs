using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SceneLoaderBasedOnPoints : MonoBehaviour
{
    [SerializeField] private Points_SCR pointsScript;
    [SerializeField] private int pointsThreshold = 50;

    // Drag scene assets into these fields
    [SerializeField] private SceneAsset winScene;
    [SerializeField] private SceneAsset loseScene;

    // Internal scene name strings
    private string winSceneName;
    private string loseSceneName;

    private void Awake()
    {
#if UNITY_EDITOR
        if (winScene != null)
            winSceneName = winScene.name;
        if (loseScene != null)
            loseSceneName = loseScene.name;
#endif
    }

    public void CheckPointsAndLoadScene()
    {
        if (pointsScript == null)
        {
            Debug.LogError("Points_SCR script is not assigned in the inspector.");
            return;
        }

        int currentPoints = pointsScript.GetCurrentPoints();

        if (currentPoints >= pointsThreshold)
        {
            Debug.Log("Points are enough! Loading " + winSceneName);
            GameManager.LoadScene(winSceneName); // or loseSceneName

        }
        else
        {
            Debug.Log("Not enough points! Loading " + loseSceneName);
            GameManager.LoadScene(winSceneName); // or loseSceneName

        }
    }
}
