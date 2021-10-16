using System.Collections.Generic;
using UnityEngine;

public class NpcMgr:MonoBehaviour
{
    
    static NpcMgr _instance;
    public static NpcMgr instance
    {
        get => _instance ??(_instance = new NpcMgr());
        set => _instance = value;
    }
    
    public Dictionary<string , NpcCtl> _GCtlList = new Dictionary<string , NpcCtl>();
    public Dictionary<string, IStoryConfig> _GCfgList = new Dictionary<string ,IStoryConfig>(); 
    public void init(){

        _GCfgList.Add("YangHuiMin", YangHuiMinConfig.instance);
    }


    public void AddCtl(string name , NpcCtl ctl){
        _GCtlList.TryGetValue(name,out NpcCtl tempCtl);
        if(tempCtl != null)
        {
            return ;
        }
        _GCtlList.Add(name,ctl);
    }

    public NpcCtl GetCtl(string name){

        NpcCtl tempCtl ;
        _GCtlList.TryGetValue(name,out tempCtl);
        if(tempCtl == null)
            Debug.LogError("no tempCtl");

        return tempCtl;
    }   
}