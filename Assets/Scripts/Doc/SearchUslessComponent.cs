using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class SearchUslessComponent : EditorWindow
{
    ///
    // desc:查找图片引用
    ///
    Vector2 scolPos;

    [MenuItem("FindUI/搜索违规UI")]
    static void SearchRefrence()
    {
        SearchUslessComponent window = (SearchUslessComponent)EditorWindow.GetWindow(typeof(SearchUslessComponent),false,"Searching",true);
        window.Show();
    }
    private static Object[] searchText = new Object[4];
    private List<GameObject> result = new List<GameObject>();
    private void OnGUI()
    {
        Font fontObject = new Font();
        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space();
        int Textlength = searchText.Length;
        for(int i = 0 ; i< Textlength ; i ++)
        {
            searchText[i] = (Object)EditorGUILayout.ObjectField(searchText[i],typeof(Object),true);
        }
        if(GUILayout.Button("获取没有以上字体的prefab(把字体拉进来)",GUILayout.Width(300)))
        {
            if(searchText[0] == null )
            {
                Debug.LogError("没有输入参数");
                return;

            }
            result.Clear();

            #region 获得拖入字体的GUID
            if(searchText[0]==null) return ;
            //字体的路径
            string[] assetPath = new string[Textlength];
            //字体的唯一识别id
            string[] assetGuild = new string[Textlength];
            for(int i = 0 ; i< Textlength;i++)
            {
                assetPath[i] = AssetDatabase.GetAssetPath(searchText[i]);
                //第i个字体的唯一识别id
                assetGuild[i] = AssetDatabase.AssetPathToGUID(assetPath[i]);
            }
            #endregion

            #region 获得所有项目prefab
            //所有prefab guid
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/ABRes/UI/Prefabs"});
            int length = guids.Length;
            #endregion

            #region 筛选出没有合法字体的prefab，保存进result
            //正则类
            MatchCollection mc;
            //所有的text字体在GUID中的默认正则匹配模式
            string pattem = @"m_Font: {fileID: [0-9]*, guid: .*, type: [0-9]*";
            for(int i = 0 ; i <length ; i++)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guids[i]);
                // bool cancel = EditorUtility.DisPlayCancelableProgressBar("Checking",filePath,(float)i,(float)length);
                bool cancel = EditorUtility.DisplayCancelableProgressBar("Checking",filePath,(float)i/(float)length);
                if(cancel)
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
                //获得prefab的所有组建的内容
                string content = File.ReadAllText(filePath);
                //把prefab下所有的文本通过正则找出来
                mc = Regex.Matches(content,pattem);

                //判断是否有误用字体的布尔值，默认为true
                bool isUnRule = true;
                for(int k = 0 ; k < mc.Count ; k++){
                    Match m = mc[k];

                    for(int j = 0 ; j< Textlength;j++)
                    {
                        //查看是否匹配合理的字体
                        if(m.Value.Contains(assetGuild[j]))
                        {
                            //如果这个文本匹配了合理的字体，去看下一个文本是否匹配
                            isUnRule =false;
                            break;
                        }
                    }
                    //没有匹配到的字体，说明有不合理的文本存在
                    if(isUnRule)
                    {

                        GameObject fileObject = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                        result.Add(fileObject);
                        break;
                    }
                } 
            }
            EditorUtility.ClearProgressBar();
            #endregion
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();









        //修改这些prefab的字体
        if(GUILayout.Button("把Arial和丢失的字体修改为tengxiang",GUILayout.Width(230)))
        {
            if(result.Count ==0 ){
                Debug.LogError("请先点击搜索");
                return ;
            }
            string[] fonts = AssetDatabase.FindAssets("t:Font",new[] {"Asset/ABRes/Fonts"});
            for(int i = 0 ;i <fonts.Length;i++)
            {
                string fontPath = AssetDatabase.GUIDToAssetPath(fonts[i]);
                //目标字体
                if (fontPath == "Assets/ABRes/Fonts/tengxiang.ttf")
                {
                    fontObject = AssetDatabase.LoadAssetAtPath<Font>(fontPath);
                    Debug.LogError("当前字体为" + fontObject.name);
                }
            }
            if(fontObject == null)
            {
                Debug.LogError("没有找到替换的字体");
                return;
            }
            int length = result.Count;
            for(int i = 0 ; i<length;i++)
            {
                Text[] texts = result[i].GetComponentsInChildren<Text>();
                bool cancel = EditorUtility.DisplayCancelableProgressBar("Changing",result[i].name,(float)i/(float)length);
                if(cancel)
                {
                    EditorUtility.ClearProgressBar();
                    break;
                }
                //修改预置体
                for(int j = 0 ;j< texts.Length;j++)
                {
                    Text text = texts[j];
                    string fontName = "";
                    if(text.font)
                    {
                        fontName = text.font.name;
                    }
                    Debug.LogError(fontName);
                    if((fontName=="")|| (fontName == "Arial"))
                    {
                        texts[j].font = fontObject;
                    }
                }
                //保存预置体
                PrefabUtility.SavePrefabAsset(result[i]);
            }
            EditorUtility.ClearProgressBar();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        //显示结果
        scolPos = EditorGUILayout.BeginScrollView(scolPos,GUILayout.MaxHeight(1000));
        for(int i = 0 ; i< result.Count;i++)
        {
            EditorGUILayout.ObjectField(result[i],typeof(Object),true,GUILayout.Width(300));
        }

        EditorGUILayout.EndScrollView();


    }

}