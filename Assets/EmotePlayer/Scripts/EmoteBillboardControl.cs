using UnityEngine;
using System.Collections;

[AddComponentMenu("Emote Player/Emote Billboard Control")]
public class EmoteBillboardControl : MonoBehaviour
{
    [HeaderAttribute("Target")]
    public EmotePlayer targetPlayer;
    public Camera targetCamera;
    [HeaderAttribute("Billboard")]
    [RangeAttribute(0,1)]
    public float tiltRate = 0.05f;
    public float tiltAngleLimit = 20;
    public float fadeoutBeginDistance = 0;
    public float fadeoutEndDistance = 0;
    [HeaderAttribute("Emote Modules")]
    public float headTrackingThreshold = 10.0f;
    public float headTrackingInterval = 0.2f;
    private bool inHeadTracking = false;
    public float bodyTrackingThreshold = 20.0f;
    public float bodyTrackingInterval = 0.4f;
    private bool inBodyTracking = false;
    [HeaderAttribute("Stereovion")]
    public float pupillaryDistance = 0.065f;
    [RangeAttribute(-30,0)]
    public float angleMin = -30;
    [RangeAttribute(0,30)]
    public float angleMax = 30;
    public float parallaxRatioLimit = 0.5f;

    private bool firstShot = true;
    private Vector3 eyeDir, headDir, bodyDir;
    private Vector3 playerPosition, cameraPosition;

    void Start() {
        if (targetPlayer == null)
            targetPlayer = this.GetComponent(typeof(EmotePlayer)) as EmotePlayer;
        if (targetCamera == null)
            targetCamera = this.GetComponent(typeof(Camera)) as Camera;
    }

    void LateUpdate() {
        if (targetPlayer == null
            || targetCamera == null) 
            return;

        GetPosition();
        UpdateBillboardDirection();
        UpdateEmoteVariables();
        TiltBillboard();
    }

    void GetPosition() {
        cameraPosition = targetCamera.transform.position;
        playerPosition = targetPlayer.transform.position;
    }

    void UpdateEmoteVariables() {
        Vector3 eyePosition = targetPlayer.GetCharaMarker("eye");
        Vector3 dirEyeToCamera = cameraPosition - eyePosition;

        Vector3 dir = dirEyeToCamera;

        if (! firstShot) {
            eyeDir = dir;
        } else {
            targetPlayer.Skip();
            firstShot = false;
            bodyDir = headDir = eyeDir = dir;
        }
        if (! inHeadTracking) {
            if (Vector3.Angle(headDir, eyeDir) >= headTrackingThreshold) {
                inHeadTracking = true;
            }
        }

        if (inHeadTracking) {
            headDir = Vector3.Lerp(headDir, eyeDir, Time.deltaTime * headTrackingThreshold / Vector3.Angle(headDir, eyeDir) / headTrackingInterval);
            if (Vector3.Angle(headDir, eyeDir) < 0.1) {
                headDir = eyeDir;
                inHeadTracking = false;
            }
        }
        if (! inBodyTracking) {
            if (Vector3.Angle(bodyDir, eyeDir) >= bodyTrackingThreshold) {
                inBodyTracking = true;
                inHeadTracking = true;
            }
        }

        if (inBodyTracking) {
            bodyDir = Vector3.Lerp(bodyDir, eyeDir, Time.deltaTime * bodyTrackingThreshold / Vector3.Angle(bodyDir, eyeDir) / bodyTrackingInterval);
            if (Vector3.Angle(bodyDir, eyeDir) < 0.1) {
                bodyDir = eyeDir;
                inBodyTracking = false;
            }
        }

        if (! inHeadTracking) {
            if (Vector3.Angle(headDir, eyeDir) >= headTrackingThreshold) 
                inHeadTracking = true;
        }
        float headHorizontal = - Vector2.Angle(new Vector2(headDir.x, headDir.z), new Vector2(eyeDir.x, eyeDir.z));
        if (headDir.z * eyeDir.x - headDir.x * eyeDir.z > 0)
            headHorizontal = - headHorizontal;

        float headVertical = - Mathf.Rad2Deg * Mathf.Atan2(headDir.y, Mathf.Sqrt(headDir.x * headDir.x + headDir.z * headDir.z));

        float bodyHorizontal = - Vector2.Angle(new Vector2(bodyDir.x, bodyDir.z), new Vector2(eyeDir.x, eyeDir.z));
        if (bodyDir.z * eyeDir.x - bodyDir.x * eyeDir.z > 0)
            bodyHorizontal = - bodyHorizontal;

        float eyeHorizontal = -headHorizontal / 2;
        float eyeVertical =  - Mathf.Rad2Deg * Mathf.Atan2(eyeDir.y, Mathf.Sqrt(eyeDir.x * eyeDir.x + eyeDir.z * eyeDir.z));
        eyeVertical -= headVertical;

        targetPlayer.SetVariableDiff("billboard", "body_LR", bodyHorizontal);
        targetPlayer.SetVariableDiff("billboard", "vr_LR", bodyHorizontal * 1.5f);
        targetPlayer.SetVariableDiff("billboard", "head_LR", headHorizontal);
        targetPlayer.SetVariableDiff("billboard", "head_UD", headVertical);
        targetPlayer.SetVariableDiff("billboard", "face_eye_UD", eyeVertical);
        targetPlayer.SetVariableDiff("billboard", "face_eye_LR", eyeHorizontal);

        if (targetPlayer.HasCharaProfile("bust")) {
            Vector3 bustPosition = targetPlayer.GetCharaMarker("bust");
            Vector3 dirBustToCamera = cameraPosition - bustPosition;
            float bodyVertical = - Mathf.Rad2Deg * Mathf.Atan2(dirBustToCamera.y, Mathf.Sqrt(dirBustToCamera.x * dirBustToCamera.x + dirBustToCamera.z * dirBustToCamera.z));
            targetPlayer.SetVariableDiff("billboard", "body_UD", -bodyVertical);
            targetPlayer.SetVariableDiff("billboard", "vr_UD", -bodyVertical);
        }
        else {
            targetPlayer.SetVariableDiff("billboard", "body_UD", 0);
            targetPlayer.SetVariableDiff("billboard", "vr_UD", 0);
        }
    }

