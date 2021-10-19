using UnityEngine;
using System.Collections;

[AddComponentMenu("Emote Player/Emote Anaglyph Composite")]
public class EmoteAnaglyphComposite : MonoBehaviour
{
    public Camera leftEyeCamera;
    public Camera rightEyeCamera;
    public GameObject leftEyeQuad;
    public GameObject rightEyeQuad;
    public bool grayscale = false;

    void Start() {
        applyGrayscale();
        Camera camera = GetComponent<Camera>();
        float aspect = camera.aspect;
        leftEyeCamera.aspect = aspect;
        rightEyeCamera.aspect = aspect;
        leftEyeQuad.GetComponent<Transform>().localScale = new Vector3(aspect, 1, 1);
        rightEyeQuad.GetComponent<Transform>().localScale = new Vector3(aspect, 1, 1);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.G))
            grayscale = ! grayscale;
        applyGrayscale();
    }

    void applyGrayscale() {
        if (! grayscale) {
            M2DebugLog.printf("applyGrayscale(): {0}", leftEyeQuad.GetComponent<Renderer>().material);
           
            leftEyeQuad.GetComponent<Renderer>().material.DisableKeyword("GRAYSCALE_ON");
            rightEyeQuad.GetComponent<Renderer>().material.DisableKeyword("GRAYSCALE_ON");
        } else {
            leftEyeQuad.GetComponent<Renderer>().material.EnableKeyword("GRAYSCALE_ON");
            rightEyeQuad.GetComponent<Renderer>().material.EnableKeyword("GRAYSCALE_ON");
        }
    }
}
