using UnityEngine;
using System.Collections;

[AddComponentMenu("Emote Player/Emote Lip Synch Control")]
public class EmoteLipSynchControl : MonoBehaviour
{
    [HeaderAttribute("Target")]
    public EmotePlayer targetPlayer;
    public AudioSource targetAudio;
    public Transform audioTransform;
    public bool autoLipSynch;
    [HeaderAttribute("Emote Variable")]
    public string variableLabel = "face_talk";
    public float variableRange = 5;
    [HeaderAttribute("Sampling Operation")]
    public float sampleDynamicRange = 0.5f;
    public float sampleGamma = 4;
    public float smoothingRate = 0.3f;
    public bool muteHysteresis = true;
    public float muteHysteresisRangeMin = 1;
    public float muteHysteresisRangeMax = 2;
    [HeaderAttribute("Auto LipSynch Operation")]
    public float transitionInterval = 0.03f;
    [HeaderAttribute("Debug")]
    public bool showStatus = false;
    public GUIStyle statusStyle;

    private float prevValue = 0;
    private bool inMute = true;

    void Start() {
        if (targetPlayer == null)
            targetPlayer = this.GetComponent(typeof(EmotePlayer)) as EmotePlayer;
        if (targetAudio == null)
            targetAudio = this.GetComponent(typeof(AudioSource)) as AudioSource;
    }

    void StopLipSynch() {
        targetPlayer.SetVariable(variableLabel, 0);
        prevValue = 0;
        inMute = true;
    }

    void Update() {
        if (targetPlayer == null)
            return;
        if (targetAudio != null)
            UpdateAudioSynch();
        else
            UpdateAutoSynch();
    }

    void UpdateAutoSynch() {
        if (autoLipSynch) {
            inMute = false;
            float curValue = Random.Range(0.0f, variableRange);
            curValue = Mathf.Lerp(prevValue, curValue, 1 / transitionInterval * Time.deltaTime);
            prevValue = curValue;
            targetPlayer.SetVariable(variableLabel, curValue);
        } else {
            if (! inMute) 
                StopLipSynch();
        }
    }

    void UpdateAudioSynch() {
        if (! targetAudio.isPlaying) {
            StopLipSynch();
            return;
        }

        if (audioTransform != null) 
            audioTransform.position = targetPlayer.GetCharaMarker("mouth");

        AudioClip clip = targetAudio.clip;
        int sampleBeginTime = (int)(Mathf.Max(0, targetAudio.time - Time.deltaTime) * clip.frequency);
        int sampleLength = (int)(clip.frequency * clip.channels * Time.deltaTime);
        if (sampleBeginTime < 0 || sampleLength <= 0)
            return;

        float[] samples = new float[sampleLength];
        clip.GetData(samples, sampleBeginTime);

        float curValue = 0;
        for (var i = 0; i < sampleLength; i++)
            curValue += Mathf.Pow(Mathf.Abs(samples[i] / sampleDynamicRange), 1  / sampleGamma);
        curValue /= sampleLength;
        curValue = curValue * variableRange;

        curValue = prevValue * smoothingRate + curValue * (1 - smoothingRate);
        if (muteHysteresis) {
            if (inMute) {
                if (curValue > muteHysteresisRangeMax) {
                    inMute = false;
                    prevValue = curValue;
                } else if (curValue >= muteHysteresisRangeMin) {
                    curValue = prevValue;
                } else {
                    prevValue = curValue;
                }
            } else {
                if (curValue < muteHysteresisRangeMin)
                    inMute = true;
                prevValue = curValue;
            }
        }

        targetPlayer.SetVariable(variableLabel, curValue);
    }

    void OnGUI() {
        if (targetPlayer == null
            || targetAudio == null)
            return;

        if (! showStatus)
            return;

        Vector2 guiScreenSize = new Vector2(800, 600);
        float scale = Mathf.Max(Screen.width / guiScreenSize.x, Screen.height / guiScreenSize.y);
        GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);
        GUILayout.Label(System.String.Format("Sampling Value: {0:f2}", prevValue), statusStyle);
        if (GUILayout.Button("Play"))
            targetAudio.Play();
    }
}
