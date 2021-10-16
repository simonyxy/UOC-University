using UnityEngine;

public class GaussianBlurImageCamera:GaussianBlur
{
    public static GaussianBlurImageCamera Instance{get; private set;}
    private GaussianBlurImage m_GaussianBlurImage;
    public GaussianBlurImage BlurImage
    {
        set
        {
            m_GaussianBlurImage =value;
            if(null != value)
            {
                enabled = true ; 
            }
            else
            {
                if(null != cacheTexture)
                {
                    ReleaseTemporary(cacheTexture,true);
                }
            }
        }
        get{
            return m_GaussianBlurImage;
        }
    }

    private RenderTexture cacheTexture;

    private void Awake(){
        Instance = this;
    }

    protected override void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (null== m_GaussianBlurImage)
        {
            enabled =false;
            return ;
        }

        base.OnRenderImage(source,destination);
    }

    protected override void ReleaseTemporary(RenderTexture rt1 , bool real = false)
    {
        if(real)
        {
            RenderTexture.ReleaseTemporary(rt1);
            cacheTexture =null;
        }
        else{
            cacheTexture =rt1 ;
            m_GaussianBlurImage.rawImage.texture =rt1;
            this.enabled =false;
            m_GaussianBlurImage.Show();
            m_GaussianBlurImage = null ;
        }
    }
}


