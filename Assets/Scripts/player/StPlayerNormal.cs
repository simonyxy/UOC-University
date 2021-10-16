using System;
using UnityEngine;
using Pathfinding;

public class StPlayerNormal : IState
{
    private readonly playerCtl _playerIns;
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
        _playerIns = playerIns;
        _rb = rb;
        _anim = anim;
        _aipath = aipath; 
                                                        
    }

    void IState.Tick(){
        
        _playerIns.HorizontalNum = Input.GetAxis("Horizontal");
        _playerIns.VerticalNum = Input.GetAxis("Vertical");
        if(_playerIns.isSelectState)
        {

        }
        else
        {
            checkClcikMouse();
            
            if(_playerIns.HorizontalNum ==  0  && _playerIns.VerticalNum == 0 )
            {
                if(_aipath.reachedEndOfPath)
                _playerIns.SetAnimatorMovement(Vector2.zero);

                if(_aipath.canMove)
                ChangeAnimByAIPath();
                else{
                    _playerIns.SetAnimatorMovement(Vector2.zero);
                }
            }
            else
            {  
                _playerIns.KeyCodeMove();
            }
            _playerIns.CheckSelectPanelClick();
        }
    }

    void checkClcikMouse(){

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


    public void OnDrawGizmos() {
        boxCenter = (Vector2)_playerIns.transform.position ;
        tempHitNpc = Physics2D.OverlapCircle(boxCenter, 1, _playerIns.npcLayer);
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

    //开始寻路
    void CheckClick(int input ){

        if (input == 0)
        {   

            _playerIns.CheckLeftMouseClick();
        }
        else if(input == 1)
        {
            
            //鼠标右键点击
            _playerIns.CheckRightMouseClick();
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
    }
    

    void IState.TiggerEvent(string Sevent){
        
    }

    void IState.OnExit()
    {
    }
}