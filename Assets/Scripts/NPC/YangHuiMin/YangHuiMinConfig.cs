
using System;
public class YangHuiMinConfig :Singleton<YangHuiMinConfig>,IStoryConfig
{
    public string name {
         get{ return "YangHuiMin";}
         }
    public string nameChi{
         get{ return "杨惠敏";}
         } 
    public string story {
         get{ 
              return 
              "我的名字叫杨惠民，21岁，在抑郁大学读书，未婚。我希望毕业能为人民群众服务、为社会做出贡献，每天学习到晚上八点才会宿舍。我不抽烟，酒仅止于浅尝。晚上11点睡觉，每天睡足八个小时。睡前，我一定喝一杯温牛奶（学到八点去小卖部买牛奶），然后做二十分钟拉伸与锻炼（以保护我的腰椎）。上了床，马上熟睡，一觉睡到天亮，绝不吧疲劳和压力留到第二天。同学都夸我很优秀。";}
         }
    public int npcID {get {return 0 ;}} 
}