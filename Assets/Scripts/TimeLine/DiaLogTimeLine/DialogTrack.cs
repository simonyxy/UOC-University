using UnityEngine.Timeline;
using UnityEngine;
using UnityEngine.Playables;

[TrackColor(220/255f,200/255f,122/255f)]
[TrackClipType(typeof(DialogClick))]
// [TrackBindingType(typeof(GameObject))]

public class DialogTrack : PlayableTrack
{
    public override void GatherProperties(PlayableDirector director ,IPropertyCollector driver)
    {
        #if UNITY_EDITOR
        // GameObject ai = director.GetGenericBinding(this) as GameObject;

        // if(ai == null) return ;
        // var serliziedObject = new UnityEditor.SerializedObject(ai);
        // var iterator  = serliziedObject.GetIterator();
        // while(iterator.NextVisible(true))
        // {
        //     if(iterator.hasVisibleChildren)
        //     {
        //         continue;
        //     }
        //     driver.AddFromName(ai,iterator.propertyPath);
        // }
        #endif
        base.GatherProperties(director,driver);
    }
}