---
title:  Unity的内存管理
tag:  Unity学习
---
多看看
<!-- more -->
# Unity内存管理和性能优化：
https://blog.csdn.net/jingangxin666/article/details/82564238

# C#的垃圾回收机制（分代）
https://www.cnblogs.com/qixuejia/p/3957966.html
-- 稍微总结一下:
对GC实际的理解上，我更喜欢把CLR比作是房东，将托管堆比作是一间大公寓，每次有对象（根）在CLR登记后，CLR就会给它提供一个身份证明（引用地址），记录到房客租赁登记表上（线程栈）。因为这件大公寓空间仍然是有限的，房客的重要性也不一样，所以大公寓将不同的房间划分为天字号，地字号，人字号三种房间（选择预算），房东比较重感情，所以刚来的房客嘛，管你有钱没钱，先给我去人字号带着。每次人字号房间不够住的时候，房东就会安排清理工（GC）来安排房间归属了。对人字号房间的房客，清理工会一个个检查过去，看看有没有房客和房东关系疏远了（不可达），这些没心没肺的（也可能是房东主动提出绝交）全都滚出去，那些剩下来的再安排到地房间去。假如地字号房间没满，清理工就不检查了（代的性能优化），满了再依旧安排。假如你是地字号的，就算你和房东绝交了，也会考虑再让你住些日子。那如果有时候发现一些房客就是暂住下，人数又多，离开又早，那清理工就会调整下房间，把各层级的房间数目再分配下
-- C# 是分代收集算法(Generational Collection)
对内存对象进行分代标记，避免全量垃圾回收带来的性能消耗。

# Lua的垃圾回收机制 以及对比
https://www.cnblogs.com/zblade/p/11357203.html
--稍微总结一下：
Lua 5.0 和前1. 双色标记清除算法
在Lua5.0中的GC，是一次性不可被打断的操作，执行的算法是Mark-and-sweep算法，在执行GC操作的时候，会设置2种颜色，黑色和白色，然后执行gc的流程
5.0后2. 三色标记清除算法
虽然是三色，本质是四色，颜色分为三种:

# Assetbudle 介绍：
https://blog.csdn.net/qq_33537945/article/details/79326857
https://www.cnblogs.com/zblade/p/9198647.html
# Assetbundle 机制和内存管理:
https://blog.csdn.net/u012529088/article/details/54984479?utm_source=itdadao&utm_medium=referral
https://blog.csdn.net/andyhebear/article/details/50977295

# 资源内存优化
https://blog.csdn.net/a133900029/article/details/80388072
https://blog.csdn.net/wotingdaonile/article/details/80111088

# 至于 代码内存的优化 
比如用枚举代替类 
少调用GetComponent等和unity交互的函数
String 和StringBuilder 和StringBuffer
对象池 ....等待自己慢慢积累把~

作者：SimonYXY
