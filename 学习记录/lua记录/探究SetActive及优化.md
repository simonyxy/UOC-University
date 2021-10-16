---
title: 探究SetActive及优化
tag:  UI
---
# 关于SetActive 
优化目的
SetActive 应该是在用unity开发的当中使用最多的一个方法之一 在ui中使用可能会更多 为了应对各种各样的需求 我们可能会在代码中使用大量的 SetActive(true) SetActive(false)
<!-- more -->
# 是否有更高效的代替方法？
三种隐藏UI的方法：
1.SetActive（）
2.Transform移动
3.canvasGroup 

 
# 1.SetActive分析
 ![avatar](/assets/img/UI_3.jpg)
\\用luaProflier 可以看到.SetActive消耗 产生（4KB）CG  lua函数调用时间0.14ms lua函数消耗0.000139s

接下来用Transform移动
![avatar](/assets/img/UI_4.jpg)
\\用luaProflier 可以看到.Transform移动消耗 产生0 CG  lua函数调用时间0.07ms lua函数消耗0.000071s


再接下来用canvasGroup 
![avatar](/assets/img/UI_5.jpg)
\\用Proflier 可以看到canvasGroup消耗 产生0 CG  函数调用时间0.05ms 

根据上面图显示消耗时间canvasGroup< Transform移动< SetActive（）

建议隐藏红点或button等小组件可以用SetActive（）
小界面panel可以用canvasGrou.alpha = 0隐藏
canvasGrou的Alpha由0设为1的时候，并不会让自己活着子节点中的脚本执行。


为了正确了解SetActive为什么会产生GC用C#模拟一下一个界面要SetActive 20次情况
![avatar](/assets/img/UI_6.png)
![avatar](/assets/img/UI_7.png)
![avatar](/assets/img/UI_8.png)
 
还会蛮整齐GC的 这就是 为什么网上大佬们都不建议用setactive这个方法 在每次使用的时候会遍历这个实体上所有继承MonoBehaviour的脚本调用OnEnable 因为Image继承的MaskableGraphic 下图可以发现 写了方法算上基类的方法产生GC。
![avatar](/assets/img/UI_9.png)
 
重点来了我手贱又在这个image下挂了一个image 如下
![avatar](/assets/img/UI_10.png)
最终结果：
![avatar](/assets/img/UI_11.png)
父物体在使用SetActive时 也会调用子物体的GameObject.Activate和GameObject.Deactivate所以在做ui时 一个panel下挂载很多的image 调用这个panel的SetActive 其实基本等同于调用这个物体上每一个子物体的SetActive 并在每次使用的时候会遍历这个实体上所有继承MonoBehaviour的脚本带来大量的GC。




