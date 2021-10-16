using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MutiDialogClick :PlayableAsset,ITimelineClipAsset
{
    public string Arolename;
    public string Qrolename;
    public string question ;
    public string[] replys = new string[4];
    public string[] answersText = new string[4];    //按钮的文字
    public string[] ansewersReply = new string[4];  //选完内容后回复
    public bool isFinishDialog = false;
    public ClipCaps clipCaps{
        get {return ClipCaps.All;}
    }

    public override Playable CreatePlayable(PlayableGraph graph,GameObject owner)
    {
        var playable = ScriptPlayable<MutiDialogBehaviour>.Create(graph);
        var MutiDialogBehavior = playable.GetBehaviour();
        MutiDialogBehavior.question = question;
        MutiDialogBehavior.replys = replys;
        MutiDialogBehavior.answersText = answersText;
        MutiDialogBehavior.ansewersReply = ansewersReply;
        MutiDialogBehavior.Arolename = Arolename;
        MutiDialogBehavior.Qrolename = Qrolename;
        MutiDialogBehavior.playableGraph = graph;
        MutiDialogBehavior.isFinishDialog =  isFinishDialog;
        return playable;
    }
}