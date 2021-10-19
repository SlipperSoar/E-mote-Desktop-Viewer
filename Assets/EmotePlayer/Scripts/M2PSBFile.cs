using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[System.Serializable]
public class M2PSBFile : ScriptableObject {
	[SerializeField]
	private TextAsset mPSBObject;
	[SerializeField]
    private string mPath;
	
	public TextAsset PSBObject {
		get { return mPSBObject; }
        set { mPSBObject = null;
          if (value == null) {
            mPath = null;
            return;
          }
          if (!isValidObject(value)) {
			throw new System.ArgumentException();
          }
          mPSBObject = value;
#if UNITY_EDITOR
		  Regex r = new Regex(@"^Assets.*/Resources/");
          mPath = AssetDatabase.GetAssetPath(value);
          mPath = r.Replace(mPath, "");
#endif
        }
	}

    public void setRawPSBObject(TextAsset value) {
       mPSBObject = value;
       mPath = null;
    }
	
	public M2PSBFile() {
		// mPSBObject = null;
	}
	
	public M2PSBFile(M2PSBFile file) {
		mPSBObject = file.mPSBObject;
        mPath = file.mPath;
	}
	
	public M2PSBFile(TextAsset obj) {
      PSBObject = obj;
	}


	public TextAsset Object() {
		return mPSBObject;
	}
	
	public static bool isValidObject(TextAsset obj) {
#if UNITY_EDITOR
		string path = AssetDatabase.GetAssetPath(obj);
		// M2DebugLog.printf("isValidObject(): path={0}", path);
		return (path.EndsWith(".bytes"));
#else
		return true;
#endif
	}

    public string path {
      get { return mPath; }
      set {
        mPath = value;
        mPSBObject = (TextAsset)Resources.Load(value, typeof(TextAsset));
        if (mPSBObject == null)
          mPath = null;
      }
    }

	public override string ToString() {
      return mPath;
	}

}
