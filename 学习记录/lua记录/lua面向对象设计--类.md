【Lua面向对象设计—-类】
Lua不存在类的概念，每个对象定义他自己的行为并拥有自己的形状。类其实是具有相同特征的对象的抽象。每个被实例化的对象都有一些相同的属性和方法。
比如，我们想让a拥有B所有的属性和方法，那怎么实现呢？
我们首先想到使用之前讲过的元表，比如:
setmetatable(a, {__index = B})
这样，对象a调用任何不存在的成员都会到对象B中查找。事实上，这里的B就是相当于类，a可以看做是B的对象。
那么我们拓展一下
Man = {name = "unknow"}
function Man:new (o)
    o = o or {}            -- 如果o不存在则创建一个实例
    setmetatable(o, self)  --这里的self是Man
    self.__index = self    --把index指向Man
    return o               --返回这个实例
end
function Man:sayHi()
    print("Hi, I am "..self.name)
end
当我们调用Man:new时，self等于Man；因此我们可以直接使用Man取代self。关于继承下节会深入。有了这段代码之后，当我们创建一个Man的实例并且调用一个方法的时候，有什么发生呢？
a = Man:new{name = "Lee"}
print(a.name) --结果为Lee
我们来看到底发生了什么。当我们调用Man:new{name = "Lee"}时，其实相当于写 Man.new(Man, {name = "Lee"})。而现在，o = {name = "Lee"}， self = Man。也就是说o不为nil，这样，第三条语句就不起作用。之后我们绑定元表为Man, 并且让Man.__index指向自己，这样，当a找不到的东西就会往Man找。
比如说下面：
a:sayHi() --结果为"Hi, I am Lee"
也就是说，a并没有sayHi这个方法。但是由于绑定了元表，所以a找不到sayHi会首先找Man.__index，之后会再找_index所指向的表，也就是Man,而Man存在sayHi方法，它会调用它。
 
我们再看一个例子
a = Man:new()    --这次我们没有传任何的值
print(a.name)    --结果为"unknow"
也就是说其实它继承了默认值。
但是，我们接着看：
a = Man:new()
a.name = "Lee"

b = Man:new()
print(b.name)     --结果为unknow,并不是"Lee"
也就是说所有new出来的对象，都拥有了自己的单独的属性，也就实现了我们面向对象中 类和对象的概念。