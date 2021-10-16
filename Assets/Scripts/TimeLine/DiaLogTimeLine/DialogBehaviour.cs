using System.Linq.Expressions;
using UnityEngine.Playables;
using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogBehaviour :PlayableBehaviour
{
    public DialogView DV;
    // public Vector3 Destination_position;
    public string rolename ; 
    public string position;
    public string dialog;
    [HideInInspector]
    public bool isGraphBegin = false;
    public bool isFinishDialog;
    public PlayableGraph playableGraph;
    private bool isClickBegin = false;

    public override void OnBehaviourPlay(Playable playable, FrameData info )
    {
        // Debug.LogError("OnBehaviourPlay");
        if(!isClickBegin)
        {
            isClickBegin = true;
        }

        if(FT.ft == null)
        return ;

        DV = UIMgr.instance.DiaLogViewEx;
        DV.ShowDiaLog(rolename, position, dialog);
        // isBehaviorBegin = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {   
        
        // Debug.LogError("OnBehaviourPAUSE");
        if(isClickBegin)
        playableGraph.Stop();

    }

    
    public override void OnPlayableCreate(Playable playable)
    {
        SEventSystem.EventIns.DIALOG_TIMELINE_NORMAL_CLICK.AddListener(__hideOrPlay);
        Debug.LogError("OnPlayableCreate");
    }
    public override void OnPlayableDestroy(Playable playable)
    {
        
        Debug.LogError("OnPlayableDestroy");
        SEventSystem.EventIns.DIALOG_TIMELINE_NORMAL_CLICK.RemoveListener(__hideOrPlay);
        UIMgr.instance.SetSrotyRunning(false);
        SEventSystem.EventIns.STORY_END_TRIGGER.Invoke();
    }

    public override void OnGraphStop(Playable playable){
        
        // Debug.LogError("OnGraphStop");
        isGraphBegin = false ;
    }
    
    
    public override void OnGraphStart(Playable playable)
    {
        
        if(SEventSystem.EventIns)
        SEventSystem.EventIns.BEGIN_STORY.Invoke();
        // Debug.LogError("OnGraphStart");
        isGraphBegin = true ;
    }
    public void __hideOrPlay(){

        
        // UIMgr.instance.isRuningStroy = true;
        UIMgr.instance.SetSrotyRunning(true);
        if (isFinishDialog)
        {
            
            DV.HideDiaLogView();
            return;
        }
        else
        {
            
        }
        playableGraph.Play();
    }
}