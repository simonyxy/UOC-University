using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MutiDiaLogView : BaseWindow
{
    FT ft ;

#region UI 
    
    private Image imgLeft;
    private Image imgRight;
    //question
    [HideInInspector]public Text   txtLeft;
    
    //answer
    [HideInInspector]public Button btnA1;
    [HideInInspector]public Button btnA2;
    [HideInInspector]public Button btnA3;
    [HideInInspector]public Button btnA4;
    
    [HideInInspector]public Text txtA1;
    [HideInInspector]public Text txtA2;
    [HideInInspector]public Text txtA3;
    [HideInInspector]public Text txtA4;
    private Text[] txtAList = new Text[4];

    private Button[] btnAList = new Button[4];
    [HideInInspector]public Text  txtRight;
    private GameObject leftBtnGroup ;
    private GameObject rightBtnGroup ;

    public int selectAnsID = -1;
    //UI界面状态
    public struct curUIState
    {
        public static int ShowPanel = 0;
        public static int SetQuestion = 1 ;
        public static int SetAnswer = 2 ;
        public static int ClickAnswer = 3 ;
        public static int SetReply = 4 ;
        public static int FinishSetReply = 5 ;

    }
    private int curState;
#endregion
    //stirng position
    private string Qrolename ; 
    private string Arolename;
    private string question;
    private string[] replys;
    private string[] answersText;
    private string[] ansewersReply;

    private bool isShow;
    private bool canClick = true;

    //动态加载字符串 // 
    private bool isRolingStr = false;
    private Text _txtCurRoling;
    private string _needShowStr;
    private string _curStr;
    private Animator anim;
    //
    public void Awake() {

        ft = FT.ft;
        //TODO:查看何时调用的这个功能
        initUI();
    }

    private void initUI() {
        
        imgLeft = transform.Find("fixHeight/imgLeft").GetComponent<Image>();
        imgRight = transform.Find("fixHeight/imgRight").GetComponent<Image>();
        btnA1 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA1").GetComponent<Button>();
        btnA2 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA2").GetComponent<Button>();
        btnA3 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA3").GetComponent<Button>();
        btnA4 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA4").GetComponent<Button>();
        txtA1 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA1/txtQ1").GetComponent<Text>();
        txtA2 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA2/txtQ1").GetComponent<Text>();
        txtA3 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA3/txtQ1").GetComponent<Text>();
        txtA4 = transform.Find("fixHeight/imgRight/RightDiaglog/btnA4/txtQ1").GetComponent<Text>();
        anim = transform.GetComponent<Animator>(); //

        txtAList =new Text[]{
            txtA1,txtA2,txtA3,txtA4
        };

        btnAList = new Button[]{
            btnA1,btnA2,btnA3,btnA4
        };
        txtLeft = transform.Find("fixHeight/imgLeft/txtLeft").GetComponent<Text>();
        txtRight = transform.Find("fixHeight/imgRight/txtRight").GetComponent<Text>();
        leftBtnGroup = transform.Find("fixHeight/imgLeft/leftDiaglog").gameObject;
        rightBtnGroup = transform.Find("fixHeight/imgRight/RightDiaglog").gameObject;

        
        btnA1.onClick.AddListener(onBtnSelectAnswer0);
        btnA2.onClick.AddListener(onBtnSelectAnswer1);
        btnA3.onClick.AddListener(onBtnSelectAnswer2);
        btnA4.onClick.AddListener(onBtnSelectAnswer3);
    }


    //开始当前timeLine Click的现实
    public void ShowQuestionAndAnser(string Qrolename , string question , 
    string Arolename,string[] answersText,string[] replys,string[] ansewersReply)
    {
        onOpen();

        this.Qrolename = Qrolename;
        this.question  = question;
        this.Arolename = Arolename;
        this.answersText   = answersText;
        this.replys    = replys;
        this.ansewersReply = ansewersReply;
        this.selectAnsID = -1;
        //关闭按钮的可点击
        
        leftBtnGroup.SetActive(false);
        rightBtnGroup.SetActive(true);
        txtLeft.gameObject.SetActive(true);
        txtRight.gameObject.SetActive(false);

        curState = curUIState.ShowPanel;
        MoveToNextMove();
    }


    //显示问题
    public void _setQuestion(string Qrolename ,string question , string Arolename,string[] answersText)
    {
        
        //TODO:通过Qrolename设置左边图案
        // imgLeft.sprite = ...
        //TODO:通过Arolename设置右边图案
        // imgLeft.sprite = ...
        SetRolingInfo(txtLeft,question);
        beginRolingStr();
        //playAnim
        txtLeft.text = "";
        txtLeft.text = this.question;  //TODO;将剧情对话设计成会一个字一个字弹出的方式
    }


    //显示答案
    public void _setAnswers(string Qrolename ,string question , string Arolename,string[] answersText)
    {

        txtLeft.text = this.question;

        for(int i = 0;i < answersText.Length ;i ++ )
        {
            this.txtAList[i].text = answersText[i];
        }
    }



    public void _showAnswersReply(string question,string[] ansewersReply)
    {

        rightBtnGroup.SetActive(false);
        txtRight.gameObject.SetActive(true);
        txtLeft.text = this.question;
        string tempReply = ansewersReply[selectAnsID];
        if(tempReply == null || tempReply =="")
        {
            return ;
        } 
        txtRight.text = tempReply;
    }


    //input K to do 
    public void MoveToNextMove()
    {
        if(curState != curUIState.SetQuestion && curState != curUIState.FinishSetReply)// && curState != curUIState.ShowPanel
        {
            //TODO:改为timer
            Timer _checkClick = new Timer("CheckClick",1,CheckClick,true);
            TimerManager.instance.AddTimer(_checkClick);
        }
        Debug.LogError("MoveToNext:" + curState);
        
        if(isRolingStr)
        {
            Debug.LogError("快速结束字幕生成");
            FinishRolingStr();
            return;
        }

        if(curState == curUIState.ShowPanel)
        {
            _setQuestion(Qrolename ,question , Arolename,answersText);
            curState = curUIState.SetQuestion;
            return ;
        }
        if(curState == curUIState.SetQuestion)
        {
            _setAnswers(Qrolename,question,Arolename,answersText);
            curState = curUIState.SetAnswer;
            return ;
        }
        else if(curState == curUIState.SetAnswer)
        {
            //temp
            canClick = true;
            _showAnswersReply(question,answersText);
            curState = curUIState.SetReply;
            return;
        }
        else if(curState == curUIState.SetReply)
        {

            curState = curUIState.FinishSetReply;
            SEventSystem.EventIns.MUTIDIALOG_TIMELINE_NORMAL_CLICK.Invoke();
            return;
        }
    }

    public void onBtnSelectAnswer0()
    {

        selectAnsID = 0;
        MoveToNextMove();
    }

    
    public void onBtnSelectAnswer1()
    {

        selectAnsID = 1;
        MoveToNextMove();
    }
    
    public void onBtnSelectAnswer2()
    {
        
        selectAnsID = 2;
        MoveToNextMove();
    }

    public void onBtnSelectAnswer3()
    {
        
        selectAnsID = 3;
        MoveToNextMove();
    }

    public void HideMutiDiaLogView(){

        Debug.LogError("HIde MutiDialogView");
        this.gameObject.SetActive(false);
    }

    public override void onHide()
    {
    }

    public override void onOpen()
    {
        if(!isShow)
        isShow = true;
        gameObject.SetActive(true);
    }

/////////////Click///////////

    void CheckClick(){

        Debug.LogError("check Click");
        if(canClick && isShow)
        {
            if( Input.GetKey(KeyCode.K))
            {
                // ft.sTimer.RemoveEvent("CheckClick"); 
                //todo:
                TimerManager.instance.RemoveTimer("CheckClick");
                MoveToNextMove();
                canClick = false;
                ColdClick();
            }
        }
    }
    public void ColdClick(){
        
        Timer _coldClick = new Timer("ColdClick",1,ColdClickEnd,false);
        // ft.sTimer.New("ColdClick",ColdClickEnd,1f);
        TimerManager.instance.AddTimer(_coldClick);
    }

    public void ColdClickEnd(){

        Debug.LogError("IN COLDcLICK");
        canClick = true;
    }
    
////////////////////////
//

///////////////////////////////////////////////////////////////////
    public void SetRolingInfo(Text txtCurRoling,string needShowStr)
    {
        _curStr = "";
        _needShowStr  = needShowStr;
        _txtCurRoling = txtCurRoling;
    }

    public void beginRolingStr(){
        
        _curStr = "";
        anim.SetBool("qustionInit",true);
        
        Timer a = new Timer("mutiStrRole",1,null,setShowTextStep,null,true);

        TimerManager.instance.AddTimer(a);
        isRolingStr = true;
    }

    public void FinishRolingStr()
    {
        _curStr = _needShowStr;
        _txtCurRoling.text = _curStr;
        TimerManager.instance.RemoveTimer("mutiStrRole");
        
        isRolingStr = false;
    }

    public void setShowTextStep()
    {
        // if(!isRolingStr)
        // return ;

        if(_curStr.Length >= _needShowStr.Length)
            FinishRolingStr();

        int curLen = _curStr.Length;
        curLen ++;
        _curStr = _needShowStr.Substring(0,curLen -1);
        _txtCurRoling.text = _curStr;
    }


    void QuicSort(int[] nums, int left , int right)
    {
        if(nums.Length == 0 || nums.Length == 1 || left >=right )
        {
            return ;
        }

        int l = left;
        int r = right;
        int temp = nums[left];
        while(l<r)
        {
            while(l<r)
            {
                if(nums[r] < temp)
                {
                    nums[l] = nums[r];
                    break;
                }
                r--;
            }    

            while(l < r)
            {
                if(nums[l] > temp)
                {
                    nums[r] = nums[l]; 
                }
                l++;
            }
        }
        nums[l] = temp;
        QuicSort(nums, 0 , l);
        QuicSort(nums,l + 1 , nums.Length);
    }     
//////////////////////////////////////////////////////////
}
 