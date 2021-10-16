using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcNormal : IState
{
    private readonly NpcCtl npcCtl;

    private BoxCollider2D colMoveArea;
    private BoxCollider2D colNpc;
    // private Rigidbody2D rigSel;


    private Bounds boundNpc;
    
    private GameObject imgDialog;
    private TextMesh txtDialog;

    public NpcNormal(NpcCtl npcIns ,BoxCollider2D colNpc,Bounds bounds)//Rigidbody2D rigSel)
    {
        this.npcCtl =  npcIns;
        this.colNpc = colNpc;
        // this.rigSel = rigSel;
        boundNpc = bounds;
        // this.imgDialog = imgDialog;
        // this.isRandomMove = isRandomMove;
    }

    void IState.Tick(){
        if(npcCtl.isRandomMove)
        npcCtl._StartMoveByType(npcCtl.npcCurState);
    }

    void IState.OnEnter(){
        
        npcCtl.BeginCheckMoveMent();
    }
    

    void IState.OnExit()
    {

    }

    void IState.TiggerEvent(string Sevent){
        if(Sevent == "PLAYER_CHOSE_UP")
        {
            npcCtl.CheckShowDiagUp();
        }

        if (Sevent == "PLAYER_HIT_NPC")
        {
            npcCtl.CheckShowDiagNear();
        }
    }

    
}
