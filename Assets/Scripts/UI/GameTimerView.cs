using System.Diagnostics.Tracing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimerView : MonoBehaviour
{
    private FT ft;
    private SEventSystem SESins;
    private SystemTime systemTime;
    private SEventSystem eventIns = SEventSystem.EventIns;
    private Text txtTimer;
    private Text txtTimScale;
    private Button btnFast;
    private Button btnSlow;
    private Button btnFrustratio;
    private Image imgHealthBar;
    private Image imgSadBar;
    private Text txtHealth;
    private Text txtSad;

    // Start is called before the first frame update
    private void Awake() {
        
        txtTimer = transform.Find("txtTimer").GetComponent<Text>();     
        btnFast = transform.Find("btnTimeFast").GetComponent<Button>();  
        btnSlow = transform.Find("btnTimeSlow").GetComponent<Button>();  
        btnFrustratio = transform.Find("btnFrustratio").GetComponent<Button>();  
        txtTimScale = transform.Find("txtTimeScale").GetComponent<Text>();   
        imgHealthBar = transform.Find("imgHealthBar").GetComponent<Image>();   
        imgSadBar = transform.Find("imgSadBar").GetComponent<Image>();   
        txtHealth = transform.Find("imgHealthBar/txtHealth").GetComponent<Text>();  
        txtSad = transform.Find("imgSadBar/txtSad").GetComponent<Text>();  

        ft = FT.ft;
        SESins =SEventSystem.EventIns;
    }
    private void OnEnable() {

        SESins.PLAYER_HEALTH_UP.AddListener(updateHealth);
        SESins.PLAYER_HEALTH_DOWN.AddListener(updateHealth);
        SESins.PLAYER_SAD_DOWN.AddListener(updateSad);
        SESins.PLAYER_SAD_UP.AddListener(updateSad);
    }

    private void OnDisable(){

        SESins.PLAYER_HEALTH_UP.RemoveListener(updateHealth);
        SESins.PLAYER_HEALTH_DOWN.RemoveListener(updateHealth);
        SESins.PLAYER_SAD_DOWN.RemoveListener(updateSad);
        SESins.PLAYER_SAD_UP.RemoveListener(updateSad);
    }
    void Start(){

        systemTime = ft._SystemTime;
        btnFast.onClick.AddListener(OnBtnFastClick);
        btnSlow.onClick.AddListener(OnBtnSlowClick);
        btnFrustratio.onClick.AddListener(OnBtnFrustratioClick);
        setTxtTimeScale();
        setPlayerState();
    }

    private void setTxtTimeScale(){

        txtTimScale.text = systemTime.TimeScale.ToString();
    }

    private void FixedUpdate() {

        txtTimer.text = systemTime.StrGlobalTime;
    }

    void OnBtnFastClick(){
        SESins.TIME_SCALE_FAST.Invoke();
        setTxtTimeScale();
    }


    void OnBtnSlowClick(){

        SESins.TIME_SCALE_SLOW.Invoke();
        setTxtTimeScale();
    }
    
    void OnBtnFrustratioClick(){
        
        UIMgr.instance.OpenWindow("RoleFrustratioPanel");
    }

    private void setPlayerState(){

        imgHealthBar.fillAmount = PlayerConfig.instance.HealthValue / 100 ;
        imgSadBar.fillAmount =  PlayerConfig.instance.SadValue / 100 ;
        txtHealth.text =  PlayerConfig.instance.SadValue.ToString();
        txtSad.text =  PlayerConfig.instance.SadValue.ToString();
    }

    void updateSad()
    {

        imgSadBar.fillAmount =  PlayerConfig.instance.SadValue / 100 ;
        txtSad.text =   PlayerConfig.instance.SadValue.ToString();
    }
    
    void updateHealth(){

        txtHealth.text =   PlayerConfig.instance.HealthValue.ToString();
        imgHealthBar.fillAmount =   PlayerConfig.instance.HealthValue / 100 ;
    }
}
