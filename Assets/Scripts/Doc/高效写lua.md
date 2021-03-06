---
title:  高质量lua代码
tag:  lua开发
---
# 前言
**对于Lua的一些编写总结**
<!-- more -->

使用Local**
    自从Lua5.0后，Lua采用一种类似于寄存器的虚拟机模式。Lua用栈来存储其寄存器。每一个活动的函数，Lua都会为其分配一个栈，这个栈用来存储函数里面的活动记录。每一个函数的栈都可以存储至多250个寄存器，因为栈的长度是用8个比特表示的。
    有了这么多寄存器，Lua的预编译器能把所有的local变量存储在其中。这就是的Lua在获取local变量时其效率十分的高的原因。

    举个例子： 假设 a 和 b 为local变量，a = a + b 的预编译会产生一条指令：

    ;a是寄存器0 b是寄存器1
    ADD 0 0 1

    但是若没有 a 和 b 都没有声明为 local 变量，则预编译会产生如下指令：

    GETGLOBAL  0 0    ;get a 
    GETGLOBAL  1 1    ;get b
    ADD        0 0 1  ;do add
    SETGLOBAL  0 0    ;set a

    PS: 全局变量（string, math等工具函数，自定义的全局变量）在引用过多的情况下，也考虑先在其作用域下用局部变量存储，这样在预编译期就只进行了一次GETGLOBAL操作

关于表（table）**

    表在Lua中使用十分频繁，因为表几乎代表了Lua的所有容器。所以了解Lua底层如何实现表，对我们编写Lua是有好处的。
    Lua的表分为两个部分：数组（array）部分和哈希（hash）部分。数组部分包含所有从1到n的整数键，其他所有的键都存储在哈希部分中。
    哈希部分其实就是一个哈希表，哈希表的本质是一个数组，它利用哈希算法将键转化为数组下标，若下标有冲突（即同一个下标对应了两个不同的键），则它会将冲突的下标上创建一个链表，将不同的键串在这个链表上，这种解决冲突的方法叫做：链地址法。
    当我们把一个新键值赋给表时，若数组和哈希表已经满了，则会触发一个 再哈希（rehash）。再哈希的代价是高昂的。首先会在内存中分配一个新的长度的数组，然后将所有记录再全部哈希一边，将原来的记录转移到新数组中。新哈希表的长度是最接近于所有元素数目的2次方。
    当创建一个空表时，数组和哈希部分的长度都将初始化为0，即不会为他们初始化任何数组。

 ``` lua
    local a = {}
    for i = 1, 3 do
        a[i] = true
    end    
``` 

    最开始，Lua创建了一个空表a，在第一次迭代中，a[1] = true 触发了一次 rehash， Lua将数组部分的长度设置了 2^ 0，即1，哈希部分仍然为空。在第二次迭代中，a[2] = true 再次触发了 rehash ，将数组部分长度设为 2^1，即2。最后一次迭代，又触发了一次 rehash，将数组部分长度设为 2^2 即 4 。
    下面这段代码：

 ``` lua
     local a = {}
     a.x = 1
     a.y = 2
     a.z = 3
``` 

    与上一段代码类似，只是其触发了三次表中哈希部分的 rehash 而已。
    只有三个元素的表，会执行3次 rehash；然而有一百万个元素的表仅仅会执行20次 rehash 而已（2^20 = 1048576 > 1000000）。但是，如果你创建了非常多的长度很小的表（比如坐标点：point = {x = 0， y = 0}），这可能会造成巨大的影响。
    如果你有很多非常多很小的表需要创建时，你可以将其进行预先填充以避免 rehash 。比如{true， true， true}，Lua知道这个表中有三个元素，所以Lua直接创建了三个元素长度的数组。类似的，{x = 1， y = 2, z = 3}，Lua会在其哈希部分中创建长度为4的数组。
以下代码执行事件为1.53秒：

 ``` lua
    a = os.clock()
    for i = 1, 200000 do
        local a = {}
        a[1] = 1
        a[2] = 2
        a[3] = 3
    end
    b = os.clock()
    print(b - a) -- 1.528293
``` 

    如果我们在创建表的时候就填充好它的大小，则只需要0.75秒，一倍效率提升。

 ``` lua
    a = os.clock()
    for i = 1, 200000 do
        local a = {1, 1, 1}
        a[1] = 1
        a[2] = 2
        a[3] = 3
    end
    b = os.clock()
    print(b - a) -- 0.746453
``` 

    所以，当需要创建非常多的小size的表时，应预先填充好表的大小。

    PS：在Unity中 C#与Lua的通信，例：用Vector3.zero, Vector3.one 代替 Vector3(0, 0, 0), Vector3(1, 1, 1)
    Vector3，Vector2等数据类型在Slua中会被转译成table，运用Unity中预定义好的Vector3等向量，同样可以避免Rehash的问题
