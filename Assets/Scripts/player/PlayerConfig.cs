using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

//todo:把数值挪走

public class PlayerConfig 
{
    private static PlayerConfig _instance;
    public static PlayerConfig instance
    {
        get => _instance ??(_instance = new PlayerConfig());
        set => _instance = value;
    }

    public float SadValue
    {
        get{return PConfig.sadValue;}
    }
    public float HealthValue
    {
        get{return PConfig.healthValue;}
    }
    private SEventSystem SESins;

    //友谊对应数，把对应的值记录到相应的名称下
    private Dictionary<string , int> npcFriendDictionary = new Dictionary<string , int>();

    //Frustratio Finish List;
    public List<int> finishFruList = new List<int>();

    //Story List
    public Dictionary<string , int> playerStoryList = new Dictionary<string , int>();



#region 初始化数据
//init--------------------------------------------------------------------
    //在FT生成的时候就初始化了    
    public void Init(){

        // Debug.LogError("Init PlayerCofig");
        SESins = SEventSystem.EventIns;

        //读取当前玩家的值
        PConfig.sadValue = PlayerPrefs.GetInt("sadValue",100);
        PConfig.healthValue = PlayerPrefs.GetInt("healthValue",100);
        //友谊值
        InitFriendData();

        //挫折
        InitFrustratioData();

        //主线故事
        InitStoryData();
    }

    public void InitFriendData(){
    
        PConfig.friend_YaGe = PlayerPrefs.GetInt("friend_YaGe",100);
        PConfig.friend_YangHuiMin = PlayerPrefs.GetInt("friend_YangHuiMin",100);
        npcFriendDictionary.Add("friend_YaGe",PConfig.friend_YaGe);
        npcFriendDictionary.Add("friend_YangHuiMin",PConfig.friend_YangHuiMin);
    }

    public void InitFrustratioData(){

        //0未开启，1开启，2完成
        PConfig.Frustratio_0 = FrustratioMgr.instance.GetFrustratioState(0);
        PConfig.Frustratio_1 = FrustratioMgr.instance.GetFrustratioState(1);
        PConfig.Frustratio_2 = FrustratioMgr.instance.GetFrustratioState(2);
    }

    //0 未进行 1 进行结束
    public void InitStoryData(){

        PConfig.Story_0 = PlayerPrefs.GetInt("Story_0",0);
        PConfig.Story_1 = PlayerPrefs.GetInt("Story_1",0);
        PConfig.Story_2 = PlayerPrefs.GetInt("Story_2",0);
        playerStoryList.Add("Story_0",PConfig.Story_0 );
        playerStoryList.Add("Story_1",PConfig.Story_1 );
        playerStoryList.Add("Story_2",PConfig.Story_2 );
    }
//--------------------------------------------------------------------------------
#endregion

    public void _AddPlayerHealth(int value){
        PConfig.healthValue += value;

        if(PConfig.healthValue > 100)
            PConfig.healthValue = 100;

        SESins.PLAYER_HEALTH_UP.Invoke();
    }

    public void _AddPlayerSad(int value){

        PConfig.sadValue += value;
        if(PConfig.sadValue > 100)
            PConfig.sadValue = 100;

        SESins.PLAYER_SAD_UP.Invoke();
        //保存悲伤值    
        PlayerPrefs.SetInt("sadValue",PConfig.sadValue);
    }

    public void _MinsPlayerSad(int value){
        PConfig.sadValue -= value;

        if(PConfig.sadValue < 0)
            PConfig.sadValue = 0;

        SESins.PLAYER_SAD_DOWN.Invoke();
        //保存悲伤值    
        PlayerPrefs.SetInt("sadValue",PConfig.sadValue);
    }

    public void _MinsPlayerHealth(int value){
        
        PConfig.healthValue -= value;

        if(PConfig.healthValue < 0)
            PConfig.healthValue = 0;

        SESins.PLAYER_HEALTH_DOWN.Invoke();
    } 
//////////////////////////////////////////////////////////////
    public int _getFriend(string npcName)
    {
        string getName = string.Concat("friend_",npcName);

        npcFriendDictionary.TryGetValue(getName,out int value);
        // int value = npcFriendDictionary.TryGetValue(getName);
        return  value;
    }

    //每次设置都保存
    public void _setFriend(string npcName,int value)
    {
        string getName = string.Concat("friend_",npcName);

        //修改Value，不确定这样写对不对
        if(npcFriendDictionary[getName] != 0 )
        npcFriendDictionary[getName] = value;
        //保存到本地永久
        PlayerPrefs.SetInt(getName,-1);
    }
    //挫折。挫折id，挫折结束时间，挫折开启时间，
///////////////////////////////////////////////////////////////////


//Check can play story
    public bool CheckCanPlayStory(int id)
    {
        Debug.LogError("check can play story , id = " + id);
        int  storyState = -1  ;
        playerStoryList.TryGetValue("Story_" + id.ToString(),out storyState);
        if( storyState  == -1 )
        {
            Debug.LogError("无法找到StoryID,id = " + id );
            return false;
        }

        return storyState == 0;
    }

//s = height * (j -i) 
    //public int MaxArea(int[] height) {

    //}
}
