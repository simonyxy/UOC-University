using System.Security.Principal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoleFrustratioPanel : BaseWindow
{
    GameObject frustratio;
    private GameObject content;
    private Button btnClose;
    // Start is called before the first frame update
    void Start()
    {
        // UIMgr.instance.isOpenWnd = true;
        btnClose = transform.Find("btnClose").GetComponent<Button>();
        frustratio = transform.Find("FrustratioSV/Viewport/frustratio").gameObject;
        content = transform.Find("FrustratioSV/Viewport/Content").gameObject;
        btnClose.onClick.AddListener(onCloseBtnClick);
        frustratio.SetActive(false);
        loadFrustatio();
    }
    

    public override void onOpen()
    {
        
        gameObject.SetActive(true);
    }
    
    
    // public override void hidePlayerDetailPanel()
    public override void onHide()
    {
        gameObject.SetActive(false);
    }
    
    void loadFrustatio(){
        
        for( int i = 0 ; i < FrustratioMgr.instance.allFrustatios.Count; i++)
        {
            if(FrustratioMgr.instance.allFrustatios[i]!= null &&  FrustratioMgr.instance.allFrustatios[i].isOpen == 0 )
            {
                GameObject item =  GameObject.Instantiate(frustratio,content.transform);
                if(item)
                setFrustatioItem(item,FrustratioMgr.instance.allFrustatios[i]);
            }
        }
    }


    //0未开启，1开启，2完成
    string[] nameStruct =new string[3] 
    {
        "未开启",
        "开启",
        "已完成"
    };
    void setFrustatioItem(GameObject item,Frustratio itemData){
        
        item.SetActive(true);
        Text txtID = item.transform.Find("txtID").GetComponent<Text>();
        Text txtName = item.transform.Find("txtName").GetComponent<Text>();
        Text txtFriendValue = item.transform.Find("txtFriendValue").GetComponent<Text>();
        Text txtOpen = item.transform.Find("txtOpen").GetComponent<Text>();
        Debug.LogError(itemData);
        txtOpen.text = nameStruct[itemData.isOpen];
        txtID.text   = itemData.id.ToString();
        txtName.text = itemData.name; 
    }

    void onCloseBtnClick(){
        
        UIMgr.instance.HideWindow("RoleFrustratioPanel");
    }
}
