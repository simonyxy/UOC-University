using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//全局事件系统声明

public class SEventSystem : MonoBehaviour
{
  //1-19
  private static SEventSystem eventInstancs;
  public static SEventSystem EventIns
   {
      get{
        if(null == eventInstancs)
        {
          // eventInstancs = new SEventSystem(); 
          return eventInstancs; 
        }
        else{

           return eventInstancs;
        }
      }
   }

  public void Init (){
  }

   //游戏开始事件
   [HideInInspector] 
   public UnityEvent GAME_LOAD_END;
   //玩家选择
   [HideInInspector] 
   public UnityEvent STORY_0;
   [HideInInspector]
   public UnityEvent BEGIN_STORY; //Story Begin
   [HideInInspector]
   public UnityEvent PLAYER_CHOSE_UP ; //玩家选择上方状态
   [HideInInspector]
   public UnityEvent PLAYER_CHOSE_DOWN ;//
   [HideInInspector]
   public UnityEvent PLAYER_CHOSE_LEFT;
   [HideInInspector]
   public UnityEvent PLAYER_CHOSE_RIGHT;

   [HideInInspector] 
   public UnityEvent TIME_SCALE_FAST;
   [HideInInspector] 
   public UnityEvent TIME_SCALE_SLOW ;

   //时间
   [HideInInspector] 
   public UnityEvent DAY_SWITCH_TIME;
   [HideInInspector] 
   public UnityEvent MAIN_STROY_1;

   //玩家撞击npc
   [HideInInspector] 
   public UnityEvent PLAYER_HIT_NPC;

   
   //玩家健康值
   [HideInInspector] 
   public UnityEvent PLAYER_HEALTH_DOWN;
   [HideInInspector] 
   public UnityEvent PLAYER_HEALTH_UP;
  
   //玩家抑郁值
   [HideInInspector] 
   public UnityEvent PLAYER_SAD_DOWN;
   [HideInInspector] 
   public UnityEvent PLAYER_SAD_UP;

   //进入TimeLine剧情模式
   [HideInInspector]
   public UnityEvent IN_TIMELINE_STORY;
   [HideInInspector]
   public UnityEvent OUT_TINELINE_STORY;
   [HideInInspector]
   public UnityEvent DIALOG_TIMELINE_NORMAL_CLICK;
   [HideInInspector]
   public UnityEvent MUTIDIALOG_TIMELINE_NORMAL_CLICK;
   
   [HideInInspector]
   public UnityEvent STORY_END_TRIGGER;
   
 
    void Awake(){
        eventInstancs = this;
    }

}
    
