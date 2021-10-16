// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
// using System;
// public class InvokManager : Singleton<InvokManager> ｛
//     private List<MyCallLate> callList = new List<MyCallLate>();
//     private List<MyCallLate> tempCallList = new List<MyCallLate>();
//     /// <summary>
//     /// 添加下帧执行方法
//     /// </summary>
//     /// <param name="handle">执行的方法</param>
//     /// <param name="delayTime">延迟时间</param>
//     public static void Add(Action handle,float delayTime)
//     ｛
//         Instance.add(handle,delayTime);
//     ｝
//     private void add(Action handle, float delayTime)
//     ｛
//         for (int i = 0; i < callList.Count; i++)
//         ｛
//             if (callList[i].Equals(handle))
//             ｛
//                 return;
//             ｝
//         ｝
//         MyCallLate callLate = new MyCallLate(handle,delayTime);
//         callList.Add(callLate);
//         if (callList.Count ==1)
//         ｛
//             MyTickManager.AddUpdate(Invoking);
//         ｝
//     ｝
//     public static void Add(Action<object> handle, float delayTime,object data)
//     ｛
//         Instance.add(handle, delayTime,data);
//     ｝
//     private void add(Action<object> handle, float delayTime, object data)
//     ｛
//         for (int i = 0; i < callList.Count; i++)
//         ｛
//             if (callList[i].Equals(handle))
//             ｛
//                 return;
//             ｝
//         ｝
//         MyCallLate callLate = new MyCallLate(handle, delayTime,data);
//         callList.Add(callLate);
//         if (callList.Count == 1)
//         ｛
//             MyTickManager.AddUpdate(Invoking);
//         ｝
//     ｝
//     public static void Remove(Action handle)
//     ｛
//         Instance.remove(handle);
//     ｝
//     private void remove(Action handle)
//     ｛
//         for (int i = 0; i < callList.Count; i++)
//         ｛
//             if (callList[i].handle.Equals(handle))
//             ｛
//                 callList.Remove(callList[i]);
//                 i--;
//             ｝
//         ｝
//     ｝
//     public static void Remove(Action<object> handle,object data)
//     ｛
//         Instance.remove(handle,data);
//     ｝
//     private void remove(Action<object> handle, object data)
//     ｛
//         for (int i = 0; i < callList.Count; i++)
//         ｛
//             if (callList[i].handleAction.Equals(handle))
//             ｛
//                 callList.Remove(callList[i]);
//                 i--;
//             ｝
//         ｝
//     ｝
//     private void Invoking()
//     ｛
//         tempCallList.Clear();
//         tempCallList.InsertRange(0,callList);
//         float time = Time.time;
//         for (int i = 0; i < tempCallList.Count; i++)
//         ｛
//             MyCallLate task = tempCallList[i];
//             if (task.callTime == time)
//             ｛
//                 continue;
//             ｝
//             float runTime = task.callTime + task.delayTime;
//             if (time<runTime)
//             ｛
//                 continue;
//             ｝
//             task.MyInvoking();
//             callList.Remove(task);
//         ｝
//         if (tempCallList.Count == 0)
//         ｛
//             MyTickManager.RemoveUpdate(Invoking);
//         ｝
//     ｝
//     // 等同于 Invoke 
//     private class  MyCallLate
//     ｛
//         public float callTime;
//         public float delayTime;
//         public Action handle;
//         public Action<object> handleAction;
//         public object data;
//         public MyCallLate(Action handle, float delayTime)
//         ｛
//             this.callTime = Time.time;
//             this.handle = handle;
//             this.delayTime = delayTime;
//         ｝
//         public MyCallLate(Action<object> handleAction, float delayTime,object data)
//         ｛
//             this.callTime = Time.time;
//             this.handleAction = handleAction;
//             this.delayTime = delayTime;
//             this.data = data;
//         ｝
//         public void MyInvoking()
//         ｛
//             if (handle != null) handle();
//             if (handleAction != null) handleAction(data);
//         ｝
//     ｝
// ｝

// 测试：
// public class App : MonoBehaviour ｛
//     void OnEnable()
//     ｛
//         InvokManager.Add(CalculateTime,3);
//     ｝
//     void Update () ｛
//         MyTickManager.Instance.MyUpdate();
//     ｝
//     public void CalculateTime()
//     ｛
//         Debug.Log("CalculateTime");
//     ｝
//     void OnDisable()
//     ｛
//         MyTickManager.RemoveUpdate(CalculateTime);
//     ｝
// ｝
