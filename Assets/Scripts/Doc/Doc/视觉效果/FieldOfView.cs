using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    //public static float GetAngleFromVectorFloat(Vector3 dir)
    //{
    //    dir = dir.normalized;
    //    float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    if (n < 0)
    //    {
    //        n += 360;
    //    }
    //    return n;
    //}

    //哪些层级可以遮挡我们的光线
    [SerializeField]private LayerMask layerMask;
    private Mesh mesh ;
    //视角角度
    public float fov ;
     Vector3 origin ;
    private void Start(){
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 90f;
        origin = Vector3.zero;
    }
    private void Update(){
        origin = transform.position;
    }
    private void LateUpdate(){
    

        //三角形数，如果数目越大，视角越平滑。
        //其实是画了两个三角形拼成一个视角，
        int rayCount = 50 ;
        float angle = 0f;
        float angleIncrease = fov / rayCount;
        //可视范围
        float viewDistance = 50f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1 ];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];
        // Debug.LogError(origin);
        vertices[0] = origin;

        int vertexIndex = 1 ;
        int trianglesIndex = 0 ;
        for( int i = 0 ; i<= rayCount ; i++)
        {
            Vector3 vertex; 

            //检测是否有碰撞
            RaycastHit2D raycastHit2D =  Physics2D.Raycast(origin, GetVectorFromAngle(angle) , viewDistance,layerMask);

            if(raycastHit2D.collider == null)
            {

                //NOHIT
                //如果没有碰撞到物体，顶点将会在我们设置好的距离之外最大距离
                vertex = origin + GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                //Hit Object
                //如果碰撞到了物体，那么顶点就在碰撞的碰撞体的位置
                // Debug.LogError("ion");
                vertex = raycastHit2D.point;
                Debug.LogError(vertex);
            }
            //
            vertices[vertexIndex] = vertex;

            if(i>0 ){
                triangles[trianglesIndex + 0 ]= 0 ;
                triangles[trianglesIndex + 1 ]= vertexIndex -1 ;
                triangles[trianglesIndex + 2 ]= vertexIndex ;

                trianglesIndex += 3 ;

            }
            vertexIndex++;
            angle -= angleIncrease;
        }
        

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        }


    //在角色移动中设置这两个方法

    //设置起始点

    //public void SetOrigin(Vector3 origin)
    //{
    //    this.origin = origin;
    //}


    ////瞄准方向
    //public void SetAimDirection(Vector3 aimDirection)
    //{
    //    startAngle = GetAngleFromVectorFloat(aimDirection) - fov / 2f;
    //}

}
  
   
    //
    

    
    

//}
