---
title:  解决mask无法裁剪其他的Layer和Renderer问题
tag:  lua开发
---
# 前言
**Mask无法对其他层级以及Renderer进行屏蔽，通过手写代码屏蔽（消耗比较大）**
<!-- more -->
### 不适合用Stencil裁剪时，使用Rect来裁剪特效的方式

当然这个方法比较消耗性能，因为需要获得子物体的所有的界面重新去渲染他的Renderer，但是目前没有想到更好的办法
 基本上要用于有特别多子物体的组件去做消耗，Scroll View下面会是一个很好的选择~ 
[MenuItem("Tool/现实点中 GameObject #&w")]   后面的自定义快捷键加在后面，可以在百度搜到！！！！
unity自带的查看在Preferences 里面的keys


```c#
//添加到组件，获取Transform 和Shader 
using UnityEngine;
[ExecuteInEditMode]
public class UIClipAble:MonoBehaviour
{
    static readonly int clipRectId = Shader.PropertyToID("_ClipRect");
    static readonly int opClipId   = Shader.PropertyToID("_ClipOpen");
    static readonly Vector3[] corners = new Vector3[4];

    RectTransform mask;
    MaterialPropertyBlock materialBlock;
    Renderer[] renderers;

    private void OnEnable()
    {
        materialBlock = new MaterialPropertyBlock();
        renderers = GetComponentsInChildren<Renderer>();
        ResetMask();
    }

    private void OnDisable()
    {
        mask = mask ?? GetComponent<RectTransform>();
        if(mask != null)
        {
            materialBlock.SetFloat(opClipId,0);
            foreach(Renderer renderer in renderers)
            {
                if(renderer != null)
                    renderer.SetPropertyBlock(materialBlock);
            }
        }
    }

    void ResetMask(){
        mask = mask ?? GetComponent<RectTransform>();
        if(mask != null)
        {
            mask.GetWorldCorners(corners);
            Vector4 clipRect = new Vector4(corners[0].x , corners[0].y,corners[2].x,corners[2].y);
            materialBlock.SetVector(clipRectId,clipRect);
            materialBlock.SetFloat(opClipId,1);
            foreach(Renderer renderer in renderers)
            {
                renderer.SetPropertyBlock(materialBlock);
            }
        }
    }

}
```