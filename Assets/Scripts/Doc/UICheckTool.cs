using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.SceneManagement;


public class  UICheckTool : MonoBehaviour {

	[MenuItem("FindUI/找到UI")]  //保存后就会在unity菜单栏中出现MyMenu的菜单栏
    static void FindUI()          //和二级目录Do Something,点击它就会执行DoSomething()方法
    {
    	GameObject go = GameObject.Find("MainCamera");
		go.AddComponent<UICheckTool>();
		go.GetComponent<UICheckTool>().uitype  =FindType.UITOP;
    }
	// [MenuItem("FindUI/找到对应Canvas")]
	// static void FindUI2(){
	// 	GameObject go = GameObject.Find("MainCamera");
	// 	go.AddComponent<UICheckTool>();
	// 	go.GetComponent<UICheckTool>().uitype =FindType.UIBOTTOM;
	// }

 	// [MenuItem("FindUI/找到UI停止游戏")]
	// static void FindCanvas(){
	// 	GameObject go = GameObject.Find("MainCamera");
	// 	go.AddComponent<UICheckTool>();
	// 	go.GetComponent<UICheckTool>().uitype =FindType.StopUI;
	// }	
	bool isTop ;//是否反映最上层UI
	private EventSystem _mEventSystem;
	PointerEventData pointerEventData;
	enum FindType{
		UITOP,
		UIBOTTOM,
		StopUI,
	}
	FindType uitype;   //查找类型
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {	
			IsRaycastUI(uitype);
        }
    }
	
	bool IsRaycastUI(FindType uitype)
	{
		if (EventSystem.current == null)
		{	 Debug.Log("nothing ");}
			//鼠标点击事件
			pointerEventData = new PointerEventData(EventSystem.current);
			//设置鼠标位置
			pointerEventData.position = Input.mousePosition;
			//射线检测返回结果
			List<RaycastResult> results = new List<RaycastResult>();
			//检测UI
			//graphicRaycaster.Raycast(pointerEventData, results);
			EventSystem.current.RaycastAll(pointerEventData, results);
			//打印结果
			for (int i = 0; i < results.Count; i++)
			{
				if(uitype == FindType.UITOP)
				{
					if(results[i].depth != 0)//脚本里面depth == 0 的是背景板 ， UI都是有depth的	
					{ 			
						//调用显示到Hierarchy中
						EditorGUIUtility.PingObject(results[i].gameObject);
						Selection.activeGameObject =results[i].gameObject;	
						break;	 //break 的原因是， 只返回最上层的UI			
					}
				}
				// if(uitype ==FindType.UIBOTTOM){
				// 	if(results[i].depth != 0)//脚本里面depth == 0 的是背景板 ， UI都是有depth的	
				// 	{ 			
				// 		//调用显示到Hierarchy中
				// 		EditorGUIUtility.PingObject(results[i].gameObject);
				// 		Selection.activeGameObject =results[i].gameObject;			
				// 	}
				// }
				// if(uitype==FindType.StopUI)
				// {	
				// 	if(results[i].depth != 0)//脚本里面depth == 0 的是背景板 ， UI都是有depth的	
				// 	{ 			
				// 		//调用显示到Hierarchy中					
				// 		EditorGUIUtility.PingObject(results[i].gameObject);
				// 		Selection.activeGameObject =results[i].gameObject;
				// 		if(Time.timeScale == 0){
				// 			Time.timeScale =1;
				// 		}
				// 		else {
				// 			Time.timeScale =0;
				// 		}
				// 		break;
				// 	}	
				// }
		}
		//返回值>0，即点击到UI
		return results.Count > 0;
	}
}
