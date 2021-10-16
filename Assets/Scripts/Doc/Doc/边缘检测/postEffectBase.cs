using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]

public class PostEffectBase : MonoBehaviour
{
    //检查是否能用屏幕后处理的脚本
    protected void CheckResources()
    {
        bool isSupported = CheckSupport();
        if(isSupported == false)
        {
            NotSupported();
        }
    }

    protected bool CheckSupport(){
        if(SystemInfo.supportsImageEffects ==false /*|| SystemInfo.supportsRenderTextures == false*/)
        {
            Debug.LogError("这个平台不支持");
            return false;
        }
        return true ;
    }

    protected void NotSupported()
    {
        enabled = false;
    }

    //制定shader作为渲染纹理的材质
    protected Material CheckShaderAndCreatMaterial(Shader shader , Material material)
    {
        if(shader == null )
        return null;

        if(shader.isSupported && material && material.shader == shader)
        return material;

        if(!shader.isSupported)
            return null;
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if(material)
            return material;
            else
            return null;
        }
    }

    void Start()
    {
        CheckResources();
    }
}