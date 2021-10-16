using System.Collections.Generic;
using UnityEngine;
using System;

public class STimer :MonoBehaviour
{
    private static STimer instance;
    public static STimer Instance{
        get{
            if(instance == null)
                instance = new STimer();
            return instance; 
        }
    }

    private List<Action> actList = new List<Action>();
    private List<float> endTimeList = new List<float>();
    private List<float> curTimeList = new List<float>();
    private List<string> actName = new List<string>();
    private List<bool>  isLoopBool  = new List<bool>();

    // private Dictionary<string , Action> actList = new Dictionary<string , Action>();
    public void New(string name, Action act ,float destoryTime )
    {
        New(name,act,destoryTime,false);
    }

    public void New(string name,Action act ,float duartion , bool isLoop)
    {
        if(name != null && act != null)
        {
            
            actList.Add(act);
            actName.Add(name);
            curTimeList.Add(0);
            endTimeList.Add(duartion);
            isLoopBool.Add(isLoop);
        }
    }

    private void Awake(){
        instance = this;
    }

    public void RemoveEvent(string name)
    {
        for(int i = 0 ; i< actName.Count ; i ++)
        {
            if(actName[i] == null)
            continue;
            
            if(actName[i] == name)
            {
                DestroyEvent(i);
            }
        }
    }


    private void Update(){

        if(actList.Count == 0 )
            return ;

        for(int i = 0 ; i< actList.Count ; i++)
        {
            if(curTimeList[i] >= endTimeList[i])
            {
                if(actList[i] != null)
                actList[i]();
                
                if(isLoopBool[i])
                {
                    curTimeList[i] = 0 ;
                }
                else
                {
                    DestroyEvent(i);
                }
            }
            else
            {
                curTimeList[i] += Time.deltaTime;
            }
        }
    }

    
    void DestroyEvent(int i)
    {
        
        actList.RemoveAt(i);
        actName.RemoveAt(i);
        curTimeList.RemoveAt(i);
        endTimeList.RemoveAt(i);
        isLoopBool.RemoveAt(i);
    }
}
