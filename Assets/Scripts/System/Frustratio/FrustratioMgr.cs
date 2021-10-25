using System;
using System.Collections.Generic;
using UnityEngine;

//desc:挫折管理器
//desc:
//
//挫折系统是只属于角色身上的东西（playerCtl上有一个记录当前挫折id的列表）。 和npc之间其实是没有关系的。
//挫折中会记录需要寻找的npc或者到达的地方。当你点击所有的npc和到达某个触发器的时候，会去遍历你身上的挫折，看npc名字，是否有满足挫折的条件（名字是否符合）。有的话就增加点击按钮，增加的按钮后续的操作，也只会用了npc的名字和挫折对应表里面的东西，不会有影响。
//]]
public class FrustratioMgr:MonoBehaviour
{
    
    static FrustratioMgr _instance;
    public static FrustratioMgr instance
    {
        get => _instance ??(_instance = new FrustratioMgr());
        set => _instance = value;
    }

    //Frustatio in player
    public List<Frustratio> playerFrustatios = new List<Frustratio>();
    public List<Frustratio> allFrustatios = new List<Frustratio>();
    public List<int> openIDList = new List<int>();
    Frustratio __playerSick = new Frustratio(0,0,
    "挫折测试",
    ()=>{},
    ()=>{return false;} ,
    ()=> {return false;}
    );

    Frustratio __ForgetTakePaper = new Frustratio(1,0,
    "拉屎没带纸",
    ()=>{},
    ()=>{return false;} ,
    ()=> {return false;}
    );

    Frustratio __SadValueIsLow = new Frustratio(2,0,
    "抑郁值太低",
    ()=>{},
    ()=>{return false;} ,
    ()=> {return false;}
    );

    public void init()
    {
        // init Frustratio
        allFrustatios.Add(__playerSick);
        allFrustatios.Add(__ForgetTakePaper);
        allFrustatios.Add(__SadValueIsLow);
        //set state
        __playerSick.SetState(GetFrustratioState(__playerSick.ToId()));
        __ForgetTakePaper.SetState(GetFrustratioState(__ForgetTakePaper.ToId()));
        __SadValueIsLow.SetState(GetFrustratioState(__SadValueIsLow.ToId()));
    } 

    void SaveFrustratioState (int id , int state){
        string temp = String.Format("Frustratio_",id.ToString());
        PlayerPrefs.SetInt( temp,state);
    }
    public int GetFrustratioState (int id ){
        return PlayerPrefs.GetInt("Frustratio_" + id.ToString(),0);
    } 
}