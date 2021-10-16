/* 
* @Author: yxy
* @desc: 卡通风格的边缘检测，目前只能用于3D
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class edgeHard : PostEffectBase
{
    public Shader edgeDetectShader;
    public Material edgeDetectMaterial = null;
    public Material material
    {
        get
        {
            edgeDetectMaterial = CheckShaderAndCreatMaterial(edgeDetectShader,edgeDetectMaterial);
            return edgeDetectMaterial;
        }
    }

    [Range(0.0f,1.0f)]
    public float edgesOnly = 0.0f;
    public Color edgeColor = Color.black;
    public Color backgroundColor = Color.white;
    public float sampleDistance = 1.0f ; //距离越远，描边越宽
    public float sensitivityDepth = 1.0f; //这个下面这个参数决定了怎么样才能算是一条边
    public float sensitivityNormals = 1.0f;

    private void OnEnable()
    {
        //需要获得深度+法线纹理
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
    }

    [ImageEffectOpaque]
    private void OnRenderImage(RenderTexture source , RenderTexture destination)
    {
        if(material != null)
        {
            material.SetFloat("_EdgeOnly",edgesOnly);
            material.SetColor("_EdgeColor",edgeColor);
            material.SetColor("_BackgroundColor",backgroundColor);
            material.SetFloat("_SampleDistance",sampleDistance);
            material.SetVector("_Sensitivity",new Vector4(sensitivityNormals,sensitivityDepth,0,0));
            Graphics.Blit(source , destination,material);
        }
        else{
            Graphics.Blit(source,destination);
        }
    }
}

