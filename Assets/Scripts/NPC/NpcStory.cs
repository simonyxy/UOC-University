using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NpcStory : IState
{
    private readonly NpcCtl _npcIns;
    private Rigidbody2D _rb;
    private Transform _tf;
    private Animator _anim;
    private FT _ft;

    // private GameObject objCurStory;
    private GameObject objMoveFather;
    private List<GameObject> objsMovePos;
    private GameObject npcObj;

    public Vector2 desPos; //移动目标
    public NpcStory(NpcCtl npcIns,FT ft)
    {
        _npcIns = npcIns;
        _ft = ft;
    }

    void IState.Tick(){
        if(_npcIns.isCodeMove) 
        {
            _npcIns.MoveToDirBy_Code();
        }
    }


    void IState.OnEnter(){

        npcObj = _npcIns.gameObject;       
        FT.ft._MStrToObj.Add("npc",npcObj); 
    }
    
    
    void IState.TiggerEvent(string Sevent){
        
        if(Sevent == "NORMAL_MOVE_TO_POSITION")
        {
            normalMoveToPosition();
        }
    }

    // void IState.TiggerEvent(string Sevent , Vector2 positon){
    //     if(Sevent == "NORMAL_MOVE_TO_POSITION")
    //     {
    //         normalMoveToPosition(positon);
    //     }
    // }

    void IState.OnExit()
    {
    }

    void normalMoveToPosition(){

        desPos = StoryMgr.instance.GetCurMovePos();
        _npcIns.isCodeMove  = true;
    }
}
