using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera;
    public Camera cutsceneCamera;

    [Header("Cutscene Settings")]
    public float cutsceneDuration = 5f;

    [Header("Particle Effects")]
    public ParticleSystem cutsceneParticles;

    [Header("Cutscene Movement")]
    public List<Transform> cameraWaypoints; // Add in order
    public float moveDurationPerSegment = 2f;

    [Header("Look At Target")]
    public Transform lookTarget; // Drag the object you want the camera to look at

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        mainCamera.enabled = false;
        cutsceneCamera.enabled = true;

        if (cutsceneParticles != null)
            cutsceneParticles.Play();

        for (int i = 0; i < cameraWaypoints.Count - 1; i++)
        {
            Transform start = cameraWaypoints[i];
            Transform end = cameraWaypoints[i + 1];
            yield return StartCoroutine(MoveCameraWithLook(cutsceneCamera.transform, start, end, moveDurationPerSegment));
        }

        float remainingTime = Mathf.Max(0f, cutsceneDuration - (moveDurationPerSegment * (cameraWaypoints.Count - 1)));
        yield return new WaitForSeconds(remainingTime);

        if (cutsceneParticles != null)
            cutsceneParticles.Stop();

        cutsceneCamera.enabled = false;
        mainCamera.enabled = true;
    }

    IEnumerator MoveCameraWithLook(Transform cam, Transform from, Transform to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            cam.position = Vector3.Lerp(from.position, to.position, t);

            if (lookTarget != null)
                cam.LookAt(lookTarget);

            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.position = to.position;

        if (lookTarget != null)
            cam.LookAt(lookTarget);
    }
}
