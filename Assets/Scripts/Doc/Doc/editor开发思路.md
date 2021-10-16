---
title:  unity editor接触思路
tag:  UI
---
# 前言
因为工作的需要，给项目统一更换prefab字体，查找弃用大图，层级点击检测的三个工具。
<!-- more -->
# 前言
如果是需要查找Unity资源中的某些内容，其实是有一个大概的方向的。当然，前提是对Unity.Editor 和 OnGUI方法有一定了解，好去编辑 编辑窗口
[MenuItem]类。这个可以unity中扩展编辑器的方法，声明在工具代码的头部
其次是EditorWindow类，用来创造一个Window界面。声明的方法也是固定的
OnGUI方法中写声明的Window中的工具，
EditorGUI,EditorGUILayout,EditorUtility，GUILayout ,这三个类里面会有UI工具和方法，用作输入和显示
要注意这三个类，如果需要一些反馈的返回值，比如scollView的位置，要把返回值变量申明为全局的，不然返回值是无法实时反馈的。

# AssetDataBase类
unity中对每一个类型的组件的每一个资源都会生成一个.meta文件，.meta文件是记录这个文件的guid，也就是说每个prefab，每个文件都会有一个唯一的guid。fileID是本地的文件id，在记录在每一个prefab中内部的引用关系。然后在.prefab文件中会记录prefab对guid和fileID的引用，所以经常会有联合开发项目的一些引用的丢失，经常会是因为.meta文件没有上传导致的prefab的guid引用丢失。或者是因为更名后.meta文件新生成导致guid引用失败。
那么guid和编辑器工具的开发或者说和assetDataBase有啥关系。

官网的API 对 AssetDataBase定义是“An Interface for accessing assets and performing operations on assets.”

就是对资源修改管理和控制的API

既然GUID是标识一个资源文件的唯一标识符，那么我们当然可以去通过这个GUID和AssetDataBase来查找和修改资源。那么资源管理的工具可就不要太简单了，只要利用AssetDataBase的API想办批量获得项目资源遍历或者通过GUID识别特定的资源就可以拿到想要的资源了。
后续通过unity的Object类，比如一些GetComonent<>的方法去对资源完成修改或者识别保存就可以了。

当然如果想要提高工具效率，我个人建议是直接使用正则去匹配prefab中的内容，而不是直接AssetDataBase 和 Object。

# [Application]类，可以获得当前unity 运行状态的数据。比如
是否在运行状态是否在后台运行等，这可以作为一个辅助的工具

# [Excel]类和Cells类，这些都是可以用来读取其他类型类型脚本的工具，具体需要的时候去查查吧~

1.一个整体的思路，已在项目中完成的工具其实主要的整体思想都是相似的


# 整体思路：
1.UnityEditor类中对项目资源进行一系列操作，然后通过unity内置类保存或者通过UnityEditor类中的方法去对unity进行显示操作

强调：UnityEditor类非常有用，可以多去研究下

1.用于资源管理，比如AssetDataBase可以让我们对项目Project中的资源进行管理，和通过GUID对Prefab的获取和转换或者加载物体
2.用于帮助我们在unity操作界面上进行需要需求的操作，比如可用于显示Inspector的Select类。EditorGUIUtility可以用于操作检视面板上的

3.用于对项目内所有物体修改的方法。比如PrefabUtility可以用于保存修改后的预制体，或者在预制体和物体中添加组件等

辅助：
prefab，GUID,.META 等文件里面存放的内容都是可读的，其实也只是保存物体的信息，很多时候我们不用去获得物体，直接用IO和正则能很大提高效率，特别是项目资源特别多的时候。

共勉~~



 ``` c#
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
 ```


 ``` c#
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
    }
}
 ```
 
<<<<<<< HEAD
//LUA（三色回收） 的垃圾回收的标记阶段不需要暂停线程，它是增量式的，就是很多个小方法逐个进行。所以在垃圾回收进行的同时，可能会有新的对象输入，对此lua也有对应的方法，设置两种white类型
c#（分代回收）的垃圾回收需要把线程全部暂停
=======
作者：Simon YXY