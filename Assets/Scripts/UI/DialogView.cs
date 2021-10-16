using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class DialogView : BaseWindow
{
    FT ft ;

    private Image imgLeft;
    private Image imgRight;
    private Text txtLeft;
    private Text txtRight;
    private int openType;
    private PlayableGraph curPlayGrapha;
    // public Dictionary<string , string > story0DialogPos ;

    //当前左置位 ，当前右置位
    private string strLeftName ; 
    private string strRightName;
    
    private bool isShow;
    private bool canClick = true;

    public struct  OpenType 
    {
        public static int timeLine = 1; //timeline打开
        public static int codeType = 2; //代码打开
    }

    public void Awake() {

        ft = FT.ft;
        imgLeft = transform.Find("imgLeft").GetComponent<Image>();
        imgRight = transform.Find("imgRight").GetComponent<Image>();
        txtLeft = transform.Find("imgLeft/leftDiaglog/txtLeft").GetComponent<Text>();
        txtRight = transform.Find("imgRight/RightDiaglog/txtRight").GetComponent<Text>();
    }

    private void OnEnable() {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetOpenType(){

    }
    public void ShowDiaLog(string name , string postion , string dialog)
    {
        //3-3
        // this.curPlayGrapha = curPlayGrapha;
        ShowDiaLogWithType(name,postion,dialog,OpenType.timeLine);
    }

    public void ShowDiaLogWithType(string name , string postion , string dialog,int inOpenType){
        
        
        if(isActiveAndEnabled == false)
        this.gameObject.SetActive(true);

        if( postion == "left")
        {
            SetDiagLogAndIcon(name,txtLeft,imgLeft ,dialog);
        }
        else
        {
            SetDiagLogAndIcon(name,txtRight,imgRight,dialog );
        }
        openType = inOpenType;  //默认状态
    }

    public void SetDiagLogAndIcon(string  name , Text txt , Image imgIcon,string dialog)
    {
        //头像设置
        //img.sprite = LoadUIAtlas ..... 

        txt.text = dialog;
        isShow =true;
        
        ft.sTimer.New("CheckClick",CheckClick,-1f,true);
    }





    void CheckClick(){

        if(isShow)
        {   
            if(canClick)
            {
                if( Input.GetKey(KeyCode.K))
                {
                    MoveToNextMove();
                    canClick = false;
                }
            }
        }
    }

    public void MoveToNextMove()
    {
        canClick  =false;
        ColdClick();
        ft.sTimer.RemoveEvent("CheckClick");
        if(openType == OpenType.timeLine)
        {
            SEventSystem.EventIns.DIALOG_TIMELINE_NORMAL_CLICK.Invoke();
        }
        else
        {
            
        }
    }

    private void ColdClick(){

        ft.sTimer.New("ColdClick",ColdClickEnd,0.3f);
    }

    void ColdClickEnd(){
        
        canClick = true;
    }

    public void HideDiaLogView(){

        this.gameObject.SetActive(false);
    }

    public override void onHide()
    {
    }

    public override void onOpen()
    {
    }
}
