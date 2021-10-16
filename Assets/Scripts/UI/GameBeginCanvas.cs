using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBeginCanvas : BaseWindow
{
    private Text txtDay;
    public void Awake() {
        
        txtDay = transform.Find("GameBeginPanel/txtDay").GetComponent<Text>();
    }

    // public void showPlayerDetail(string name){
    public void onShow(int day){
        
        txtDay.text ="第" + day.ToString() + "天";
        onOpen();
        
        transform.GetComponent<Animator>().SetBool("gameBegin",true);
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
    

     void EndBeginGameAnim(){
        Animator tempAnim  =  this.transform.GetComponent<Animator>();
        tempAnim.SetBool("gameBegin",false);
        tempAnim.enabled = false;

        UIMgr.instance.HideWindow("GameBeginCanvas");
    }
}
