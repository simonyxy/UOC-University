//后处理检查和生成材质和shader底层脚本

using System.Collections.Generic;
using UnityEngine;

namespace UnityStandarAssets.ImageEffects
{
    [ExecuteInEditMode]
    [RequireComponent (typeof(Camera))]
    public class PostEffectsBase2:MonoBehaviour
    {
        protected bool supprotHDRTexture =true ;
        protected bool supprotDX11 = false;
        protected bool isSupported =true ;

        private List<Material> creatMaterials = new List<Material>();

        protected Material CheckShaderAndCreatMaterial(Shader s , Material m2Create)
        {
            if(!s)
            {
                //Log出错提示
                enabled =false ; 
                return null;
            }

            if(s.isSupported && m2Create && m2Create.shader ==s )
            return m2Create;

            if(!s.isSupported)
            {
                NotSupported();

                Debug.LogError("The shader" + s.ToString() + "不支持在这个平台");
                return null;
            }
            m2Create = new Material(s);
            creatMaterials.Add(m2Create);
            m2Create.hideFlags = HideFlags.DontSave;

            return m2Create;
        }
        protected Material CreatMaterial(Shader s , Material m2Create)
        {
            if(!s){
                //////////////////
                //Debug.Log出错提示 //
                //////////////////
                return null;
            }

            if(m2Create && (m2Create.shader == s) && (s.isSupported))
                return m2Create;

            if(!s.isSupported)
            {
                return null;
            }

            m2Create = new Material(s);
            creatMaterials.Add(m2Create);
            m2Create.hideFlags = HideFlags.DontSave;

            return m2Create;
        }
        void OnEnable(){
            isSupported =true ;
        }

        void OnDestroy(){
            RemoveCreateMaterials();
        }

        private void RemoveCreateMaterials(){
            while(creatMaterials.Count > 0 )
            {
                Material mat = creatMaterials[0];
                creatMaterials.RemoveAt(0);
#if UNITY_EDITOR
                DestroyImmediate(mat);
#else
                Destroy(mat);
#endif
            }
        }
        protected bool CheckSupport()
        {
            return CheckSupport(false);
        }
        public virtual bool CheckResources(){
            return isSupported;
        }

        protected bool CheckSupport(bool needDepth)
        {
            isSupported =true ;
            supprotHDRTexture = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
            supprotDX11 = SystemInfo.graphicsShaderLevel >=50 && SystemInfo.supportsComputeShaders;

            // if(!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
            // {
            //     NotSupported();
            //     return false;
            // }
            if(needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth)){
                NotSupported();
                return false;
            }

            if(needDepth)
            {
                GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
            }
            return true;
        }
        protected bool CheckSupport(bool needDepth,bool needHdr)
        {
            if(!CheckSupport(needDepth))
                return false;

            if(needHdr && !supprotHDRTexture)
            {
                NotSupported();
                return false;
            }
            return true;
        }

        public bool Dx11Support(){
            return supprotDX11;
        }

        protected void ReportAutoDisable(){
            //输出错误信息
        }
        bool CheckShader(Shader s)
        {
            if(!s.isSupported)
            {
                NotSupported();
                return false;
            }
            else
            {
                return false;
            }
        }

        protected void NotSupported()
        {
            enabled =false;
            isSupported =false;
            return ;
        }

        protected void DrawBorder(RenderTexture dest , Material material)
        {
            float x1;
            float x2;
            float y1;
            float y2 ;

            RenderTexture.active = dest ; 

            bool invertY = true;

            GL.PushMatrix();
            GL.LoadOrtho();

            for(int i = 0 ; i<material.passCount;i++)
            {
                material.SetPass(i);

                float y1_;float y2_;
                if(invertY )
                {
                    y1_ =1.0f;
                    y2_ =0.0f;
                }
                else
                {
                    y1_ =0.0f;
                    y2_ =1.0f;
                }

                //left 
                x1 = 0.0f;
                x2 = 0.0f +1.0f/(dest.width *1.0f);
                y1 =0.0f;
                y2 = 1.0f;
                GL.Begin(GL.QUADS);
                

                GL.TexCoord2(0.0f,y1_);GL.Vertex3(x1,y1,0.1f);
                GL.TexCoord2(1.0f,y1_);GL.Vertex3(x2,y1,0.1f);
                GL.TexCoord2(1.0f,y2_);GL.Vertex3(x2,y2,0.1f);
                GL.TexCoord2(0.0f,y2_);GL.Vertex3(x1,y2,0.1f);

                //right 
                x1 = 1.0f -1.0f/(dest.width * 1.0f);
                x2= 1.0f;
                y1= 0.0f;
                y2 = 1.0f;

                GL.TexCoord2(0.0f,y1_);GL.Vertex3(x1,y1,0.1f);
                GL.TexCoord2(1.0f,y1_);GL.Vertex3(x2,y1,0.1f);
                GL.TexCoord2(1.0f,y2_);GL.Vertex3(x2,y2,0.1f);
                GL.TexCoord2(0.0f,y2_);GL.Vertex3(x1,y2,0.1f);

                //top 
                x1 = 0.0f;
                x2= 1.0f;
                y1= 0.0f;
                y2 = 0.0f  + 1.0f/(dest.height * 1.0f);
                
                GL.TexCoord2(0.0f,y1_);GL.Vertex3(x1,y1,0.1f);
                GL.TexCoord2(1.0f,y1_);GL.Vertex3(x2,y1,0.1f);
                GL.TexCoord2(1.0f,y2_);GL.Vertex3(x2,y2,0.1f);
                GL.TexCoord2(0.0f,y2_);GL.Vertex3(x1,y2,0.1f);

                //top 
                x1 = 0.0f;
                x2= 1.0f;
                y1= 1.0f  - 1.0f/(dest.height * 1.0f);
                y2 =  1.0f;
                GL.TexCoord2(0.0f,y1_);GL.Vertex3(x1,y1,0.1f);
                GL.TexCoord2(1.0f,y1_);GL.Vertex3(x2,y1,0.1f);
                GL.TexCoord2(1.0f,y2_);GL.Vertex3(x2,y2,0.1f);
                GL.TexCoord2(0.0f,y2_);GL.Vertex3(x1,y2,0.1f);

                GL.End();
            }
            GL.PopMatrix();
        }
    }
}