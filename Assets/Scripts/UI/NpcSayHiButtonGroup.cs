using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcSayHiButtonGroup : BaseWindow
{
    private Button btnSayHiUp;
    private Button btnSayHiDown;
    private Button btnSayHiLeft;
    private Button btnSayHiRight;
    private string npcName;

    private NpcCtl npcCtl;
    private float positionOffSet = 1f;
    public override void onHide()
    {
        gameObject.SetActive(false);
        // FT.ft.ST.SetTimeScaleByBool(false);
    }
    
    public override void onOpen()
    {
        gameObject.SetActive(true);
        // FT.ft.ST.SetTimeScaleByBool(true);
    }

    public void Awake() {
        btnSayHiUp   = transform.Find("btnSayHiUp").GetComponent<Button>();
        btnSayHiDown = transform.Find("btnSayHiDown").GetComponent<Button>();
        btnSayHiLeft = transform.Find("btnSayHiLeft").GetComponent<Button>();
        btnSayHiRight= transform.Find("btnSayHiRight").GetComponent<Button>();
        
        btnSayHiUp.onClick.AddListener(onBtnHiUpClick);
        btnSayHiDown.onClick.AddListener(onBtnHiDownClick);
        btnSayHiLeft.onClick.AddListener(onBtnHiLeftClick);
        btnSayHiRight.onClick.AddListener(onBtnHiRightClick);
    }

    //打开
    public void onRefresh(string name,Vector3 position)
    {
        onOpen();
        transform.position = position;
        npcName = name; 


        npcCtl = NpcMgr.instance.GetCtl(npcName);
        if(!npcCtl) 
        {
            Debug.LogError("没有改npc数据，name:" + npcName);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void onBtnHiUpClick(){

        PlayerMoveToNpc();
    }


    void onBtnHiDownClick(){
        
        PlayerMoveToNpc();
    }

    
    void onBtnHiLeftClick(){
        
        PlayerMoveToNpc();
    }

    
    void onBtnHiRightClick(){
        
        PlayerMoveToNpc();
    }

    void PlayerMoveToNpc(){

        //判断npc朝向
        Vector2 npcPosition = new Vector2(transform.position.x + positionOffSet,transform.position.y)  ;
        playerCtl.PlayerIns.normalMoveToPosition(npcPosition,()=>{});
        npcCtl.FroceNpcBeingClick();
        UIMgr.instance.HideMapUI();
    }
}
