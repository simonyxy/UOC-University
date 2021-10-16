using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class FrameSwitch : MonoBehaviour
{
    public static FrameSwitch frameCtl;
    [HideInInspector]public GameObject curFrameObject;
    [HideInInspector]public PolygonCollider2D curFrameCollider; 
    private CinemachineConfiner cineConfiner;
    //设置第一场景
    public GameObject FirstFrameObject;


    public static FrameSwitch GetInstance(){
        return frameCtl;
    }
    
    
    private void Awake() {
        frameCtl = this;

        cineConfiner = GameObject.FindGameObjectWithTag("MainCamera").transform.GetChild(0).GetComponent<CinemachineConfiner>();

        if(FirstFrameObject)
        curFrameObject = FirstFrameObject;
    }
   
    
    public void SetCurFram(PolygonCollider2D frameCollider,GameObject IntoframeObject)
    {

        curFrameObject = IntoframeObject;
        curFrameCollider = frameCollider;

        curFrameObject.SetActive(true);
    }


    public void swichFrame(PolygonCollider2D frameCollider,GameObject OutofframeObject){
        
        if(OutofframeObject != curFrameObject){
            OutofframeObject.SetActive(false);
        }
        
        cineConfiner.m_BoundingShape2D = curFrameCollider;
    }
}
