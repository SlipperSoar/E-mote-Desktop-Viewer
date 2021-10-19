using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class EmoteAnimatorControllerBuilder
{
    [MenuItem("CONTEXT/Animator/Build Emote Timeline Animator Controller")]
     static void BuildEmoteTimelineAnimatorController(MenuCommand menuCommand) {
         var animator = menuCommand.context as Animator;
         var controller = animator.runtimeAnimatorController as AnimatorController;
         if (controller == null) {
             Debug.LogError("valid AnimatorControler musb be attached to Animator.", animator);
             return;
         }
         var emotePlayer = animator.GetComponent<EmotePlayer>();
         if (emotePlayer == null) {
             Debug.LogError("valid EmotePlayer musb be attached to GameObject.", animator);
             return;
         }
         if (EditorUtility.DisplayDialog("Replace Animator Controller asset?", 
                                         "Are you sure you want to replace an asset?\n\"" + AssetDatabase.GetAssetPath(controller) + "\"",
                                         "Replace",
                                         "Cancel")) {
             var clone = GameObject.Instantiate(animator.gameObject) as GameObject;
             clone.hideFlags = HideFlags.HideAndDontSave;
             emotePlayer = clone.GetComponent<EmotePlayer>();
             BuildAnimatorController(emotePlayer, controller);
             Object.DestroyImmediate(clone);
         }
     }

    static void BuildAnimatorController(EmotePlayer emotePlayer, AnimatorController controller) {
        while (controller.layers.Length > 0)
            controller.RemoveLayer(0);

        var stateLabels = emotePlayer.mainTimelineLabels;
        var layerName = "Main";
        var slot = "";
        BuildAnimatorControllerLayer(controller, layerName, stateLabels, slot);

        stateLabels = emotePlayer.diffTimelineLabels;
        for (int i = 1; i <= 6; i++) {
            layerName = "#" + i.ToString();
            slot = i.ToString();
            BuildAnimatorControllerLayer(controller, layerName, stateLabels, slot);
        }
    }

    static void BuildAnimatorControllerLayer(AnimatorController controller, string layerName, string[] stateLabels, string slot) {
        controller.AddLayer(layerName);
        var layer = controller.layers[controller.layers.Length - 1];
        var stateMachine = layer.stateMachine;

        bool first = true;
        
        foreach (var _stateLabel in stateLabels) {
            string stateLabel, timelineLabel;
            if (first) {
                timelineLabel = "";
                stateLabel = "stop";
                first = false;
            } else {
                timelineLabel = _stateLabel;
                stateLabel = _stateLabel;
            }
            var state = stateMachine.AddState(stateLabel);
            state.speed = 0;
            var behaviour = state.AddStateMachineBehaviour(typeof(EmoteTimelineStateMachineBehaviour)) as EmoteTimelineStateMachineBehaviour;
            behaviour.slot = slot;
            behaviour.timelineLabel = timelineLabel;
            var transition = stateMachine.AddAnyStateTransition(state);
            transition.exitTime = 0;
            transition.duration = 0;
            transition.hasExitTime = true;
            transition.canTransitionToSelf = false;
        }
    }

}