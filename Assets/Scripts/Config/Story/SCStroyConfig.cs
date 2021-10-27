/// story Manager

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

[Serializable]
public enum Charater
{
    Player,
    ChenLaoShi,
}

[Serializable]
public enum MoveType
{
    None,
    Move,
    Dialog
}

public enum DialogPosition
{
    Left,
    Right
}

//https://aihailan.com/unity-odin-inspector-tutorials/ herer

[Serializable]
public class DialogInfo
{

    [OnValueChanged("ChangeMoveType"), GUIColor(0.3f, 1f, 1f)]
    public MoveType moveType;
    private void ChangeMoveType()
    {
        isMove = moveType == MoveType.Move;
        isDialog = moveType == MoveType.Dialog;
    }

    [HideInInspector]
    public bool isMove;
    [HideInInspector]
    public bool isDialog ;
    [BoxGroup("Talk") , ShowIf("isDialog")]
    public Charater charater;
    [BoxGroup("Talk"), ShowIf("isDialog")]
    public DialogPosition dialogPosition;
    [BoxGroup("Talk") , ShowIf("isDialog")]
    public string talkInfo;
}

[CreateAssetMenu(menuName = "Config/StoryConfig", fileName = "StoryConfigDefault.asset")]
public class SCStroyConfig : ScriptableObject
{

    [ListDrawerSettings(OnBeginListElementGUI = "BeginDrawListElement", OnEndListElementGUI = "EndDrawListElement")]
    public List<DialogInfo> storyDialogList;
    private void BeginDrawListElement(int index)
    {
        SirenixEditorGUI.BeginBox("ÐÐ¶¯" + index);
    }
    private void EndDrawListElement(int index)
    {
        SirenixEditorGUI.EndBox();
    }
}
