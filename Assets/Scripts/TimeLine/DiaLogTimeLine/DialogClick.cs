using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogClick :PlayableAsset,ITimelineClipAsset
{
    // public AStarBehaviour template = new AStarBehaviour();
    //public Vector3 destination_position;
    public string rolename ;
    public string position ;
    public string dialog;
    public bool isFinishDialog = false;
    public ClipCaps clipCaps{
        get {return ClipCaps.All;}
    }

    public override Playable CreatePlayable(PlayableGraph graph,GameObject owner)
    {
        var playable = ScriptPlayable<DialogBehaviour>.Create(graph);
        var DialogBehavior = playable.GetBehaviour();
        DialogBehavior.dialog = dialog;
        DialogBehavior.position = position;
        DialogBehavior.rolename = rolename ;
        DialogBehavior.playableGraph = graph;
        DialogBehavior.isFinishDialog =  isFinishDialog;
        return playable;
    }
}