
using System.Text;
using System.Collections.Generic;
using UnityEngine;
public class NpcconfigBase 
{
    
    // StringBuilder tempStr = new StringBuilder();
    private static NpcconfigBase _instance;
    List<string> tempStrList = new List<string>();
    public NpcconfigBase(){
        Init();
    }

    public virtual void Init(){
        
    }
    public struct playerTiggerType {
        public static int playerUp  = 0 ;
        public static int playerDown  = 1 ;
        public static int playerRight  = 2 ;
        public static int playerLeft  = 3 ;
    }
    
    //类型int ， 好友值 int ，string对话List
    private Dictionary<int , Dictionary<int , List<string>>> npcDiagDic = new Dictionary<int , Dictionary<int , List<string>>>();
    public Dictionary<int , Dictionary<int , List<string>>> NpcDiagDic{
        get{
            return npcDiagDic;
        }
    }
    
    public void addDiagByTypeAndFriend(int type,int friendValue,List<string> value)
    {
        if(npcDiagDic.ContainsKey(type))
        return ;
        //int :好友值 ，List对话内容
        Dictionary<int , List<string>> tempValue = new Dictionary<int , List<string>>();
        tempValue.Add(friendValue,value);
        npcDiagDic.Add(type,tempValue);
    }

    public string GetDiaLogByTypeAndFriend(int type , int frendHp)
    {   
        string tempStr;
        if(!npcDiagDic.ContainsKey(type))
        {
            Debug.LogError("没有这个类型的话啊");
            return null;
        }
        if(!npcDiagDic[type].ContainsKey(frendHp))
        {
            Debug.LogError("没有这个亲密度的话啊");
            return null;
        }
        tempStrList = npcDiagDic[type][frendHp];
        int random = Random.Range(0,tempStrList.Count - 1 );
        tempStr = tempStrList[random];
        return tempStr;
    }
}
