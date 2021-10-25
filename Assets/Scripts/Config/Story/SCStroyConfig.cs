/// story Manager

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum Charater
{
    Player,
    ChenLaoShi,
}
//https://aihailan.com/unity-odin-inspector-tutorials/ herer

[Serializable]
public class DialogInfo
{

    public Charater charater;
    public string talkInfo;
}

[CreateAssetMenu(menuName = "Config/StoryConfig", fileName = "StoryConfigDefault.asset")]
public class SCStroyConfig : ScriptableObject
{

    public List<DialogInfo> dialogList;

}
