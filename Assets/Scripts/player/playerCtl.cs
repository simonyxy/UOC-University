
using UnityEngine;
using System;
using Pathfinding;        
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class playerCtl : MonoBehaviour
{   
    public readonly string name = "player";
    public GameObject objPlayer;
    private static playerCtl _playerInstance;
    public static playerCtl PlayerIns
    {
        get{
            if(_playerInstance == null)
            {
                _playerInstance = new playerCtl();
                return _playerInstance;
            }  
            else
            {
                return _playerInstance;
            }
        }
    }
    
    private FT ft;
    public StateMachine _stateMachine;
    private Animator anim;

    //Physics
    private Rigidbody2D rb;

    private float horizontalNum;
    private float verticalNum;
    public float HorizontalNum {
        get{return horizontalNum;}
        set{horizontalNum = value;}
    }
    public float VerticalNum {
        get{return verticalNum;}
        set{verticalNum = value;}
    }
    private float speed = 4f;
    private Vector2 direction;

    //TODO:添加playerUICtl
    public playerUICtl _pUICtl;
    public bool isSelectState = false;
    public bool canUIClick = true;
    
    private  struct SelectType
    {
        public static int In = 1;
        public static int Out =2 ;
    };

    //StateMachine
    StPlayerNormal Stnormal;
    StplayerStory Ststory;
    StPlayerUIOpen StUIOpen;

    //ai path 
    public IAstarAI aipath;
    public Transform targetPos;
    
    //LayerMask
    public LayerMask npcLayer;
    public LayerMask MapUILayer;

    private void Awake() {
        
        _playerInstance = this;
        objPlayer = this.gameObject;
        ft = FT.ft;

        //Component
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        aipath = this.GetComponent<IAstarAI>();
        _pUICtl = gameObject.AddComponent<playerUICtl>();       //玩家ui控制

        npcLayer = LayerMask.GetMask("Npc");//layer
        MapUILayer = LayerMask.GetMask("MapUI");//layer

        //init StateMachine
        _stateMachine = new StateMachine();
        Stnormal = new StPlayerNormal(this, rb,anim,aipath);
        Ststory = new StplayerStory(this, rb,anim , ft,aipath);
        StUIOpen = new StPlayerUIOpen();

        _stateMachine.SetState(Stnormal);
    }


    void Start(){

        ft._MStrToObj.Add("player",objPlayer);
    }

    private void OnEnable() {

        SEventSystem.EventIns.BEGIN_STORY.AddListener(beginStory);
        SEventSystem.EventIns.IN_TIMELINE_STORY.AddListener(gameStart);
    }

    private void OnDisable() {
        
        SEventSystem.EventIns.BEGIN_STORY.RemoveListener(beginStory);
        SEventSystem.EventIns.IN_TIMELINE_STORY.RemoveListener(gameStart);
    }

    private void beginStory(){

Debug.LogError(" set player state");
        _stateMachine.SetState(Ststory);
    }

    private void gameStart(){

        _stateMachine.SetState(Ststory);
    }

    
    //鼠标右键点击
    public void CheckRightMouseClick(){

        Vector3 p = ft.mainCam.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0;
        RaycastHit2D hit = new RaycastHit2D();
        hit = Physics2D.Raycast(new Vector2(p.x,p.y),Vector3.forward,5.0f,npcLayer) ;
        if(hit)
        {
            int layer = hit.collider.gameObject.layer;
            UIMgr.instance.InitNPCInfoButtonGroup(hit.collider,p);
        }
    } 


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool IsClickUILayer(){

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        raycastResults.Clear();
        EventSystem.current.RaycastAll(eventData,raycastResults); //
        //return when click UI
        if(raycastResults.Count != 0)
        {
            return true;
        }
        return false;
    }
    

    //鼠标左键点击
    [HideInInspector]public Vector2  tempAIvel;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    public void CheckLeftMouseClick(){

        //除了普通状态不让检测点击
        if (_stateMachine.CurrentState != Stnormal)
            return;

        Vector2 p = ft.mainCam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits ;
        // hit = Physics2D.Raycast(new Vector2(p.x,p.y),Vector3.forward,5.0f,MapUILayer) ; //点击到mapUI
        hits =  Physics2D.RaycastAll(new Vector2(p.x,p.y),Vector2.down);
        if(hits.Length != 0 )
        {
            for(int i = 0 ; i< hits.Length ; i ++ )
            {
                
            }
        }
        
        //if UI Click return 
        if(IsClickUILayer()) 
            return;
       
        //if map UI click reurn 
        if(UIMgr.instance.isOpenMapUI)
            return;

        aipath.canMove = true; 
        aipath.destination = ft.mainCam.ScreenToWorldPoint(Input.mousePosition);
        aipath.SearchPath();

        tempAIvel = (Vector2)aipath.velocity;
    } 


    public void OnDrawGizmos() {

        Stnormal.OnDrawGizmos();
    }


    // Update is called once per frame
    private void FixedUpdate() {
        
        _stateMachine.Tick();
    }


#region move code
    public void KeyCodeMove(){

        direction.Set(horizontalNum,verticalNum);
        SetAnimatorMovement(direction);
        if(direction == Vector2.zero) 
        {
            return;
        }
        DisableNavAI();
        transform.Translate(direction * speed * Time.deltaTime);
    }


    //查看是否在寻路
    public void DisableNavAI(){

        if(aipath.canMove){
            
            aipath.canMove = false;
        }
    }
#endregion


    public void SetAnimatorMovement(Vector2 direction){

        anim.SetFloat("xDir", direction.x );
        anim.SetFloat("yDir", direction.y );
        if(direction != Vector2.zero){
            anim.SetLayerWeight(1,1);
        }
        else{
            anim.SetLayerWeight(1,0);
        }
    }


    public void CheckSelectPanelClick(){

        if(canUIClick && !isSelectState)
        {
            if(Input.GetKey(KeyCode.K)){

                _pUICtl.StSelectState(SelectType.In);
            }
        }
    }
    

    //触发状态机中的事件
    public void _TigeerSTEvent(string Sevent){

        if( Sevent == "OUTOF_STORY")
        {
            _stateMachine.SetState(Stnormal);
            return ;
        }
        else if(Sevent == "OPEM_VIEW" )
        {
            _stateMachine.SetState(StUIOpen);
            return ;
        }
        else if(Sevent == "CLOSE_VIEW")
        {
            _stateMachine.SetState(Stnormal);
            return ;
        }
    }


//------------------------------迫使玩家移动到某个地方-----------------------------------------
    private Vector2 desPosition; //移动目标
    private bool isSetMoveToPosition = false;
    public void normalMoveToPosition(Vector2 desPosition, Action cb){
        
        _stateMachine.SetState(Ststory);
        this.desPosition = desPosition;
        isSetMoveToPosition = true;
        ft.sTimer.New("startMoveToPosition",startMoveToPosition,-1f,true);
    }

    public void startMoveToPosition()
    {
        if(isSetMoveToPosition) 
        {
           Vector2 beforPos = transform.position;
           //移動
           if(Math.Abs(beforPos.x - desPosition.x)>=1 )
           {
               //移動x
               float x =  (desPosition.x  - beforPos.x) >0? 1:-1;
               MoveByDir(x , 0);
           }
           else
           {
               if(Math.Abs(beforPos.y - desPosition.y)>=1 )
               {
                   //移動y
                   float y =  (desPosition.y  - beforPos.y) >0? 1:-1;
                   MoveByDir(0 , y);
               }
               else{
                    JumpOutMoveToPosition();
               }
           }
        }
    }

    
    public void MoveByDir(float dirx,float diry)
    {

        direction.Set(dirx , diry);
        SetAnimatorMovement(direction);
        if(direction == Vector2.zero) 
        {
            return;
        }
        DisableNavAI();  //--这个方法可以只调用一次，不用每次都调用
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    void JumpOutMoveToPosition(){

        desPosition = default;
        SetAnimatorMovement(Vector2.zero);
        isSetMoveToPosition = false;
        ft.sTimer.RemoveEvent("startMoveToPosition");
        _stateMachine.SetState(Stnormal);
    }
}
//---------------------------------------------------------------------------------------