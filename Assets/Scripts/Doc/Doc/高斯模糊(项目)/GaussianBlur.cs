/* 
* @Author: yxy
* @Date:   2020-09-18 16:02:11
* @desc:   高斯模糊后处理基础，有较强通用性，渲染整个Game环境
*/
using System;
using UnityEngine;
using UnityStandarAssets.ImageEffects;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class GaussianBlur: PostEffectsBase2 
{
    public Color blurColor = Color.white;
    public Shader shader = null;
    protected Material _Material =null;

    //模糊半径
    public float BlurRadius = 1.0f;
    //降低分辨率
    public int downSample =2 ;
    //迭代次数
    public int iteration =1 ;

    public override bool CheckResources(){
        CheckSupport(false);

        _Material = CheckShaderAndCreatMaterial(shader, _Material);

        if(!isSupported)
            ReportAutoDisable();
        return isSupported;
    }

    protected virtual void OnRenderImage(RenderTexture source,RenderTexture destination)
    {
        if(CheckResources() == false)
        {
            Graphics.Blit(source,destination);
            return; 
        }

        _Material.SetColor("_Color",blurColor);

        //申请RenderTexture ,RT的分辨率按照上面的降低
        RenderTexture rt1 =RenderTexture.GetTemporary(source.width >> downSample,source.height >> downSample , 0,source.format );
        RenderTexture rt2 =RenderTexture.GetTemporary(source.width >> downSample,source.height >> downSample , 0,source.format ); 

        //直接将原图拷贝到分辨率的RT上
        Graphics.Blit(source,rt1);

        //进行高斯模糊迭代
        for(int i = 0 ; i< iteration ; i++)
        {
            //第一次高斯模糊，设置offset，竖向模糊
            _Material.SetVector("_offsets",new Vector4(0,BlurRadius,0,0));
            Graphics.Blit(rt1,rt2,_Material);
            //第二次高斯模糊，设置offsets，横向模糊
            _Material.SetVector("_offsets",new Vector4(BlurRadius,0,0,0));
            Graphics.Blit(rt2,rt1,_Material);
        }

        //将结果输出
        Graphics.Blit(rt1,destination);

        //释放申请的两块RenderBuff内容
        ReleaseTemporary(rt1);
        RenderTexture.ReleaseTemporary(rt2);
    }

    protected virtual void ReleaseTemporary(RenderTexture rt1 , bool real =false)
    {
        RenderTexture.ReleaseTemporary(rt1);
    }
}