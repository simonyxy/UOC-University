using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public abstract class NpcCtl : MonoBehaviour
{
    
    public struct NpcMoveType 
    {
        public static int Idle = 0 ;
        public static int Move = 1 ;
        public static int Wait = 2 ;
    }
    [HideInInspector] public int npcCurState = NpcMoveType.Wait;
    float waitTimeTotal = 2f;
    float waitTimeColder = 0f;
    float idleTimeTotal = 2f;
    float idleTimeColder =0f; 
    float distance;
    float moveDis = 0 ; 
    private float speedNPC = 0.1f ;

    private Vector2 V2moveDir;
    private Vector2 distanceV2;
    private Vector2 pos;
    private Vector2 desV2;
    
    //init other
    private playerCtl _playerInstance;
    public StateMachine _stateMachine;
    private SEventSystem SESins;
    [HideInInspector]public FT ft;
    [HideInInspector]public static NpcCtl npcCtl;
    public abstract string  npcName{
        get;set;
    }
    //physics
    private BoxCollider2D colMoveArea;
    private BoxCollider2D colNpc;
    private Rigidbody2D rigSel;
    [HideInInspector]public bool isCodeMove;

    //ditects
    private CircleCollider2D colMidRangeReactArea;
    private CircleCollider2D nearRangeReactArea;
    private float midRangeReactRadius;
    private float nearRangeReactRadius;
    private Bounds boundsMove;


    //dialog
    private GameObject imgDialog;
    private TextMesh txtDialog;

    private GameObject npcObj;
    [HideInInspector]public Vector2 direction;
    
    //stateMachine
    public NpcStory Ststory;
    public NpcNormal normal;

    public abstract bool isRandomMove
    {get;set;} 


    protected virtual void Awake() {

        //init other
        ft = FT.ft;
        SESins = SEventSystem.EventIns;
        //phyics
        colNpc = GetComponent<BoxCollider2D>();
        rigSel = GetComponent<Rigidbody2D>();
        //ditect
        colMoveArea = transform.Find("moveArea").GetComponent<BoxCollider2D>();
        colMidRangeReactArea = transform.Find("midRangeReactArea").GetComponent<CircleCollider2D>();
        nearRangeReactArea   = transform.Find("nearRangeReactArea").GetComponent<CircleCollider2D>();
        midRangeReactRadius = colMidRangeReactArea.radius;
        nearRangeReactRadius = nearRangeReactArea.radius;
        boundsMove = colMoveArea.bounds;
        colMidRangeReactArea.enabled =false;
        colMoveArea.enabled = false;
        //Dialog
        imgDialog   = transform.Find("imgDialog").gameObject ;
        // void At(IState to , IState from ,Func<bool> condition) => _stateMachine.AddTransition(to,from,condition);
        //state Machine
        _stateMachine = new StateMachine();
        normal  = new NpcNormal(this,colNpc,boundsMove);
        Ststory = new NpcStory(this, ft);
        _stateMachine.SetState(normal);
        
        txtDialog = imgDialog.transform.Find("txtDialog").GetComponent<TextMesh>();
        
    }

    
    //子类可调用的设置随机移动的状态参数
    public void SetRandomMoveInfo(float waitTimeTotal,float idleTimeTotal,float speedNPC)
    {  
       
        this.waitTimeTotal = waitTimeTotal;
        this.idleTimeTotal = idleTimeTotal;
        this.speedNPC = speedNPC;
    }


    private void Start() {
            
        _playerInstance = playerCtl.PlayerIns;
    }

    
    private void OnEnable() {
        
        //当玩家选中按钮的时候。触发事件trigger
        _stateMachine.CurrentState.OnEnter();
        SESins.PLAYER_CHOSE_UP.AddListener(__playerSelectUp);
        SESins.PLAYER_CHOSE_DOWN.AddListener(__playerSelectDown);
        SESins.PLAYER_CHOSE_LEFT.AddListener(__playerSelectLeft);
        SESins.PLAYER_CHOSE_RIGHT.AddListener(__playerSelectRight);
        // SESins.PLAYER_CHOSE_RIGHT.AddListener(__playerSelectRight);
        SESins.PLAYER_HIT_NPC.AddListener(CheckPlayerInTheNearArea);
        SESins.BEGIN_STORY.AddListener(setStoryState);
        SEventSystem.EventIns.GAME_LOAD_END.AddListener(Start);
    }

    private void OnDisable() {
        
        _stateMachine.CurrentState.OnExit();
        SESins.PLAYER_CHOSE_UP.RemoveListener(__playerSelectUp);
        SESins.PLAYER_CHOSE_DOWN.RemoveListener(__playerSelectDown);
        SESins.PLAYER_CHOSE_LEFT.RemoveListener(__playerSelectLeft);
        SESins.PLAYER_CHOSE_RIGHT.RemoveListener(__playerSelectRight);
        // SESins.PLAYER_CHOSE_RIGHT.RemoveListener(__playerSelectRight);
        SESins.PLAYER_HIT_NPC.RemoveListener(CheckPlayerInTheNearArea);
        SESins.BEGIN_STORY.RemoveListener(setStoryState);
        SEventSystem.EventIns.GAME_LOAD_END.RemoveListener(Start);
    }
    

    private void setStoryState(){

        _stateMachine.SetState(Ststory);
    }


    private void FixedUpdate() {
        
        _stateMachine.Tick();    
    }


    //可重写的方法
    public virtual void __playerSelectUp(){
        CheckPlayerInTheMidArea("PLAYER_CHOSE_UP");
    }
    
    //可重写的方法
    public virtual void __playerSelectLeft(){
        CheckPlayerInTheMidArea("PLAYER_CHOSE_LEFT");
    }

    //可重写的方法
    public virtual void __playerSelectRight(){
        CheckPlayerInTheMidArea("PLAYER_CHOSE_RIGHT");
    }

    //可重写的方法
    public virtual void __playerSelectDown(){
        CheckPlayerInTheMidArea("PLAYER_CHOSE_DOWN");
    }


    //检查玩家是否在中距离检测范围中
    public virtual void CheckPlayerInTheMidArea(string Sevent){
        
        float abs =  (transform.position  -  _playerInstance.transform.position).magnitude;
        if(  abs < midRangeReactRadius )
        {
            _stateMachine.CurrentState.TiggerEvent(Sevent);
        }
    }


     public virtual void CheckPlayerInTheNearArea(){
        
        _stateMachine.CurrentState.TiggerEvent("PLAYER_HIT_NPC");
    }


    public void _TigeerSTEvent(string Sevent){

        _stateMachine.CurrentState.TiggerEvent(Sevent);
    }

    public void ShowDiaLog(){

        imgDialog.SetActive(true);
        ft.sTimer.New("HideDiaLog",HideDiaLog,3f);
    }


    void HideDiaLog(){

        imgDialog.SetActive(false);
    }


    public void MoveByDir(float dirx,float diry){

        direction.Set(dirx , diry);
        transform.Translate(direction * 1 * Time.deltaTime);
    }


    //Gizmos
    private void OnDrawGizmos() {

        Gizmos.color = Color.yellow;
        if(colMoveArea){
            Gizmos.DrawWireCube(boundsMove.center,boundsMove.size);
        }  

        if(midRangeReactRadius != 0 )
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position,midRangeReactRadius);
        }
        
        if(nearRangeReactRadius != 0 )
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,midRangeReactRadius);
        }
    }



    public virtual void _sayHiBtnTrigger(){
        
    }

    ///////////Code Move///////////////////

    //代码控制移动
    public virtual void MoveToDirBy_Code(){
        Vector2 beforPos = transform.position;
        //移動
        if(Math.Abs(beforPos.x - Ststory.desPos.x)>=1 )
        {
            //移動x
            float x =  (Ststory.desPos.x  - beforPos.x) >0? 2:-2;
            MoveByDir(x , 0);
        }
        else
        {
            if(Math.Abs(beforPos.y - Ststory.desPos.y)>=1 )
            {
                //移動y
                float y =  (Ststory.desPos.y  - beforPos.y) >0? 2:-2;
                MoveByDir(0 , y);
            }
            else{
                JumpOutMove_Code();
            }
        }
    }

    public virtual void JumpOutMove_Code(){

       //退出移動
       isCodeMove = false;
       Ststory.desPos = default;
       // _npcIns.SetAnimatorMovement(Vector2.zero);
       StoryMgr.instance.ForwardTriggerList();
    }

    /////////////////////////////


    ////////////stnormal//code Auto Move//////////////////////
    public virtual void _StartMoveByType(int type)
    {
        if(!isRandomMove) return; 
        if(type  == NpcMoveType.Move)
        {
            StMove();
        }
        else if(type == NpcMoveType.Idle)
        {
            StIdle();
        }
        else if(type == NpcMoveType.Wait)
        {
            StWait();
        }
    }

    void StMove(){

        //移动
        pos = rigSel.transform.position;
        if(distanceV2.magnitude <moveDis)
        {
            StopMove();
        }
        V2moveDir = distanceV2.normalized * speedNPC;
        moveDis += V2moveDir.magnitude;
        rigSel.MovePosition(pos - V2moveDir );
    }

    void StWait(){

        waitTimeColder += Time.fixedDeltaTime;
        if(waitTimeColder > waitTimeTotal)
        {
            waitTimeColder = 0;
            BeginCheckMoveMent();
        }
    }

    void StIdle(){

        idleTimeColder += Time.fixedDeltaTime;
        if(idleTimeColder > idleTimeTotal)
        {
            idleTimeColder = 0;
            ChangeState(NpcMoveType.Wait);
        }
    }

    void ChangeState(int state)
    {
        npcCurState = state;
    }
    
    public virtual void BeginCheckMoveMent()
    {

        desV2 = new Vector2(UnityEngine.Random.Range(boundsMove.min.x , boundsMove.max.x),UnityEngine.Random.Range(boundsMove.min.y,boundsMove.max.y));
        //计算距离
        Vector2 npcPos = rigSel.transform.position; // 可能可以优化下
        distanceV2 = npcPos - desV2;
        if(distanceV2.magnitude >2)
        {
            ChangeState(NpcMoveType.Move);
        }
        else{
            ChangeState(NpcMoveType.Idle);
        }
    }

    void StopMove(){

        moveDis = 0 ; 
        ChangeState(NpcMoveType.Wait);
        return;
    }


    #region  事件开始
    public virtual void CheckShowDiagNear(){

        //TODO:"in检测,进来看这块代码，需要继续完善掉的"
        string diastr = "别tm离我这么近";
        // imgDialog.SetActive(true);
        ShowDiaLog();
        txtDialog.text = diastr;
        PlayerConfig.instance._MinsPlayerHealth(10);
        PlayerConfig.instance._AddPlayerSad(10);
    }

    
    //对话dialog 
    private ConfigDemoNpc diagList = ConfigDemoNpc.Instance; 
    //通过事件，检查距离和显示对话
    public virtual void CheckShowDiagUp(){
        
        //TODO: need 完善
        string diastr = diagList.GetDiaLogByTypeAndFriend(0,10);
        // imgDialog.SetActive(true);
        ShowDiaLog();
        txtDialog.text = diastr;

        //在这触写具体的扣除生命值和抑郁值的方法
        PlayerConfig.instance._AddPlayerHealth(10);
        PlayerConfig.instance._MinsPlayerSad(10);
    }
    #endregion
    ///////////////////////////////////////////////
    public virtual void FroceNpcBeingClick()
    {
        
        _stateMachine.SetState(Ststory);
    }

    ///////////////

}
