using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class AStarClick :PlayableAsset,ITimelineClipAsset
{
    // public AStarBehaviour template = new AStarBehaviour();
    public Vector3 destination_position;
    public bool isFinishStory;
    
    public ClipCaps clipCaps{
        get {return ClipCaps.All;}
    }

    public override Playable CreatePlayable(PlayableGraph graph,GameObject owner)
    {
        //创建的时候都需要进入故事模式
        UIMgr.instance?.SetSrotyRunning(true);
        var playable = ScriptPlayable<AStarBehaviour>.Create(graph);
        var AIBehavior = playable.GetBehaviour();
        AIBehavior.Destination_position = destination_position;
        AIBehavior.playableGraph = graph;
        AIBehavior.isFinishStory = isFinishStory;
        AIBehavior.OnCreat();
        return playable;
    }
}