using UnityEngine;

public class TimerUseExample : MonoBehaviour
{
    void Start()
    {
        Timer a = new Timer( "text" , 3 , null , () => {
            Debug.LogError("in");
        },null,false);


        TimerManager.instance.AddTimer(a);
    }
} 