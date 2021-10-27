using System;
using UnityEngine;
using Pathfinding;

public class StPlayerNormal : IState
{
    private readonly playerCtl _player;
    // private static float speed = 1000f;

    private Rigidbody2D _rb;
    private Transform _tf;
    private Animator _anim;
    private IAstarAI _aipath;
    private Vector2 V2aivel;
    private Vector2 boxCenter;
    private Collider2D tempHitNpc;
    
    private bool isClickMouse;
    public StPlayerNormal(playerCtl playerIns,Rigidbody2D rb,Animator anim,IAstarAI aipath)
    {
        _player = playerIns;
        _rb = rb;
        _anim = anim;
        _aipath = aipath; 
                                                        
    }

    void IState.Tick(){

        _player.HorizontalNum = Input.GetAxis("Horizontal");
        _player.VerticalNum = Input.GetAxis("Vertical");
        if(_player.isSelectState)
        {

        }
        else
        {

            checkClickMouse();
            checkInputKey();
            
        }
    }

    void checkClickMouse(){

        if( isClickMouse )
        return; 

        if(Input.GetMouseButton(1))
        {
            CheckClick(1);
            setClick();
        }
        else if(Input.GetMouseButton(0))
        {
            CheckClick(0);
            setClick();
        }  
    }

    void setClick(){
        isClickMouse = true;
        FT.ft.sTimer.New("isClickMouse",()=> isClickMouse =false,0.3f);
    }


    /// <summary>
    /// 检测按下按钮
    /// </summary>
    void checkInputKey()
    {

        if (_player.HorizontalNum == 0 && _player.VerticalNum == 0)
        {
            if (_aipath.reachedEndOfPath)
                _player.SetAnimatorMovement(Vector2.zero);

            if (_aipath.canMove)
                ChangeAnimByAIPath();
            else
            {
                _player.SetAnimatorMovement(Vector2.zero);
            }
        }
        else
        {
            _player.KeyCodeMove();
        }
        _player.CheckSelectPanelClick();
    }



    public void OnDrawGizmos() {
        boxCenter = (Vector2)_player.transform.position ;
        tempHitNpc = Physics2D.OverlapCircle(boxCenter, 1, _player.npcLayer);
        if ( tempHitNpc != null)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        Gizmos.DrawWireSphere(boxCenter,1);
    }


    /// <summary>
    /// 检测鼠标点击
    /// </summary>
    void CheckClick(int input ){

        if (input == 0)
        {

            _player.CheckLeftMouseClick();
        }
        else if(input == 1)
        {

            //鼠标右键点击
            _player.CheckRightMouseClick();
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
        _player.SetAnimatorMovement(V2aivel);
    }


    void IState.OnEnter(){
    }
    

    void IState.TiggerEvent(string Sevent){
        
    }

    void IState.OnExit()
    {
    }
}