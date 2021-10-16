/* 
* @Author: anchen
* @Date:   2020-09-16 19:27:10
* 使用屏幕后处理的例子
*/
using UnityEngine;
public class BrightnessSetting : PostEffectBase
{
    public Shader Tshader;
    private Material Tmaterial;

    public Material material
    {
        get
        {
            Tmaterial = CheckShaderAndCreatMaterial(Tshader,Tmaterial);
            return Tmaterial;
        }
    } 

    [Range(0f,3f)]
    public float brightness =1.0f;
    [Range(0f,3f)]
    public float saturation =1.0f;
    [Range(0f,3f)]
    public float contrast =1.0f;

    private void OnRenderImage(RenderTexture source , RenderTexture destination)
    {
        if(material != null) 
        {
            material.SetFloat("_Brightness",brightness);
            material.SetFloat("_Saturation",saturation);
            material.SetFloat("_Contrast",contrast);

            Graphics.Blit(source,destination,material);
        }
        else
            Graphics.Blit(source,destination);
    }
}