using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBeginMgr : MonoBehaviour {
    
    GameObject UIRoot;
    GameObject GameInit;
    
    void Awake(){

        UIRoot = GameObject.Find("UIRoot");
        // GameInit = GameObject.Find("UIRoot/GameInit");
        
    }

    void Start()
    {

        GameInit = Instantiate<GameObject>(Resources.Load<GameObject>("GameInit"),UIRoot.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
