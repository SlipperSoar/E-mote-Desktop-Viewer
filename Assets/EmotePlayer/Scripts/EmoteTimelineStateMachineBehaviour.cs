using UnityEngine;

public class EmoteTimelineStateMachineBehaviour : StateMachineBehaviour
{
    public string slot;
    public string timelineLabel;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex) {
        var emotePlayer = animator.GetComponent<EmotePlayer>();
        if (emotePlayer == null) {
            Debug.LogWarning("This animator must be attached to EmotePlayer GameObject.", animator);
            return;
        }
        switch (slot) {
        case "": emotePlayer.mainTimelineLabel = timelineLabel; break;
        case "1": emotePlayer.diffTimelineSlot1 = timelineLabel; break;
        case "2": emotePlayer.diffTimelineSlot2 = timelineLabel; break;
        case "3": emotePlayer.diffTimelineSlot3 = timelineLabel; break;
        case "4": emotePlayer.diffTimelineSlot4 = timelineLabel; break;
        case "5": emotePlayer.diffTimelineSlot5 = timelineLabel; break;
        case "6": emotePlayer.diffTimelineSlot6 = timelineLabel; break;
        }
    }
}