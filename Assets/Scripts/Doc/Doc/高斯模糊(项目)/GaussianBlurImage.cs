using System.Runtime.CompilerServices;
/* 
* @Author: yxy
* @Date:   2020-09-18 16:34:02
* @desc:   将生成的RT 付给当前Raw Image
*/
using UnityEngine;
using UnityEngine.UI;

public class GaussianBlurImage:MonoBehaviour
{
    public RawImage rawImage;
    private void Awake(){
        rawImage = GetComponent<RawImage>();
        if(null == rawImage ) return ;

        GaussianBlurImageCamera.Instance.BlurImage = this;
    } 

    public void Show(){
        if(null == rawImage) return ;
        rawImage.color = Color.white;
    }

    public void OnDestroy()
    {
        if(this!= GaussianBlurImageCamera.Instance.BlurImage)
        return;
        GaussianBlurImageCamera.Instance.BlurImage =null; 


    }
}

