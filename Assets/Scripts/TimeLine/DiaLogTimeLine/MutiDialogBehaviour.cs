using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;
//qus:
//ans 1.2.3.4  setint selectans  记录选择
//ans template = " xxxxx ; xxxxx ;xxxxx"   //分开显示
//qus2;
//ans2 1.2.3.4 

[System.Serializable]
public class MutiDialogBehaviour :PlayableBehaviour
{
    // public MutiDiaLogView mutiDiaLogView;
    // public Vector3 Destination_position;
    public string Arolename; 
    public string Qrolename;
    public string question;
    public string[] answersText;
    public string[] replys;
    public string[] ansewersReply;
    [HideInInspector]
    public bool isFinishDialog;
    public PlayableGraph playableGraph;
    private bool isClickBegin = false;


    public override void OnBehaviourPlay(Playable playable, FrameData info )
    {

        Debug.LogError("BeginClick");
        if(!isClickBegin)
        {
            isClickBegin = true;
        }

        if(FT.ft == null)
        return ;

        UIMgr.instance.OpenMutiDiaLogView(Qrolename,question,Arolename,answersText,replys,ansewersReply);
    }


    public override void OnBehaviourPause(Playable playable, FrameData info)
    {   
        Debug.LogError("Des");
        if(isClickBegin)
        {
            if( playableGraph.IsValid())
            {

                SEventSystem.EventIns.BEGIN_STORY.Invoke(); //设置玩家和npc等状态
                playableGraph.Stop();
            }
            else
            {
                Debug.LogError("Des33");
                SEventSystem.EventIns.STORY_END_TRIGGER.Invoke();
            }
        }
    }

    public override void OnPlayableCreate(Playable playable)
    {
        SEventSystem.EventIns.MUTIDIALOG_TIMELINE_NORMAL_CLICK.AddListener(MutiDiaLogClipFinish);
    }

    public override void OnPlayableDestroy(Playable playable)
    {
        // if( playableGraph.IsValid())
        // {
        //     SEventSystem.EventIns.MUTIDIALOG_TIMELINE_NORMAL_CLICK.RemoveListener(MutiDiaLogClipFinish);
        // }
    }

    public override void OnGraphStop(Playable playable){

        Debug.LogError("Des3");
    }
    
    
    public override void OnGraphStart(Playable playable)
    {
        SEventSystem.EventIns.BEGIN_STORY.Invoke(); //设置玩家和npc等状态
    }



    public void MutiDiaLogClipFinish(){

        // UIMgr.instance.SetSrotyRunning(true);
        if (isFinishDialog)
        {
            UIMgr.instance.HideMutiDiaLogView();
            // mutiDiaLogView.HideMutiDiaLogView();
            SEventSystem.EventIns.MUTIDIALOG_TIMELINE_NORMAL_CLICK.RemoveListener(MutiDiaLogClipFinish);
            return;
        }
        else
        {
            
        }
        playableGraph.Play();
    }
}