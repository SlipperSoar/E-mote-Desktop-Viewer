
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("Emote Player/Emote Device")]
public class EmoteDevice : MonoBehaviour {
  private EmoteDeviceManager mManager;

  void Start() {
    mManager = new EmoteDeviceManager();
    mManager.Load();
  }

  void OnDestroy() {
    mManager.Unload();
  }
};

