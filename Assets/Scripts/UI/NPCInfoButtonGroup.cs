using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCInfoButtonGroup : BaseWindow
{

    private Button btnInfo;
    private Button btnSayHi;
    private string npcName;
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
        btnSayHi = transform.Find("btnSayHi").GetComponent<Button>();
        btnInfo = transform.Find("btnInfo").GetComponent<Button>();
        
        // btnSayHi.onClick.AddListener(OnBtnFastClick);
        
        btnInfo.onClick.AddListener(OnInfoClick);
        btnSayHi.onClick.AddListener(OnSayHiClick);
    }

    //打开
    public void onRefresh(string name,Vector3 position)
    {
        onOpen();
        transform.position = position;
        npcName = name; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void OnInfoClick()
    {
        UIMgr.instance.HideWindow("NPCInfoButtonGroup");
        UIMgr.instance.OpenNpcDetailWindow(npcName);
    }

    // Update is called once per frame
    private void OnSayHiClick()
    {
        UIMgr.instance.HideWindow("NPCInfoButtonGroup");
        UIMgr.instance.InitSayHiButtonGroup(npcName,transform.position);
    }
}
