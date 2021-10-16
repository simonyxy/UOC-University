---
title:  重复切换场景的时候不删除对象池和GameManager
tag:  我的射击游戏
---
**前言**
<!-- more -->
在游戏开发的时候，有时候需要在不停场景中间切换。unity提供了Don'tDestoryOnLoad的方法让我们保存一些全局的变量。
直接调用Don'tDestroyOnLoad 会让你把当前场景的对象保存下来。但是当你再次打开场景的时候又回去重新加载，所以会有两个物体同事存在在场景上
所以像我射击游戏里面的，只有两个场景相互切换的情况下。不能直接使用Don'tDestroyOnLoad的方法

但是我需要把对象池，GameManger这种物体只生成一次的话，那可能需要别的手段
[雨松](http://www.xuanyusong.com/archives/2938)
从大佬的博客里面看到两种方法。
**第一种**
第一种方法是游戏开始的时候进入一个新的Scene，并且整个游戏过程中只执行一次。这个是可行的

 ``` C#
Object[] initsObjects = GameObject.FindObjectsOfType(typeof(GameObject));
		foreach (Object go in initsObjects) {
			DontDestroyOnLoad(go);
		}
```
**第二种**
第二种方法在比较新的unity版本里面已经是不可行的了。因为unity拒绝了在Mono类底下写构造函数
所以我自己想了一个方法，大概的就是，因为不能在局部场景定义需要DontDestroyOnLoad那我们就间接生成
而且需要在加载之前再在全局做一个判断，每一个物体给定一个判断符。通过全局变量去生成，不怕被销毁
没有就加载一个，有就不加载了

+ 声明一个脚本，命名为SceneManager
- 为脚本声明一个全局bool(isOpen),是为了后面物体参数的初始化和游戏逻辑的一些判断
- 声明需要加载的物体的数组
- 为数组的每个对象，添加一个全局bool类型判断之前是否有加载过
- 加载过就不再加载了，没加载过的加载
- 最后在脚本外把需要加载的全局的预制体，声明到数组中就行

**写的代码可能是根据我游戏实际需求的~~~所以有些变量没有说明啦~~**
 ``` C#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneControl : MonoBehaviour
{
    //预制体（不销毁的物体（做成预制体））,外部载入预制体判断
    public GameObject[] DontDestoryObj; 
    //初始化判断符
    private static bool[] isHave;  
    //初始化脚本是否被打开过（打开过就不初始化isHave）
    private static bool isOpen;
    private GameObject clone;//克隆的不销毁物体
     public void Awake()
     {
        if(!isOpen) {
             Debug.Log("ISoPEN:" + isOpen);
            //1.初始化全局变量 clone + obj
            isHave = new bool[DontDestoryObj.Length];
            for(int i =0;i<DontDestoryObj.Length;i++){
                isHave[i] = false;
                Debug.Log("isHAve:" + DontDestoryObj[i].name +"   " +isHave[i] );
            }

        }
        for(int i =0;i<DontDestoryObj.Length;i++){
            if (!isHave[i])
            {
                clone = GameObject.Instantiate(DontDestoryObj[i], transform.position, transform.rotation);
                PlayerPrefs.SetInt("clone"+ DontDestoryObj[i].name,1);
                DontDestroyOnLoad(clone);//切换场景不销毁clone
                isHave[i] = true;
            }
        }
        #region 游戏加载情况说明（isOpen 的用处）
         //除了第一次打开之外，其他情况都需要我们自己手动调用游戏开启逻辑，因为GameManege保存在全局里没有重新加载，里面的方法不会去调用。我们手动调用游戏开启函数
         //如果现在是第一次加载游戏场景 isOpen 在这里为false
         //如果现在是第二此加载场景，isOpen为true ，那么就会走这个逻辑
         #endregion
        if(isOpen){
            GameManager.gameManager_ins.StartGame();
        }
        //最后再把isOpen 设为 true；
        if(!isOpen){
             isOpen = true;
        }
     }
}
``` 


作者：Simonyxy