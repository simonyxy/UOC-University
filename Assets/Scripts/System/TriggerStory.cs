//放在每个触发Story的物体上，这个物体下面还需要有角色移动position顺序
//
using UnityEngine;
using UnityEngine.Playables;

public class TriggerStory : MonoBehaviour
{
    public int storyID;
    public int frustratio;
    //private GameObject objCurStory;
    private BoxCollider2D colTrigger;
    [HideInInspector]public PlayableDirector timeLineCur;

    private void Awake(){

        timeLineCur = transform.GetComponentInChildren<PlayableDirector>();
        colTrigger = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if(other.tag == "Player")
        {
            if(PlayerConfig.instance.CheckCanPlayStory(storyID))
            {

                Debug.LogError("进行故事, id = " +storyID);
                timeLineCur.Play();
                colTrigger.enabled = false;
            }
            else
            {
                Debug.LogError("无法进行故事, id = "+ storyID);
                colTrigger.enabled = false;
            }
        }
    }
}