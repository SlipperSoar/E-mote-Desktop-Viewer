using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor( typeof(EmoteAPILogger) )]
public class EmoteAPILoggerEditor : Editor {
    Vector2 scroll = Vector2.zero;

    public override void OnInspectorGUI() {
        if (target == null)
            return;

        EmoteAPILogger logger = target as EmoteAPILogger;

        EmotePlayer player = EditorGUILayout.ObjectField("Emote Player", logger.player, typeof(EmotePlayer), true) as EmotePlayer;
        logger.player = player;

        if (player != null) {
            EditorGUILayout.BeginHorizontal();

            bool isRecording = player.recordingAPILog;
            bool isReplaying = player.replayingAPILog;

            GUI.enabled = ! isRecording;
            if (GUILayout.Button("Record")) {
                player.ClearAPILog();
                player.StartRecordAPILog();
            }
            
            GUI.enabled = ! isReplaying;
            if (GUILayout.Button("Replay")) {
                player.apiLog = logger.log;
                player.StartReplayAPILog();
            }
            
            GUI.enabled = isRecording || isReplaying;
            if (GUILayout.Button("Stop")) {
                if (isRecording) {
                    player.StopRecordAPILog();
                    logger.log = player.apiLog;
                    GUI.FocusControl("");
                }
                if (isReplaying) 
                    player.StopReplayAPILog();
            }

            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
        }

        if (logger.log.Length <= 15000) {
            scroll = EditorGUILayout.BeginScrollView(scroll, true, true, GUILayout.Height(100));
            logger.log = EditorGUILayout.TextArea(logger.log, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        } else {
            EditorGUILayout.HelpBox("current log is too large to display on textarea.", MessageType.Warning);
            scroll = EditorGUILayout.BeginScrollView(scroll, true, true, GUILayout.Height(100));
            GUI.enabled = false;
            EditorGUILayout.TextArea(logger.log.Substring(0, 15000), GUILayout.ExpandHeight(true));
            GUI.enabled = true;
            EditorGUILayout.EndScrollView();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("Save Log to File")) {
            var path = EditorUtility.SaveFilePanel("Save Log as TXT", "", "", "txt");
            if (path.Length != 0) 
                File.WriteAllText(path, logger.log);
        }
        if (GUILayout.Button("Load Log from File")) {
            var path = EditorUtility.OpenFilePanel("Load Log from TXT", "", "txt");
            if (path.Length != 0) {
                logger.log = File.ReadAllText(path);
                GUI.FocusControl("");
            }
        }
        EditorGUILayout.EndHorizontal();
     }
};
