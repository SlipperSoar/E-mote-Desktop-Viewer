#if UNITY_EDITOR
#define WRITE_TO_FILE
#endif

#define LOG_TIMESTAMP

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.IO;
using System.Text;

static public class M2DebugLog
{
#if WRITE_TO_FILE
	static private string mPath = "M2DebugLog.txt";
	static private bool mAppend = true;
	static private bool mFlush = true;
	static private StreamWriter mWriter;
#endif

	static M2DebugLog() {
		open();
	}
	
	static public void open() {
#if WRITE_TO_FILE
		mWriter = new StreamWriter(mPath, mAppend);
#endif
	}

	static public void close() {
#if WRITE_TO_FILE
		mWriter.Close();
		mWriter = null;
#endif
	}

	static public void print(string str) {
#if WRITE_TO_FILE
		mWriter.Write(str+"\n");
		if (mFlush) mWriter.Flush();
#endif
		Log(str);
	}

	static public void printf(string format, params object[] args) {
		string str = string.Format(format+"\n", args);
#if WRITE_TO_FILE
		mWriter.Write(str);
		if (mFlush) mWriter.Flush();
#endif
    Log(str);
	}

    static public void Log(string str) {
#if LOG_TIMESTAMP && UNITY_EDITOR
      str = String.Format("{0,12:F3}: {1}", 
                          EditorApplication.timeSinceStartup, 
                          str);
#endif        
      Debug.Log(str);
    }
}

