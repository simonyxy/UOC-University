using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FrameCtl : MonoBehaviour
{
    private GameObject frameObject;
    private FrameSwitch fIns;
    private PolygonCollider2D framePolyCod ;
    // private Tilemap tileUI ;
    // private Vector2 tileSizes;
    // private Vector2 tileCenter;
    private void Awake() {

        //第一个物体必须是场景Obj
        frameObject = transform.GetChild(0).gameObject;
        // tileUI = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();
        // tileSizes = (Vector3)tileUI.size;
        // tileCenter = tileUI.transform.position;
        framePolyCod = GetComponent<PolygonCollider2D>();
        Vector2[] point = {Vector2.zero,new Vector2(10,10)};
        // framePolyCod.SetPath(0,point);
        frameObject.SetActive(false);
    }
    private void Start() {

        fIns = FrameSwitch.GetInstance();
    }

    //
    private void OnTriggerEnter2D(Collider2D other) {

        if(other.tag == "Player" )
        {
            fIns.SetCurFram(framePolyCod,frameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        
        if(other.tag == "Player" )
        {
            fIns.swichFrame(framePolyCod,frameObject);
        } 
    }

    private void OnDrawGizmos() {
        if(!framePolyCod)
            framePolyCod = GetComponent<PolygonCollider2D>();

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(framePolyCod.bounds.center,framePolyCod.bounds.size);
    }

    //https://www.cnblogs.com/yzxhz/p/12753670.html
}
