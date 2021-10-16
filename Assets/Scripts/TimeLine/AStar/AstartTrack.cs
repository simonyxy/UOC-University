using UnityEngine.Timeline;
using UnityEngine;
using UnityEngine.Playables;

[TrackColor(220/255f,120/255f,0f)]
[TrackClipType(typeof(AStarClick))]
[TrackBindingType(typeof(GameObject))]

public class AstartTrack : TrackAsset
{
    public override void GatherProperties(PlayableDirector director ,IPropertyCollector driver)
    {
        #if UNITY_EDITOR
        GameObject ai = director.GetGenericBinding(this) as GameObject;

        if(ai == null) return ;
        var serliziedObject = new UnityEditor.SerializedObject(ai);
        var iterator  = serliziedObject.GetIterator();
        while(iterator.NextVisible(true))
        {
            if(iterator.hasVisibleChildren)
            {
                continue;
            }
            driver.AddFromName(ai,iterator.propertyPath);
        }
        #endif
        base.GatherProperties(director,driver);
    }
}