    float asymptotic(float value, float limit, float coff = 1.6f) {
        return (1 - Mathf.Exp(-coff * (value / limit))) * limit;
    }

    void UpdateBillboardDirection() {
        targetPlayer.transform.LookAt(new Vector3(cameraPosition.x, playerPosition.y, cameraPosition.z));
        targetPlayer.transform.Rotate(Vector3.up, 180);
        // fadeout 
        if (fadeoutBeginDistance > 0
            && fadeoutEndDistance > 0
            && fadeoutBeginDistance >= fadeoutEndDistance) {
            Vector3 top = targetPlayer.GetCharaMarker("top");
            Vector3 bottom = targetPlayer.GetCharaMarker("bottom");
            Vector3 myVector = cameraPosition - top;
            Vector3 baseVector = bottom - top;
            float dotProduct = Vector3.Dot(myVector, baseVector);
            Vector3 nearestPoint;
            if (dotProduct > 0) {
                float baseLength = baseVector.magnitude;
                float projection = dotProduct / baseLength;
                if (projection < baseLength) {
                    nearestPoint = top + baseVector * projection / baseLength;
                } else {
                    nearestPoint = bottom;
                }
            } else {
                nearestPoint = top;
            }
            float dist = Vector3.Distance(nearestPoint, cameraPosition);
            float alpha;
            if (dist <= fadeoutEndDistance)
                alpha = 0;
            else if (dist >= fadeoutBeginDistance)
                alpha = 1;
            else 
                alpha = (dist - fadeoutEndDistance) / (fadeoutBeginDistance - fadeoutEndDistance);
            if (targetPlayer.isLegacyTransform
                || ! targetPlayer.mapToRenderTexture) {
                Color color = targetPlayer.vertexColor;
                color.a = alpha;
                targetPlayer.vertexColor = color;
            } else {
                Color color = targetPlayer.mainColor;
                color.a = alpha;
                targetPlayer.mainColor = color;
            }
        }
        // set stereovision parallax ratio
        if (targetPlayer.stereovisionEnabled) { 
            Quaternion cameraRotation = Quaternion.AngleAxis(targetCamera.transform.eulerAngles.y, Vector3.up);
            Vector3 leftEyePosition = cameraPosition + cameraRotation * Vector3.left * pupillaryDistance / 2;
            Vector3 rightEyePosition = cameraPosition + cameraRotation * Vector3.right * pupillaryDistance / 2;
            float angle = Vector2.Angle(new Vector2(leftEyePosition.x, leftEyePosition.z) - new Vector2(playerPosition.x, playerPosition.z),
                                        new Vector2(rightEyePosition.x, rightEyePosition.z) - new Vector2(playerPosition.x, playerPosition.z));
            float parallaxRatio = angle / (angleMax - angleMin);
            parallaxRatio = asymptotic(parallaxRatio, parallaxRatioLimit);
            Vector3 cameraPlane = (targetCamera.transform.rotation * Vector3.forward).normalized;
            Vector3 cameraUp = (targetCamera.transform.rotation * Vector3.up);
            Vector3 playerUp = Vector3.ProjectOnPlane(targetPlayer.transform.rotation * Vector3.up, cameraPlane);
            float roll = Vector3.Angle(cameraUp, playerUp);
            float rollRatio = Mathf.Cos(roll * Mathf.Deg2Rad);
            targetPlayer.stereovisionParallaxRatio = parallaxRatio * rollRatio;
        }
        // culling billboard
        targetPlayer.culled = Vector3.Angle(targetPlayer.transform.forward, targetCamera.transform.forward) > 90;
    }

    void TiltBillboard() {
        float targetY = playerPosition.y;
        // forward billboard to camera.
        if (tiltRate > 0) 
            targetY = targetY * (1 - tiltRate) + (cameraPosition.y - targetPlayer.GetCharaMarker("eye").y) * tiltRate;
        targetPlayer.transform.LookAt(new Vector3(cameraPosition.x, targetY, cameraPosition.z));
        targetPlayer.transform.Rotate(Vector3.up, 180);
        // modify tilt angle to slowly closer to limit.
        if (tiltRate > 0) {
            Vector3 eulerAngles = targetPlayer.transform.eulerAngles;
            if (eulerAngles.x < 180)
                eulerAngles.x = asymptotic(eulerAngles.x, tiltAngleLimit);
            else
                eulerAngles.x = 360 - asymptotic((360 - eulerAngles.x), tiltAngleLimit);
            targetPlayer.transform.eulerAngles = eulerAngles;
        }
    }

     public void ResetFollow() {
        firstShot = true;
    }
}