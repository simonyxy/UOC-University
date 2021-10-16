using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


//所有需要让玩家强制移动的操作都需要把player设置成这个状态，不让玩家进行操作，只有动画机响应
public class StplayerStory : IState
{
    private readonly playerCtl _playerIns;
    // private static float speed = 10f;
    private Rigidbody2D _rb;
    private Transform _tf;
    private Animator _anim;
    private FT _ft;

    private GameObject objCurStory;
    private List<int> curStoryList;
    private GameObject objMoveFather;
    private List<GameObject> objsMovePos;


    private Vector2 V2aivel;
    private IAstarAI _aipath;
    public StplayerStory(playerCtl playerIns,Rigidbody2D rb,Animator anim,FT ft,IAstarAI aipath)
    {
        _playerIns = playerIns;
        _rb = rb;
        _anim = anim;
        _ft = ft;
        _aipath  = aipath;
    }

    void IState.Tick(){

        if(_aipath.reachedEndOfPath)
        _playerIns.SetAnimatorMovement(Vector2.zero);

        if(_aipath.canMove)
        ChangeAnimByAIPath();
        else{
            _playerIns.SetAnimatorMovement(Vector2.zero);
        }
    }

    //通过AIpath中的速度去改变角色动画
    void ChangeAnimByAIPath(){

        V2aivel = (Vector2)_aipath.velocity.normalized;
        if(Math.Abs(V2aivel.x) > Math.Abs(V2aivel.y))
        {
            //设置动画为
            V2aivel.y = 0 ;
        }
        else{
            V2aivel.x = 0 ;
        }
        _playerIns.SetAnimatorMovement(V2aivel);
    }


    void IState.OnEnter(){

        objCurStory = StoryMgr.instance.objCurStory ; //获得物体
        curStoryList = StoryMgr.instance.curStoryMoveList;//获得当前故事的动作要素
        
        SEventSystem.EventIns.STORY_END_TRIGGER.AddListener(EndStory);
    }
    
    //1-22
    void IState.TiggerEvent(string Sevent){
        
        if(Sevent == "NORMAL_MOVE_TO_POSITION")
        {
            setMovePosition();
        }
    }

    void IState.OnExit()
    {
        SEventSystem.EventIns.STORY_END_TRIGGER.RemoveListener(EndStory);
    }
    
    private Vector2 desPosition; //移动目标
    void setMovePosition(){

        desPosition = StoryMgr.instance.GetCurMovePos();
        _playerIns.normalMoveToPosition(desPosition,()=>{});//传空方法？
    }

    
    //角色跳出故事
    void EndStory(){
        _playerIns._TigeerSTEvent("OUTOF_STORY");
    }
}  
