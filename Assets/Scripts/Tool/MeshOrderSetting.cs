using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MeshOrderSetting : MonoBehaviour
{
   public string layName = "player";
    public int orderNum = 0;

    private Renderer _render;

    public Renderer render 
    {
        get{
            if(null == _render)
                _render = gameObject.GetComponent<Renderer>();
            return _render;
        }
    }


    void Awake(){
        ReOrder();
    }

    public void ReOrder()
    {
        var r = gameObject.GetComponent<Renderer>();
        if(null != r)
        {
            r.sortingLayerName = layName;
            r.sortingOrder =orderNum;
        }
    }

    #if UNITY_EDITOR
    protected void OnValidate(){
        ReOrder();
    }
    #endif

    public void SetOrder(int order)
    {
        orderNum = order;
        render.sortingOrder = order;
    }
}
