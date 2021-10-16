【Lua面向对象设计——面向对象基础】
在了解了元表之后，我们可以了解一些Lua面向对象基础。
我们都知道c++、java等语言是面向对象语言，有类和对象的概念，而Lua一样可以通过表实现面向对象。
比如我们可以利用表实现以下代码：
Man = {}
Man.new = function(name)
    local man = {}              --创建实例
    man.name = name             --属性
    man.sayHi = function()      --方法
        print("Hi,I'm "..man.name)
    end
    return man
end

Lee = Man.new("Lee")
Lee.sayHi()                     --结果：Hi,I'm Lee
上述代码看起来简单易懂，它实现了类似于c++、Java new的功能。但是事实上这样写还不完整。
我们每次调用sayHi()函数的时候都必须写Lee，当我们自己给Lee添加一个方法时，也必须在方法和属性名前写Lee,这会非常不方便，尤其是后面讲到的继承
于是我们想到像C++/java那样拥有this,而我们Lua常用的是self。
比如一个例子：
Account = {balance = 0}
function Account.withdraw (v)
Account.balance = Account.balance - v
end
改写后就是：
function Account.withdraw (self, v)
    self.balance = self.balance - v
end
其实就是在接口里添加多一个self。
我们可以这样调用：
a1 = Account; Account = nil
...
a1.withdraw(a1, 100.00)
当然，有的同学就会提出，这个a1看着别扭，每次调用我都要传一个自己进去。能不能像C++、java那样隐藏起来？行的，这没问题，Lua也实现了类似的机制。Lua提供了通过使用冒号操作符来隐藏这个参数的声明。我们可以重写上面的代码：
function Account:withdraw (v)              --等效于function Account.withdraw (self,v)
    self.balance = self.balance - v
end
 
这样，调用的时候就变成：
a:withdraw(100.00)   --等效于a.（a,100.00）
这样，我们写代码就方便多了