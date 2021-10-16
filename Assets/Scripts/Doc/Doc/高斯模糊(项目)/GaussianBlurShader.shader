Shader "Custom/GaussianBlur"
{
    Properties
    {
        _MainTex("Base(RGB)",2D) = "white"{}
        _Color("Main Color",Color) = (1,1,1,1)
    }

    //通过CGINCLUDE我们可以预定一些下面在Pass中用到的struct以及函数，
    //这样在pass中只需要设置渲染状态和调用函数就行，shader更加简明了
    
    CGINCLUDE
    #include "UnityCG.cginc"

    //blur结构体，从blur的vert函数传递到frag函数的参数
    struct v2f_blur{
        float4 pos:SV_POSITION; //顶点位置
        float2 uv:TEXCOORD0;    //纹理坐标
        float4 uv01 :TEXCOORD1; //一个vector4存储两个纹理坐标
        float4 uv23 :TEXCOORD2; //一个vector4存储两个纹理坐标
        float4 uv45 :TEXCOORD2; //一个vector4存储两个纹理坐标
    };

    //shader中使用到的参数
    sampler2D _MainTex;
    //XX_TexelSize，XX纹理的像素相关大小width，height对应纹理的分辨率，x = 1/width,y=1/height,z=width,w=height
    float4 _MainTex_TexelSize;
    //给一个offset，这个offset可以再外面设置，是我们设置横向和竖向blur关键参数
    float4 _offsets;
    float4 _Color;

    //vertex shader
    v2f_blur vert_blur(appdata_img v)
    {
        v2f_blur o;
        o.pos = UnityObjectToClipPos(v.vertex);
        //uv坐标
        o.uv =v.texcoord.xy;

        //计算一个偏移值，offset可能是(0,1,0,0)也可能是(1,0,0,0)这样就表示了横向和竖向像素周围的点
        _offsets *= _MainTex_TexelSize.xyxy;

        //由于uv可以存储四个值，所以一个uv保存两个vector坐标，_offsets.xyxy*float4(1,1,-1,-1)可能表示(0,1,0,-1)，表示像素上下两个坐标，
        //也可能是(1,0,-1,0)表示左右两个像素点的坐标，下面*2.0，*3.0同理
        o.uv01 = v.texcoord.xyxy + _offsets.xyxy * float4(1,1,-1,-1);
        o.uv01 = v.texcoord.xyxy + _offsets.xyxy * float4(1,1,-1,-1) *2.0;
        o.uv01 = v.texcoord.xyxy + _offsets.xyxy * float4(1,1,-1,-1) *3.0;
        return o;
    }
    //fragment shader
    fixed4 frag_blur(v2f_blur i) :SV_Target
    {   
        fixed4 color = fixed4(0,0,0,0);
        //将像素本身以及左右像素(上下或者左右，取决于vertex传进来的坐标)像素值取加权平均
        color += 0.4 * tex2D(_MainTex , i.uv);
        color += 0.15 * tex2D(_MainTex , i.uv.xy);
        color += 0.15 * tex2D(_MainTex , i.uv.zw);
        color += 0.10 * tex2D(_MainTex , i.uv.xy);
        color += 0.10 * tex2D(_MainTex , i.uv.zw);
        color += 0.05 * tex2D(_MainTex , i.uv.xy);
        color += 0.05 * tex2D(_MainTex , i.uv.zw); 

        return color * _Color;
    }

    ENDCG

    ////////////////
    //开始SubShader //
    ////////////////
    SubShader
    {
        Pass{
            //后处理的标配
            ZTest Always
            Cull Off
            Zwrite Off
            Fog{Mode off}

            CGPROGRAM
            #pragma vertex vert_blur
            #pragma fragment frag_blur
            ENDCG
        }
    }
    //后处理一般没有fallback ，不行就不处理嘛
}