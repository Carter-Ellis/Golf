using UnityEngine;

public class ParallaxParticleFollow : MonoBehaviour
{
    public Transform cameraTransform;
    [Range(0f, 1f)] public float parallaxFactor = 0.1f;

    private Vector3 lastCamPos;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCamPos = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCamPos;
        transform.position += delta * parallaxFactor;
        lastCamPos = cameraTransform.position;
    }
}
