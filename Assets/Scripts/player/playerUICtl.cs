using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;


public class playerUICtl : MonoBehaviour
{
    //SelectPanel
    private playerUICtl _pUIC;
    private GameObject SelectCircle;
    private GameObject objSelectup;
    private GameObject objSelectleft;
    private GameObject objSelectright;
    private GameObject objSelectdown;
    private GameObject objCurSelectd;
    private Button  btnSelectup;
    private Button  btnSelectleft;
    private Button  btnSelectright;
    private Button  btnSelectdown;
    private FT ft;
    private playerCtl playIns;
    private UnityEvent triggerAction;
    
    
    private  struct SelectType
    {
        public static int In = 1;
        public static int Out =2 ;
    };
    private void Awake() {
        
        _pUIC = this;
        ft = FT.ft;
        playIns = playerCtl.PlayerIns;
        SelectCircle = transform.Find("SelectCircle").gameObject;
        objSelectdown = SelectCircle.transform.Find("down/selectBg").gameObject;
        objSelectleft = SelectCircle.transform.Find("left/selectBg").gameObject;
        objSelectright = SelectCircle.transform.Find("right/selectBg").gameObject;
        objSelectup = SelectCircle.transform.Find("up/selectBg").gameObject;
        btnSelectup = objSelectup.GetComponent<Button>();
        btnSelectleft = objSelectleft.GetComponent<Button>();
        btnSelectright = objSelectright.GetComponent<Button>();
        btnSelectdown = objSelectdown.GetComponent<Button>();
    }

    //select State
    //TODO:放到playerUIctl中
    public void StSelectState(int state){

        playIns.canUIClick = false;
        if(state == SelectType.In)
        {
            SelectCircle.SetActive(true);
            playIns.isSelectState = true;
            ft.ST.SetTimeScaleByBool(true);
            //进入选择
            ft.sTimer.New("CheckAndSetSelect",CheckAndSetSelect,-1f,true);
            ft.sTimer.New("UIClickColding",UIClickColding,0.015f);
        }
        else {

            SelectCircle.SetActive(false);
            playIns.isSelectState = false;
            ft.ST.SetTimeScaleByBool(false);
            ft.sTimer.New("UIClickColding",UIClickColding,0.015f);
        }
    
    }

    
    private void UIClickColding(){

        playIns.canUIClick = true;
    }

    private void CheckAndSetSelect(){

        if (!playIns.canUIClick)
            return;

        if(Input.GetKey(KeyCode.W))
        {
            SelectButton(objSelectup, SEventSystem.EventIns.PLAYER_CHOSE_UP);
        
            playIns.canUIClick = false;
            ft.sTimer.New("UIClickColding",UIClickColding,0.015f);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            SelectButton(objSelectdown, SEventSystem.EventIns.PLAYER_CHOSE_DOWN);
            playIns.canUIClick = false;
            ft.sTimer.New("UIClickColding",UIClickColding, 0.015f);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            SelectButton(objSelectleft, SEventSystem.EventIns.PLAYER_CHOSE_LEFT);
            playIns.canUIClick = false;
            ft.sTimer.New("UIClickColding",UIClickColding, 0.015f);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            SelectButton(objSelectright, SEventSystem.EventIns.PLAYER_CHOSE_RIGHT);
            playIns.canUIClick = false;
            ft.sTimer.New("UIClickColding",UIClickColding, 0.015f);
        }
        else if(Input.GetKeyDown(KeyCode.K))
        {
            ft.sTimer.RemoveEvent("CheckAndSetSelect");
            StSelectState(SelectType.Out);
            playIns.canUIClick = false;
            ft.sTimer.New("UIClickColding",UIClickColding, 0.2f);
            if(triggerAction != null)
            {
                triggerAction.Invoke();
            }
        }
    }

    private void SelectButton(GameObject objSelect , UnityEvent eEvent){
        objCurSelectd?.SetActive(false);
        objCurSelectd = objSelect;
        objSelect.SetActive(true);
        triggerAction = eEvent;
    }
}