using System;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr:MonoBehaviour
{
    
    static UIMgr _instance;
    public static UIMgr instance
    {
        get => _instance ??(_instance = new UIMgr());
        set => _instance = value;
    }

    public GameObject uiRoot;
    public GameObject npcDetailPanel;
    public bool isOpenWnd = false;
    public bool isOpenMapUI = false; //处于地图上的UI，而不是主界面UI
    public string curMapUI;         //当前正在打开的地图上的UI
    public Dictionary<string , BaseWindow> UIconfig = new Dictionary<string , BaseWindow>();
    public Dictionary<string , GameObject> UIprefab = new Dictionary<string , GameObject>();
    
    public GameObject NPCInfoButtonGroup ;
    public GameObject NpcSayHiButtonGroup ;
    public GameObject diaLogView;
    public GameObject GameBeginCanvas;
    public GameObject RoleFrustratioPanel;
    public GameObject mutiDiaLogView;

    public DialogView DiaLogViewEx;
    public MutiDiaLogView MutiDiaLogViewEx;
    public GameBeginCanvas  GameBeginCanvasEx;
    public int __openWndCount = 0;
    public bool isRuningStroy = false;
    public void init(){

        uiRoot = FT.ft.uiRoot;
        diaLogView          = uiRoot.transform.Find("MainCanvas/DiaLogView").gameObject;
        mutiDiaLogView      = uiRoot.transform.Find("MainCanvas/MutiDiaLogView").gameObject;
        npcDetailPanel      = uiRoot.transform.Find("NpcDetailPanel").gameObject;
        GameBeginCanvas     = uiRoot.transform.Find("GameBeginCanvas").gameObject;
        RoleFrustratioPanel = uiRoot.transform.Find("RoleFrustratioPanel").gameObject;
        //UI Controller
        DiaLogViewEx = diaLogView.GetComponent<DialogView>();
        MutiDiaLogViewEx = mutiDiaLogView.GetComponent<MutiDiaLogView>();

        UIprefab.Add("NpcDetailPanel",npcDetailPanel);
        UIconfig.Add("NpcDetailPanel",npcDetailPanel.GetComponent<NpcDetailPanelEx>());

        UIprefab.Add("RoleFrustratioPanel",RoleFrustratioPanel);
        UIconfig.Add("RoleFrustratioPanel",RoleFrustratioPanel.GetComponent<RoleFrustratioPanel>());
        
        GameBeginCanvasEx = GameBeginCanvas.GetComponent<GameBeginCanvas>();
        GameBeginCanvasEx.Awake();
        UIprefab.Add("GameBeginCanvas",GameBeginCanvas);
        UIconfig.Add("GameBeginCanvas",GameBeginCanvasEx);
    }

    public void OpenDayBeginCanvas(int day){

        //点击人物出现的选项框
        GameBeginCanvasEx.onShow(day);
    }
   
/// 、、、、、、、、、、、、、、、、、通用打开不用传参就能开启的界面、、、、、、、、、、、、、、、、、、、、、、、、
    public void OpenWindow(string name)
    {
        BaseWindow win ;
        UIconfig.TryGetValue(name,out win);
        if(win == null) return;   
        win.onOpen();
        __openWndCount ++ ;
    }



    public void HideWindow(string name)
    {
        BaseWindow win ;
        __openWndCount --;
        UIconfig.TryGetValue(name,out win);
        if(win != null)
        {
            if(playerCtl.PlayerIns != null)
            {
                playerCtl.PlayerIns._TigeerSTEvent("CLOSE_VIEW");
            }
            win.onHide();
        }
        else{
            Debug.LogError("没有这个window ，name：" + name);
        }
        __openWndCount = __openWndCount < 0  ? 0 : --__openWndCount ;
    }
///////////////////////////////end///////////////////////////////////////
/// 、、、、、、、、、、、、、、、需要参数开启的界面、、、、、、、、、、、、、、、、、、、、、、、、

    //角色详情界面
    public void OpenNpcDetailWindow(string dataStr)
    {

        GameObject winPrefab ;
        UIprefab.TryGetValue("NpcDetailPanel",out winPrefab);
        if(winPrefab != null)
        {   
            playerCtl.PlayerIns._TigeerSTEvent("OPEM_VIEW");
            winPrefab.GetComponent<NpcDetailPanelEx>().onShow(dataStr);
            __openWndCount ++ ;
        }
    }


    //生成点击npc操作界面
    public void InitNPCInfoButtonGroup(Collider2D col ,Vector3 position)
    {
        string name = col.gameObject.name;
        InitNPCInfoButtonGroup(name,position);
    } 


    //生成点击npc操作界面
    public void InitNPCInfoButtonGroup(string name ,Vector3 position)
    {   
        __openWndCount ++;
        isOpenMapUI = true;
        curMapUI = "NPCInfoButtonGroup";
        if(!NPCInfoButtonGroup)
        {
            NPCInfoButtonGroup = Instantiate(Resources.Load<GameObject>("NPCInfoButtonGroup"),position,Quaternion.identity);
            UIprefab.Add("NPCInfoButtonGroup",NPCInfoButtonGroup);
            UIconfig.Add("NPCInfoButtonGroup",NPCInfoButtonGroup.GetComponent<NPCInfoButtonGroup>());
        }
        NPCInfoButtonGroup.GetComponent<NPCInfoButtonGroup>().onRefresh(name,position);
    }


    //生成点击npc操作界面
    public void InitSayHiButtonGroup(string name ,Vector3 position)
    {
        __openWndCount ++;
        isOpenMapUI =true;
        curMapUI = "NpcSayHiButtonGroup";
        if(!NpcSayHiButtonGroup)
        {
            NpcSayHiButtonGroup = Instantiate(Resources.Load<GameObject>("NpcSayHiButtonGroup"),position,Quaternion.identity);
            UIprefab.Add("NpcSayHiButtonGroup",NpcSayHiButtonGroup);
            UIconfig.Add("NpcSayHiButtonGroup",NpcSayHiButtonGroup.GetComponent<NpcSayHiButtonGroup>());
        }
        NpcSayHiButtonGroup.GetComponent<NpcSayHiButtonGroup>().onRefresh(name,position);
    }

    public void HideMapUI(){
        
        if(curMapUI != "")
        {
            HideWindow(curMapUI);
            curMapUI = "";
            isOpenMapUI =false;
            __openWndCount --;
        }
    }
    

    public void OpenMutiDiaLogView(string Qrolename , string question , 
    string Arolename,string[] answersText,string[] replys,string[] ansewersReply){

        SetSrotyRunning(true);
        MutiDiaLogViewEx.ShowQuestionAndAnser(Qrolename,question,Arolename,answersText,replys,ansewersReply);
    }

    public void  HideMutiDiaLogView()
    {

        SetSrotyRunning(false);
        MutiDiaLogViewEx.HideMutiDiaLogView();
    }

    public void SetSrotyRunning(bool status)
    {
        isRuningStroy = status;
    }
}