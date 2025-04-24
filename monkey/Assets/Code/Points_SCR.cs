using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Points_SCR : MonoBehaviour {
    [SerializeField] private Text Points_text;

    int Points = 0;

    public void TrickAddPoints() {
        Points += 10;
        Points_text.text = "Aura Points: " + Points;
    }

    public void TrickAddPoints(int AddPoints) {
        Points += AddPoints;
        Points_text.text = "Aura Points: " + Points;
    }

    public void resetScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // print(SceneManager.GetActiveScene().name); // Used to test can delete later
    }

/*
    // Used to test can delete later
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            resetScene();
        }
    }
*/
}