关于字符串**

    Lua在实现字符串类型与其他主流脚本有所不同。
    第一，所有的字符串在Lua中都只储存一份拷贝。当新字符串出现时，Lua检查会否有其相同的拷贝，若没有则创建它，否则，指向这个拷贝。这可以使得字符串比较和表索引变得相当的快，因为比较字符串只需要检查引用是否一致即可；但是这也降低了创建字符串时的效率，因为Lua需要去查找比较一遍。
    第二，所有的字符串变量，只保存字符串引用，而不保存它的buffer(缓存)。这使得字符串的赋值变得十分高效。例如在Perl中，$x = $y，会将 $y 的 buffer 整个的复制到 $x 的 buffer 中，当字符串很长时，这个操作的代价将十分昂贵。而在Lua，同样的赋值，只复制引用，十分的高效。
    但是只保存引用会降低字符串连接时的速度。在Perl中，$s = $s . 'x' 和 $s .= ' x ' 的效率差距惊人，前者将会获取整个 $s 的拷贝，并将 ' x '  添加到它的末尾；而后者，将直接将 ' x ' 插入到 $x 的buffer末尾。
    由于后者不需要进行拷贝，所以其效率和 $s 的长度无关，因为十分高效。
    在Lua中，并不支持第二种更快的操作。以下代码将花费 6.65 秒：

 ``` lua
    a = os.clock()
    local s = ""
    for i = 1, 300000 do
        s = s .. "a"
    end
    b = os.clock()
    print(b - a) -- 6.649481

    我们可以用 table 来模拟 buffer ，下面的代码只需花费 0.72秒， 9倍多的效率提升：

    a = os.clock()
    local t = {}
    for i = 1, 300000 do
        t[#t + 1] = "a"
    end
    local s = table.concat(t, "")   
    b = os.clock
    print(b - a) -- 0.07178
``` 

    所以：在大字符串连接中，我们应避免  .. 来进行字符串连接。应用 table 来模拟 buffer ，然后 concat 得到最终字符串。
3R原则

    3R原则是：减量化（reducing），再利用（reusing）和再循环（recycling）三种原则的简称。
    3R原则本是循环经济和环保的原则，但是其同样适用于Lua。
    Reducing
        有许多办法能够避免创建新对象和节约内存。例如：如果你的程序中使用了太多的表，你可以考虑换一种数据结构表示。
        举个例子，假设你的程序中有多边形这个类型，你用一个表来储存多边形的顶点：

 ``` lua
        polyline = {
            {x = 1, y = 2,},
            {x = 1, y = 2,},
            {x = 1, y = 2,},
             ...
        }        
``` 

        以上数据结构十分自然，便于理解。但是每一个顶点都需要一个哈希部分来储存。如果放置在数组部分中，则会减少内存占用：

 ``` lua
        polyline = {
            {1, 2,},
            {1, 2,},
            {1, 2,},
             ...
        }     
```   

        一百万个顶点时，内存将会由 153.3 MB减少到 107.6 MB，但是代价是代码的可读性降低了。
        然而最节省内存的方法是：

 ``` lua
        polyline = {
            x = {1, 2, 3, 4, ...},
            y = {1, 2, 3, 4, ...},
        }    
```   

        一百万个顶点，内存将只占用 32 MB， 相当于原来的1/5。你需要在性能和代码可读性之间做出取舍。
        在循环中，我们更需要注意实例的创建。

 ``` lua
        for i = 1, n do
            local t = {1, 2, 3}
            -- 执行逻辑, 但不修改t 
            ...
        end 

        我们应该把在循环中不变的东西放到循环外来创建：

        local t = {1, 2, 3}
        for i = 1, n do
            -- 执行逻辑, 但不修改t
            ...
        end 
```   

    Reusing
        如果无法避免创建新对象，我们需要考虑重用旧对象。
        考虑下面这段代码：

 ``` lua
        local t = {}
        for i = 1, 12 do
           t[i] = os.time({year = 2020, month = i, day = 1})
        end

        在每次循环迭代中，都会创建一个新表{year = i， month = 6， day = 14}，但只有 year 是变量。
        下面代码重用了表：

        local t = {}
        local tmp = {year = 2020, month = nil, day = 1}
        for i = 1, 12 do
            tmp.month = i
            t[i] = os.time(tmp)
        end
```   

        另一种方式的重用，则是在于缓存之前计算的内容，以避免后续的重复计算。后续遇到相同的情况时，则可以直接查表取出。这种方式实际就是动态规划效率高的原因所在，其本质是用空间换时间。

    Recycling
        Lua自带垃圾回收器，所以我们一般不需要考虑垃圾回收的问题。
        了解Lua的垃圾回收能够使得我们编程的自由度更大。
        Lua的垃圾回收器是一个增量运行的机制。即回收分成许多小步骤（增量的）来进行。
        频繁的垃圾回收可能会降低程序的运行效率。
        我们可以通过Lua的 collectgarbage 函数来控制垃圾回收器。
        对于批处理的Lua程序来说，停止垃圾回收 collectgarbage("stop")会提高效率，因为批处理程序在结束时，内存将全部被释放。
        对于垃圾回收器的步幅来说，实际上很难一概而论。更快幅度的垃圾会收回消耗更多CPU，但会释放更多内存，从而也降低CPU的分页时间。
堆栈溢出 当我们运行lua的时候，可能出现stack overflow的报错，以下代码为例

 ``` lua
function func(a)

    a = a + 1
    if a > 100000 then
        print(a)
    else
        func(a)
    end

    return
end

x =  func(x)
```   

语法上确实没有任何问题，但执行的时候就会出现stack overflow的报错，是什么原因导致堆栈溢出呢？这里需要追究到Lua源码：Lua虚拟机会对堆栈进行一系列检查（函数：luaL_checkstack）,错误类型就有： "too many argument"， "assume array is smaller than 2^40" "string slice too long" "too many captures" "too many argument to script" "too many nested functions" 上述代码就属于递归嵌套次数太多，默认限制20000。

lua与c#性能交互
https://blog.csdn.net/qq_14914623/article/details/80923198?utm_medium=distribute.pc_relevant.none-task-blog-baidujs_baidulandingword-0&spm=1001.2101.3001.4242

lua博客对table源码的剖析
https://yuerer.com/Lua5.3-%E8%AE%BE%E8%AE%A1%E5%AE%9E%E7%8E%B0(%E4%BA%8C)-Table%E4%B8%8EMetatable/


作者：Simon YXY
