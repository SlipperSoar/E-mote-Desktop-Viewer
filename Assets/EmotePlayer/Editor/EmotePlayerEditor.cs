using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[CustomEditor( typeof(EmotePlayer) )]
public class EmotePlayerEditor : Editor {

	public override void OnInspectorGUI() {
        if (target == null)
            return;

		EmotePlayer motion = target as EmotePlayer;

        EditorGUI.BeginChangeCheck();

        bool toggle;
        toggle = motion.toggleAppearance = EditorGUILayout.Foldout(motion.toggleAppearance, "Appearance");
        if (toggle) {
            EditorGUI.indentLevel++;
            motion.dataTextAsset = EditorGUILayout.ObjectField("Data File", motion.dataTextAsset, typeof(TextAsset), true) as TextAsset;
            motion.meshDivisionRatio = EditorGUILayout.Slider("Mesh Division Ratio", motion.meshDivisionRatio, 0, 5);
            motion.grayscale = EditorGUILayout.Slider("Grayscale Rate", motion.grayscale, 0, 1);
            motion.vertexColor = EditorGUILayout.ColorField("Vertex Color", motion.vertexColor);
            EditorGUI.indentLevel--;
        }
        toggle = motion.toggleTransform = EditorGUILayout.Foldout(motion.toggleTransform, "Transform");
        if (toggle) {
            EditorGUI.indentLevel++;
            string[] labels = { "Legacy", 
                                "Algin to origin", 
                                "Align to top", 
                                "Align to bottom", 
                                "Align to center",
                                "Align to bust",
                                "Align to mouth",
                                "Align to eye",
            };
            motion.transformAlignment = (EmotePlayer.TransformAlignment)EditorGUILayout.Popup("Alignment", (int)motion.transformAlignment, labels);
            if (motion.transformAlignment == EmotePlayer.TransformAlignment.LEGACY) {
                motion.coord = EditorGUILayout.Vector2Field("Coord", motion.coord);
                motion.rot = EditorGUILayout.FloatField("Rot", motion.rot);
                motion.scale = EditorGUILayout.FloatField("Scale", motion.scale);
            } else {
                motion.convolveObjectTransformToPhysics = EditorGUILayout.Toggle("Convolve Object Transform", motion.convolveObjectTransformToPhysics);
                if (! motion.isLegacyTransform 
                    && ! motion.isCharaProfileAvailable)
                    motion.scale = EditorGUILayout.FloatField("Scale", motion.scale);
                motion.fixedScale = EditorGUILayout.FloatField("Fixed Scale" , motion.fixedScale);
            }
            EditorGUI.indentLevel--;
        }

        toggle = motion.toggleRenderTexture = EditorGUILayout.Foldout(motion.toggleRenderTexture, "Render Texture");
        if (toggle) {
            EditorGUI.indentLevel++;
            motion.mapToRenderTexture = EditorGUILayout.BeginToggleGroup("Map to RenderTexture", motion.mapToRenderTexture);
            motion.renderTextureClearColor = EditorGUILayout.ColorField(new GUIContent("Clear Color"), motion.renderTextureClearColor, true, false, false, (ColorPickerHDRConfig)null);
            if (GUILayout.Button("Calc Suitable Clear Color"))
                motion.calcSuitableRenderTextureClearColor();
            motion.mainColor = EditorGUILayout.ColorField("Main Color", motion.mainColor);
            motion.texWidth = EditorGUILayout.IntPopup("Texture Width", motion.texWidth, EmotePlayer.TEX_WIDTH_POPUP_NAMES, EmotePlayer.TEX_WIDTH_POPUP_NUMS);
            motion.texHeight = EditorGUILayout.IntPopup("Texture Height", motion.texHeight, EmotePlayer.TEX_WIDTH_POPUP_NAMES, EmotePlayer.TEX_WIDTH_POPUP_NUMS);
            if (motion.transformAlignment != EmotePlayer.TransformAlignment.LEGACY) {
                var list = AllSortingLayer;
                var selectedIndex = list.IndexOf(motion.renderTextureSortingLayerName);
                if (selectedIndex == -1)
                    selectedIndex = list.IndexOf("Default");
                selectedIndex = EditorGUILayout.Popup("Sorting Layer", selectedIndex, list.ToArray());
                motion.renderTextureSortingLayerName = list[selectedIndex];
                motion.renderTextureSortingOrder = EditorGUILayout.IntField("Order In Layer", motion.renderTextureSortingOrder);
                motion.renderTextureMesh = EditorGUILayout.ObjectField("Mesh", motion.renderTextureMesh, typeof(Mesh), true) as Mesh;
                motion.renderTextureMaterial = EditorGUILayout.ObjectField("Material", motion.renderTextureMaterial, typeof(Material), true) as Material;
                motion.renderTextureResolutionCorrection = EditorGUILayout.Toggle("Resolution Correction", motion.renderTextureResolutionCorrection);
                motion.renderTextureMargin = EditorGUILayout.Slider("Texture Margin", motion.renderTextureMargin, 0.0f, 1.0f);
                motion.clipRenderTexture = EditorGUILayout.BeginToggleGroup("Clip", motion.clipRenderTexture);
                motion.renderTextureClipRect = EditorGUILayout.RectField(motion.renderTextureClipRect);
                EditorGUILayout.EndToggleGroup();
            }
            EditorGUILayout.EndToggleGroup();
            EditorGUI.indentLevel--;
        }
        if (motion.transformAlignment != EmotePlayer.TransformAlignment.LEGACY) {
            toggle = motion.toggleStereovision = EditorGUILayout.Foldout(motion.toggleStereovision, "Stereovision");
            if (toggle) {
                EditorGUI.indentLevel++;
                motion.stereovisionEnabled = EditorGUILayout.BeginToggleGroup("Enabled", motion.stereovisionEnabled);
                motion.stereoVisionLeftEyeLayer = EditorGUILayout.LayerField("Left Eye Layer", motion.stereoVisionLeftEyeLayer);
                motion.stereoVisionRightEyeLayer = EditorGUILayout.LayerField("Right Eye Layer", motion.stereoVisionRightEyeLayer);
                motion.stereovisionVolume = EditorGUILayout.Slider("Volume", motion.stereovisionVolume, 0, 1);
                motion.stereovisionParallaxRatio = EditorGUILayout.Slider("Parallax Ratio", motion.stereovisionParallaxRatio, -1, 1);
                EditorGUILayout.EndToggleGroup();
                EditorGUI.indentLevel--;
            }
        }

        toggle = motion.toggleBehavior = EditorGUILayout.Foldout(motion.toggleBehavior, "Behavior");
        if (toggle) {
            EditorGUI.indentLevel++;
            string[] timings = { "Update()", "LateUpdate()" };
            motion.updateTiming = (EmotePlayer.UpdateTiming)EditorGUILayout.Popup("Update Timing", (int)motion.updateTiming, timings);
            motion.stepUpdate = EditorGUILayout.Toggle("Step Update", motion.stepUpdate);
            motion.applyTimeScale = EditorGUILayout.Toggle("Apply Time Scale", motion.applyTimeScale);
            motion.speed = EditorGUILayout.Slider("Speed", motion.speed, 0, 5);
            motion.hairScale = EditorGUILayout.Slider("Hair Scale", motion.hairScale, 0, 3);
            motion.bustScale = EditorGUILayout.Slider("Bust Scale", motion.bustScale, 0, 3);
            motion.partsScale = EditorGUILayout.Slider("Parts Scale", motion.partsScale, 0, 3);
            motion.windSpeed = EditorGUILayout.Slider("Wind Speed", motion.windSpeed, -20, 20);
            float windPowMin = motion.windPowMin;
            float windPowMax = motion.windPowMax;
            EditorGUILayout.LabelField("Wind Power Min", windPowMin.ToString());
            EditorGUILayout.LabelField("Wind Power Max", windPowMax.ToString());
            EditorGUILayout.MinMaxSlider(ref windPowMin, ref windPowMax, 0, 5);
            motion.windPowMin = windPowMin;
            motion.windPowMax = windPowMax;
            if (GUILayout.Button("Skip"))
                motion.Skip();
            if (GUILayout.Button("Pass"))
                motion.Pass();
            EditorGUI.indentLevel--;
        }
        toggle = motion.toggleTimeline = EditorGUILayout.Foldout(motion.toggleTimeline, "Timelines");
        if (toggle) {
            EditorGUI.indentLevel++;
            string[] labels = motion.mainTimelineLabels;
            if (labels.Length > 0) {
                motion.mainTimelineIndex = EditorGUILayout.Popup("Main Timeline", motion.mainTimelineIndex, labels);
            }
            labels = motion.diffTimelineLabels;
            if (labels.Length > 0) {
                motion.diffTimelineSlot1Index = EditorGUILayout.Popup("Diff Timeline #1", motion.diffTimelineSlot1Index, labels);
                motion.diffTimelineSlot2Index = EditorGUILayout.Popup("Diff Timeline #2", motion.diffTimelineSlot2Index, labels);
                motion.diffTimelineSlot3Index = EditorGUILayout.Popup("Diff Timeline #3", motion.diffTimelineSlot3Index, labels);
                motion.diffTimelineSlot4Index = EditorGUILayout.Popup("Diff Timeline #4", motion.diffTimelineSlot4Index, labels);
                motion.diffTimelineSlot5Index = EditorGUILayout.Popup("Diff Timeline #5", motion.diffTimelineSlot5Index, labels);
                motion.diffTimelineSlot6Index = EditorGUILayout.Popup("Diff Timeline #6", motion.diffTimelineSlot6Index, labels);
                motion.diffTimelineFadeOutTime = EditorGUILayout.Slider("Fade Out Time", motion.diffTimelineFadeOutTime, 0, 1);
            }
            EditorGUI.indentLevel--;
        }
        toggle = motion.toggleVariable = EditorGUILayout.Foldout(motion.toggleVariable, "Variables");
        if (toggle) {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical("box");
            motion.variableTransitionTime = EditorGUILayout.Slider("Transition Time", motion.variableTransitionTime, 0, 2);
            motion.variableTransitionEasing = EditorGUILayout.Slider("Transition Easing", motion.variableTransitionEasing, -3, 3);
            EditorGUILayout.EndVertical();

            EditorGUILayout.HelpBox("The value of the following variables are not serialized.", MessageType.Warning);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.FindStyle("button"));
            buttonStyle.fontSize = 8;
            List<EmotePlayer.Variable> variableList = motion.variableList;
            int variableCount = variableList.Count;
            for (int i = 0; i < variableCount; i++) {
                EmotePlayer.Variable variable = variableList[i];
                float oldValue = motion.GetVariable(variable.label);
                float newValue = EditorGUILayout.Slider(variable.label, oldValue, variable.minValue, variable.maxValue);
                if (newValue != oldValue) {
                    motion.SetVariable(variable.label, newValue);
                }
                EditorGUILayout.BeginHorizontal("box");
                int frameCount = variable.frameList.Count;
                for (int j = 0; j < frameCount; j++) {
                    EmotePlayer.VariableFrame frame = variable.frameList[j];
                    if (GUILayout.Button(frame.label, buttonStyle, GUILayout.ExpandWidth(false))) {
                        if (EditorApplication.isPlaying)
                            motion.SetVariable(variable.label, frame.value, motion.variableTransitionTime, motion.variableTransitionEasing);
                        else
                            motion.SetVariable(variable.label, frame.value);
                    }
                    if (j % 8 == 7 && j != frameCount - 1) {
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.BeginHorizontal("box");
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        // [TODO] this operation don't effect undo after unity5.3
        if (EditorGUI.EndChangeCheck()) {
            EditorUtility.SetDirty( motion );
            if (! EditorApplication.isPlaying)
                EditorSceneManager.MarkSceneDirty( SceneManager.GetActiveScene() );
        }
    }

	private static SerializedProperty sortinglayer = null;
	public static SerializedProperty SortingLayer{
		get{
			if( sortinglayer == null )
			{
				var tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
				sortinglayer = tagManager.FindProperty("m_SortingLayers");
			}
			return sortinglayer;
		}
	}

	private List<string> AllSortingLayer
	{
		get{
			var layerNameList = new List<string>();
			for(int i=0; i<SortingLayer.arraySize; i++)
			{
				var tag = SortingLayer.GetArrayElementAtIndex(i);
				layerNameList.Add( tag.displayName );
			}
			return layerNameList;
		}
	}
}
