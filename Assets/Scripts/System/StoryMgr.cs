using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StoryMgr : MonoBehaviour
{
    static StoryMgr _instance;
    public static StoryMgr instance
    {
        get => _instance ??(_instance = new StoryMgr());
        set => _instance = value;
    }

    //init other
    private FT ft;
    private SEventSystem SESins;
    
    //dialog object
    public string sroty0ContentRole = "player-npc";
    //  0 -move ， 1- talk ,2-multmove， 3-移动到一半还没移动完，进入下一个触发（2个position）
    public List<int> story0Move = new List<int>(){0,0,1,1,0,0,1,1,1};
    public List<string> stroy0Role = new List<string>(){ "player-left","player-left","player-left","npc-right","npc-right","player-left","player-left","npc-right","player-left"};
    
    //dialog p
    public List<string> stroy1Dialog  = new List<string>()
    {
        "你好",
        "嗯你好，我是你的舍友卷哥。",
        "卷哥你好，你是哪里人啊。",
        "俺是东百的",
        "东百的都要死"
    };

    public int curTIndex = 0;        //用來維護當前事件id
    public int curMoveID = 0;        //用來維護移動下標
    public int curDialogID = 0 ;     //用來維護故事下標
    public Dictionary<int,Vector2> PosList = new Dictionary<int , Vector2>() ;         // 剧情位置
    public List<string> curDialogList  = new List<string>();          //
    public List<int> curStoryMoveList;   //当前剧情列表
    // private List<UnityEvent> Ecol = new List<UnityEvent>();  //
    
    public int curStoryID ;
    public GameObject objCurStory;
    public List<string> curRoleList;

    public void Init(){
        
        ft = FT.ft;
        // Ecol.Add(SEventSystem.EventIns.STORY_0);
    }


    public void TriggerEvent(int id,GameObject objTrigger)
    {
    //    if(null!= Ecol[id])
    //    {

        //    Ecol[id].Invoke();          //triggerEvent
           curStoryID = id;            //save stroy id
           objCurStory = objTrigger;   //trigger object
           curStoryMoveList = story0Move;  //
           curRoleList  = stroy0Role; 
           BeginStory();
    //    }
    }

    //强制劇情開始
    void BeginStory(){

       curTIndex = 0 ;
       curMoveID = 0 ;
       curDialogID = 0;
       curDialogList = stroy1Dialog;
       GetAllPositionAndDiaglog();
       InvokeTriggerID(curTIndex);
    }

    public void GetAllPositionAndDiaglog(){

       int posObjsNum = objCurStory.transform.childCount;
       for (int i = 0 ;i < posObjsNum ; i++)
       {
           GameObject temp =  objCurStory.transform.GetChild(i).gameObject;
           int poses = temp.transform.childCount;
           for (int j = 0 ; j < poses ; j ++)
           {
               int.TryParse(temp.transform.GetChild(j).name , out int posID);
               PosList.Add(posID , temp.transform.GetChild(j).position);
           }
       }
    }


    //執行當前事件
    public void InvokeTriggerID(int curTIndex){

       if(curTIndex > curStoryMoveList.Count - 1 )
       {
           string[] roles = Regex.Split(sroty0ContentRole,"-");
           for (int i = 0 ; i < roles.Length;i++)
           {
               GameObject roleObj ;
               ft._MStrToObj.TryGetValue(roles[i],out roleObj);
               roleObj.GetComponent<playerCtl>()?._TigeerSTEvent("OUTOF_STORY"); 
           }
           UIMgr.instance.DiaLogViewEx.HideDiaLogView();
           return;
       }
       int moveType = curStoryMoveList[curTIndex];
       string[] str2 = Regex.Split(curRoleList[curTIndex],"-");

       string moveRole = str2[0];
       string diaLogPos= str2[1];
       //在_MStrToObj中通过moveRole获得物体
       if(moveType == 0 )
       {   
           if(curTIndex != 0 && curStoryMoveList[curTIndex -1] == 1)
           {
               UIMgr.instance.DiaLogViewEx.HideDiaLogView();
           }
           if(moveRole == "player")
           {
               GameObject playerObj ;
               ft._MStrToObj.TryGetValue(moveRole,out playerObj);
               playerObj.GetComponent<playerCtl>()._TigeerSTEvent("NORMAL_MOVE_TO_POSITION"); 
           } 
           else if(moveRole == "npc")
           {
               GameObject npcObj ;
               ft._MStrToObj.TryGetValue(moveRole,out npcObj);
               npcObj.GetComponent<NpcCtl>()._TigeerSTEvent("NORMAL_MOVE_TO_POSITION"); 
           } 
       }
       if(moveType == 1)
       {
           if(curDialogID <=  stroy1Dialog.Count)
           {
               UIMgr.instance.DiaLogViewEx.ShowDiaLogWithType(moveRole,diaLogPos,stroy1Dialog[curDialogID],2);
               curDialogID++;
           }
       }
    }   


    public void ForwardTriggerList(){

       curTIndex ++ ;
       InvokeTriggerID(curTIndex);
    } 

    public Vector2 GetCurMovePos(){

        PosList.TryGetValue(curMoveID,out Vector2 curPos);
        curMoveID++;
        return curPos;
    }
}