using UnityEngine;
using System.Collections;

[AddComponentMenu("Emote Player/Emote Fps Camera Control")]
public class EmoteFpsCameraControl : MonoBehaviour
{
    [HeaderAttribute("Target")]
    public EmotePlayer targetPlayer;
    public Camera targetCamera;
    public enum LookAtTargetPlayer {
        none,
        top,
        eye,
        mouth,
        bust,
        bottom,
    };
    public LookAtTargetPlayer lookAtTargetPlayer = LookAtTargetPlayer.none;
    [HeaderAttribute("Movement")]
    public float speed = 1.3f;
    public float rotSpeed = 60;
    public float slerpSpeed = 10;
    private Vector3 pos;
    private float rotX = 0, rotY = 0;
    [HeaderAttribute("Debug")]
    public bool showStatus = false;
    public GUIStyle statusStyle;
    static private string[] markers = System.Enum.GetNames(typeof(LookAtTargetPlayer));
    static private string[] variables = { "body_LR", "body_UD", "head_LR", "head_UD", "face_eye_LR", "face_eye_UD", "vr_LR", "vr_UD" };

    void Start()
    {
        if (targetPlayer == null)
            targetPlayer = this.GetComponent<EmotePlayer>();
        if (targetCamera == null)
            targetCamera = this.GetComponent<Camera>();

        if (targetCamera != null) {
            pos = targetCamera.transform.position;
            Vector3 rv = targetCamera.transform.rotation.eulerAngles;
            rotX = rv.x;
            rotY = rv.y;
        }            
    }

    void Update() {
        if (targetPlayer == null
            || targetCamera == null) 
            return;

        Vector3 ofst = Vector3.zero;
        Vector3 forward = targetCamera.transform.forward;
        forward.y = 0;
        forward = forward.normalized;
        if (Input.GetKey(KeyCode.A)) ofst += Quaternion.AngleAxis(-90, Vector3.up) * forward;
        if (Input.GetKey(KeyCode.D)) ofst += Quaternion.AngleAxis(+90, Vector3.up) * forward;
        if (Input.GetKey(KeyCode.S)) ofst += Quaternion.AngleAxis(+180, Vector3.up) * forward;
        if (Input.GetKey(KeyCode.W)) ofst += forward;
        if (Input.GetKey(KeyCode.UpArrow)) ofst += Vector3.up;
        if (Input.GetKey(KeyCode.DownArrow)) ofst += Vector3.down;
        pos += ofst * speed * Time.deltaTime;
        targetCamera.transform.position = Vector3.Slerp(targetCamera.transform.position, pos, Time.deltaTime * slerpSpeed);
        if (Input.GetKeyDown(KeyCode.L))
            lookAtTargetPlayer = (LookAtTargetPlayer)(((int)lookAtTargetPlayer + 1) % (System.Enum.GetValues(typeof(LookAtTargetPlayer)).Length));
        if (lookAtTargetPlayer == LookAtTargetPlayer.none
            || targetPlayer == null) {
            if (Input.GetKey(KeyCode.Q)) rotY -= rotSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E)) rotY += rotSpeed * Time.deltaTime;;
            if (Input.GetKey(KeyCode.LeftArrow)) rotX += rotSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow)) rotX -= rotSpeed * Time.deltaTime;
            targetCamera.transform.rotation = Quaternion.Slerp(targetCamera.transform.rotation, Quaternion.Euler(rotX, rotY, 0), Time.deltaTime * slerpSpeed);
        } else {
            targetCamera.transform.LookAt(targetPlayer.GetCharaMarker(markers[(int)lookAtTargetPlayer]));
            Vector3 rv = targetCamera.transform.rotation.eulerAngles;
            rotX = rv.x;
            rotY = rv.y;
        }
    }
    
    void OnGUI() {
        if (targetPlayer == null
            || targetCamera == null) 
            return;

        if (! showStatus)
            return;

        Vector2 guiScreenSize = new Vector2(1280, 720);
        float scale = System.Math.Max(Screen.width / guiScreenSize.x, Screen.height / guiScreenSize.y);
        GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);
        GUILayout.Label(System.String.Format("Character Height: {0:f2}cm", targetPlayer.charaHeight * 100), statusStyle);
        GUILayout.Label(System.String.Format("Camera Height: {0:f2}cm", targetCamera.transform.position.y * 100), statusStyle);
        GUILayout.Label(System.String.Format("Length between Character and Camera: {0:f2}cm", 100 * Vector2.Distance(new Vector2(targetPlayer.transform.position.x, targetPlayer.transform.position.z),
                                                                                                                     new Vector2(targetCamera.transform.position.x, targetCamera.transform.position.z))), statusStyle);
        GUILayout.Label(System.String.Format("Look at: {0}", markers[(int)lookAtTargetPlayer]), statusStyle);
        GUILayout.Label(System.String.Format("Top: {0:f2}, Bottom: {1:f2}", targetPlayer.GetCharaMarker("top").y * 100, targetPlayer.GetCharaMarker("bottom").y * 100), statusStyle);
        foreach (string variable in variables) {
            GUILayout.Label(System.String.Format("{0}: {1:f2}", variable, targetPlayer.GetVariableDiff("billboard", variable)), statusStyle);
        }
   }
}