using UnityEngine.Playables;
using Pathfinding;
using UnityEngine;

// using UnityEngine.Events;

[System.Serializable]
public class AStarBehaviour :PlayableBehaviour
{
    public IAstarAI AI;
    public Vector3 Destination_position;
    public bool isFinishStory;
    public bool isBegin = true;
    public PlayableGraph playableGraph;

    public override void ProcessFrame(Playable playable , FrameData info ,object playerData)
    {
        
        SEventSystem.EventIns.IN_TIMELINE_STORY.Invoke();
        if(isBegin)
            {
            GameObject temp = playerData as GameObject ; 
            if(temp)
            {
                AI = temp.GetComponent<IAstarAI>();
                if(!AI.canMove)
                {
                    AI.canMove = true;
                }
                AI.destination = Destination_position;
                AI.SearchPath();
                isBegin = false;
            }
        }
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info )
    {
        if(!UIMgr.instance.isRuningStroy)
        {
            playableGraph.Stop();
        }
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
        if(isFinishStory)
        {
            SEventSystem.EventIns.STORY_END_TRIGGER.Invoke();
        }
    }

    
    public override void OnPlayableDestroy(Playable playable)
    {
        
        SEventSystem.EventIns.DIALOG_TIMELINE_NORMAL_CLICK.RemoveListener(__hideOrPlay);
        UIMgr.instance.SetSrotyRunning(false);
    }

    
    public void __hideOrPlay(){

        // UIMgr.instance.isRuningStroy = true;
        UIMgr.instance.SetSrotyRunning(true);
        if (isFinishStory)
        {
            // DV.HideDiaLogView();
            return;
        }
        else
        {
            
        }
        playableGraph.Play();
    }
    public void OnCreat()
    {
        
        UIMgr.instance.SetSrotyRunning(true);
        playableGraph.Play();
    }
}