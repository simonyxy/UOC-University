using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInitViewEx : MonoBehaviour
{
    // Start is called before the first frame update
    private Button btnBegin;
    private Button btnSetting;

    private void Awake() {

        btnBegin = transform.Find("FixHeight/btnBegin").GetComponent<Button>();
        btnSetting = transform.Find("FixHeight/btnSetting").GetComponent<Button>(); 
    }

    void Start()
    {
        btnBegin.onClick.AddListener(onBtnBeginClick);
    }

    private void onBtnBeginClick(){
        SceneManager.LoadSceneAsync(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
