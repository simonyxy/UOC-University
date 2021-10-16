using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NpcDetailPanelEx : BaseWindow
{
    private Text txtName ;
    
    private Text txtFriendValue;
    private Text txtStoryText;
    private Button btnClose;
    // Start is called before the first frame update
    public void Awake() {
        
        txtName = transform.Find("NPCDetailPanel/txtName").GetComponent<Text>();
        txtFriendValue = transform.Find("NPCDetailPanel/txtFriendValue").GetComponent<Text>();
        txtStoryText = transform.Find("NPCDetailPanel/storyScroll/Viewport/Content/storyText").GetComponent<Text>();
        btnClose= transform.Find("NPCDetailPanel/btnClos").GetComponent<Button>();
        btnClose.onClick.AddListener(btnCloseOnClick);
    }

    // public void showPlayerDetail(string name){
    public void onShow(string name){
        
        NpcMgr.instance._GCfgList.TryGetValue(name,out IStoryConfig config );
        if(config != null)
        {
            onOpen();
            txtName.text = config.nameChi;
            txtFriendValue.text = PlayerConfig.instance._getFriend(name).ToString();
            txtStoryText.text = config.story;
            gameObject.SetActive(true);
        }
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
    
    public  void btnCloseOnClick(){
        
        UIMgr.instance.HideWindow("NpcDetailPanel");
    }
}
