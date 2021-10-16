using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public  class SystemTime : MonoBehaviour
{
    //经过每天都需要保存一次永久数据，不覆盖那种，方便跳转回来

    public static SystemTime sysTime;
    //1-21
    private FT ft;
    public SEventSystem SESins;

    [HideInInspector]private string strglobalTime;    
    [HideInInspector]private int globalTimeNum;
    [HideInInspector]private float timeScale = 1f;
    [HideInInspector]private int days = 1;
    [HideInInspector]private int hours = 1;
    [HideInInspector]private int minutes = 1;


    [HideInInspector]public int GlobalTimeNum{
        get{return globalTimeNum;}
    } 
    [HideInInspector]public string StrGlobalTime {
        get{ return strglobalTime;}
    } 
    [HideInInspector]public float TimeScale{
        get{return timeScale;}
    } 

    private void Awake() {
        ft = FT.ft;
        sysTime = this;
        SESins = SEventSystem.EventIns;
    }

    private void OnEnable() {
        
        SESins.TIME_SCALE_FAST.AddListener(TimeScaleFaster);
        SESins.TIME_SCALE_SLOW.AddListener(TimeScaleSlow);
        
    }

    private void OnDisable() {
        
        SESins.TIME_SCALE_FAST.RemoveListener(TimeScaleFaster);
        SESins.TIME_SCALE_SLOW.RemoveListener(TimeScaleSlow);
    }

    private void Start(){

        //从内存读取初始化时间days，hours，minutes，totalseconds
        readGloabTime();
    }

    
    void readGloabTime() {

        strglobalTime = FormatDayTime(globalTimeNum);
    }

    
    //时间系统也需要修改
    void FixedUpdate() {
        
        globalTimeNum ++ ; 
        strglobalTime = AddOneDayTime();
    }

    string FormatDayTime(int totalSeconds)
    {
        days = (totalSeconds/3600)/24;
        hours = (totalSeconds/3600)/24 - (days *24);
        minutes = (totalSeconds - (hours * 3600) - (days * 86400))/60;
        return string.Format("{0:D2}:{1:D2}:{2:D2}",days,hours,minutes);
    }

    void JumpToDay(int day)
    {
        //读取该天数据
        globalTimeNum = day * 3600 * 24;
    }


    string AddOneDayTime()
    {
        minutes++;
        if(minutes == 60)
        {
            minutes = 0 ; 
            hours ++ ; 

            if(hours /2 == 0)
            {

                //保存一次永久数据
                SaveAllDataForever();
            }

            if(hours == 24)
            {
                hours =0;
                days++;
            }
        }
        return string.Format("{0:D2}:{1:D2}:{2:D2}",days,hours,minutes);
    }

    //保存数据
    void SaveAllDataForever(){
        
    }
    
    //游戏加速
    void TimeScaleFaster(){

        if(timeScale ==1 )
        {
            timeScale =3f;
        }
        Time.timeScale = timeScale;
    }

    //游戏减速
    void TimeScaleSlow(){
        if(timeScale == 3f )
        {
            timeScale = 1f;
        }
        Time.timeScale = timeScale;
    }

    public void SetTimeScaleByBool(bool isSlowMove){
         if(isSlowMove)
        {
            Time.timeScale = 0.1f;
        }
        else{
            Time.timeScale = 1f;
        }
    }

    void AddDayTime( int days,int hours,int minutes)
    {

    }
}
