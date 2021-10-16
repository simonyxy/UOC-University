
// 总算勉强完成了，碰到了以下问题：

// 1.使用KeyCode赋初始值后，后面自定义了其他按键，那么这个KeyCode会有两个按键值。

// 2.自定义键盘按键时，要把鼠标按键屏蔽掉，不然你鼠标执行的最后一步，KeyCode会获取你的鼠标值。

// 3.以上代码虽然可以实现自定义按键，但是做法比较笨，没有可扩展性，只有一个思路；大家有更好的思路可以回复我，大家一起学习。





// 2-9 
//把选择panel的按钮选项加上
//2-9
//把选择panel分离出来写一个单独的脚本


//Playable graph playableGraph Visualizer


///DOTs
//cpu bound  , cpu卡住了 gpu等待
//gpu bound 反过来
//DOT 就是为了性能而生的东西


//插件 
//post-poccessing

//2-12 timeline 对话系统中 ，对话播放之后不会暂停，需要设置一下playable暂停，并且在按下K键或者选择完之后继续


//2-18
//优化时间

// 2-22开会内容
//游戏重点之一，挫折
// 1.发生挫折 （克服或无视)增减抑郁质。 （例：克服具体形式：对话（可有多次，和一次），或者每天到某个地方做某件事情)
// 2.上课的小游戏 。 

//3.玩家状态（保持每天状态） ， 点击npc进行对话，根据状态。必经剧情或者点击后发生剧情 。（点击后的背景故事）。
//4.npc要出现在排行榜上 。 
//5.思考每天跟npc的对话内容。
//6.玩家跟npc 背包内容
//7.写挫折
//8.民哥之死 (死后回去，不用重新开档)


//其他：
//每周内容汇报
//所有人： 1.写出一个人物 （背景，状态，互动）  2.2个挫折 。 timeline学习（剧情设置）

//1.研究出 Timeline 里面的View 触发后暂停后，怎么给他继续运行下去~~~~

//  2-25
//Timeline 里面的View 触发后 , 增加是否隐藏选项     1
//写一个npc状态类           
// npc 和player的方法都写在ctl中不要写在st中，  1

// 2-25
// 给npc添加timeline状态
// 把PConfig 和PlayerConfig融合

// 2-26
//把isMove  desPos改名写到NpcCtl中，并且把原来的删除    1


//2-27
//玩家timeline 第二个 移动指令不移动问题 查看修复

//3-1
// 修改了AstarBehviour 和 StPlayerStory内容
// 退出剧情模式的事件要添加呀      1添加了player AStarthBehavoir DiaLogBehavioe


//3--1的思考
//通过不同图片的组合，去制作2D动画 ，sprite Animation

//触发心里想法，不用选择。npc头上黄点
//每个星期初发钱、
//引导找人

//角色身上需要一个任务类，保存所有当前任务id。

//任务id对应任务表
//任务 记录一个任务id
//触发条件：。  任务目标地点。
//某个条件发生后会触发挫折  


//先修复timeline 
//做出npc与人物的亲密度关系
//地图功能
//挫折功能
//登录的scene


//5-18
//详细的URP讲解 https://zhuanlan.zhihu.com/p/84908168
//shader Grapha 介绍https://zhuanlan.zhihu.com/p/35887656
//优化https://segmentfault.com/a/1190000019844821

//silder中的值如果在lua中 赋予无限大 或者 某个除于0的数会使得组件读取不了值得卡死
//https://www.cnblogs.com/kuluodisi/p/14096172.html 为何lua用于热更新

//lua和c#相互调用的性能
//https://www.cnblogs.com/zwywilliam/p/5999924.html

//5-22给mutidialogView 添加问题Text
//初始化按钮绑定问题



//挫折系统和故事系统是不同的，挫折系统更像是一个随机性的东西，即时碰撞即时触发
//故事系统是线性的，一个接着一个。
//故事系统直接在playerconfig 中管理。挫折系统会在一个单独的管理类中管理，叫做FrustratioMgr


//5-27
//新计时器
//https://gameinstitute.qq.com/community/detail/127757

//项目frame 
//维护一个framePool表，每帧调用这个表里面的第一个元素



//工作记录
//6-1
//寻找像素字
//设置初始化游戏界面

//6-2 起名
//今日博客断言与异常：https://www.jb51.net/article/78708.htm
//unity中文件，文件引用 ， .meta 的内容 https://www.cnblogs.com/CodeGize/p/8697227.html
//YoungChange Game  （YC GAME）  
//重写timer系统   完成
//添加全局不移除脚本  DestoryHelper  完成
//添加民歌动画  完成
//TODO:添加动效 未完成
//全局mono单例，MonoSinglten类 完成
//游戏开启动画 完成


//6-3
//https://blog.csdn.net/wotingdaonile/article/details/105958762
//空间换时间：对象池，LOD,Minmap，LightMap
//时间换空间：压缩，AB，图集压缩？各种压缩
//todo:引导点击界面
//给mutidialogView添加Animation，并且完成滚动对话功能
//将mutidialogView中的Timer修改为新Timer ， 并且验证
//给npc添加动画，并且添加逻辑



//6-4
//使用timeline ， shaderGraph  ， A* Pathing ， Profiler

//https://www.zhihu.com/question/30317295树 

//6-5
//如何搭建游戏服务器https://cloud.tencent.com/developer/article/1062044

//一个工程https://github.com/ketoo/NoahGameFrame
//微信小程序广告介入http://www.sikiedu.com/course/834
//服务器搭建https://www.bilibili.com/s/video/BV1Jp4y1y7A1
//b站搜索， 从零搭建游戏服务器


//  2:00倒计时
//  [ 随机生成 ]先做一个的 ， 或者做两个的
//playnite
//youtube 搜索game server tutorial
//连接进房间就开始对战
//房间号的问题




//x-https://www.youtube.com/watch?v=5LhA4Tk_uvI

//每一个操作会有一个时间的消耗，比如点击和玩家对话会耗时十分钟。上课两小时，然后一天的结束时会对当天完成必须事情做一个统计

//询问，slg九宫会同步信息，那么服务器怎么知道我们当前在哪个九宫里呢？我们移动的时候会给后端发一个数据对吗

//BRDF

///////x-https://learnopengl-cn.github.io/07%20PBR/01%20Theory/ 非常好的教程