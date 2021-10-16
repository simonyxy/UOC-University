using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YangHuiMinCtl : NpcCtl
{
    public override string npcName {
        get{
            return  "YangHuiMin";
        }
        set{}
    }

    public override bool isRandomMove { get{return true;} set{}}
    
    public override void _sayHiBtnTrigger(){
        
    }
    
    protected override void Awake() {
        base.Awake();
        NpcMgr.instance.AddCtl(npcName , this);
    }
}
