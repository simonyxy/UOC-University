using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏启动脚本，所有的一切的起源，是生命的源泉
public class FT : MonoBehaviour
{
    [HideInInspector]public static FT ft;
    [HideInInspector]public StoryMgr SM;
    [HideInInspector]public STimer sTimer;
    [HideInInspector]public SystemTime ST;
    public GameObject uiRoot;
    [HideInInspector]public GameObject player;
    private TimerManager timerMgr;

    //寻路需要的，  物体名 - 物体列表 
    [HideInInspector]public Dictionary<string , GameObject> _MStrToObj = new Dictionary<string , GameObject>(); 
    [HideInInspector]public Camera mainCam;
    
    ////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////
    [HideInInspector]public int G_day; 

    private void Awake(){
        
        LoadAllThing();
        LoadAllConfig(); 
        LoadAllPlayer();
        
        //下一帧执行，未测试
        ft.sTimer.New("GameLoadEnd",GameLoadEnd,Time.deltaTime);
    }

    private void LoadAllThing()
    {
        ft = this;

        //delay function tool   
        sTimer = gameObject.AddComponent<STimer>() as STimer;
        timerMgr = TimerManager.instance; //唤醒

        //system time
        ST = gameObject.AddComponent<SystemTime>() as SystemTime;

        //manager
        new StoryMgr();
        new UIMgr();
        new NpcMgr();
        new FrustratioMgr();
        StoryMgr.instance.Init();
        UIMgr.instance.init();
        NpcMgr.instance.init();
        FrustratioMgr.instance.init();

        //uiRoot = GameObject.Find("UIRoot").gameObject;
        mainCam = GameObject.Find("mainCemera").GetComponent<Camera>();
        player = GameObject.Find("Player");
    }
    

    //GameBegin loadConfig 其实只是在XXconfig中初始化的过程
    private void LoadAllConfig()
    {    
        //读取游戏数据，当前第几天之类的
        AddNpcConfig();
    }

    //将npcConfig添加到全局
    private void AddNpcConfig(){
        
    }
    
    
    private void GameLoadEnd(){
        
        UIMgr.instance.OpenDayBeginCanvas(1);
        SEventSystem.EventIns.GAME_LOAD_END.Invoke();
    }

    //loadPlayer 加载玩家相关
    void LoadAllPlayer(){
        //player config
        new PlayerConfig(); 
        PlayerConfig.instance.Init();
        InitPlayer(); //player加载
    }

    private void InitPlayer(){
        
        player.AddComponent<playerCtl>();

    }
    
}