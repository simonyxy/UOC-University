【Lua面向对象设计——继承】
继续我们上一节，仍然假设我们有一个Man类：
Man = {name = ""}
function Man:new(o)
    o = o or {}            --如果不存在则实例化一个空表
    setmetatable(o, self)  
    self.__index = self
    return o
end

function Man:sayHi()
    print("Hi,I'm "..self.name)
end
我们实例化它：
Lee = Man:new{name = "Lee"}
Lee:sayHi()   -结果是 Hi,I'm Lee
现在我们让一个SuperMan继承Man，并且再用SuperMan实例化多一个Lee,并且添加一个方法测试：
SuperMan = Man:new()
--添加多一个fly方法
function SuperMan:fly()
    print("I'm "..self.name..". I'm flying!")
end
--用SuperMan实例化 Lee
Lee = SuperMan:new{name = "Lee"}
Lee:sayHi()    --结果：Hi, I'm Lee
Lee:fly        --结果：I'm Lee. I'm flying!
结果显示，superman继承了Man的方法。并可以添加自己的方法。
下面我们让SuperMan重写Man的sayHi方法：
function SuperMan:sayHi()
    print("I am SuperMan, "..self.name)  
end

Lee = SuperMan:new{name = "Lee"}
Lee:sayHi()            --结果是：I am SuperMan, Lee

这说明Lua也能像c++/java那样，子类重写父类的方法