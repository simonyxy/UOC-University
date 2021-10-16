-- 1.把所有继承的类存到BaseClass的一个表里面，这样比较好删除
--o就是表名字，存在classinfo
--基础类
BaseClass ={ 
	--为了确保Init只被调用一次，写个标识符_Init
    _Init = false,
 }
function  BaseClass:Creat( o )
	o = o or {}
	--实现继承
	setmetatable(o , self)
	self.__newindex = 
	function ( t,k,v )
    	self [k] = v   --新添加数据的时候，这里是直接添加到父类中。也就是说Monster类的信息在BaseClass里面，BOSS类里面的信息放在Monster类里面，方便删除处理
    end

	self.__index = self  --找数据的时候，在对应name下面的info表里面

	function o:New( type , ...  )
		if  o:Init() then 
		    o:Init()
		else
			self:Init()
		end
	end
	o:New(o)

  	o.Delete = function (  )
  		--删除的时候把自己和父类的index都删除了，因为newindex的时候是把元素生成在父类上面的，
  		--父类__index删掉后，那就等于删掉了
  		self.__index =nil
  	end
	return o 
end

----写一个new方法调用作用仅是init
-- function BaseClass:New(  )
-- 	if  self.classtype:Init()
-- end
function BaseClass:Init( )
	if self._Init ==true  then  
		return 
	else 
		print("调用了基类的Init")
		self._Init = true 
	end
end
--BaseClass的Delete要自动清理类里面的成员，但他是基类，怎么访问到子类的成员呢。
--思路：把子类的成员写到基类BaseClass里面？
function BaseClass:Delete()

end
-------Text-------------------
-- s = BaseClass:New()
-- s.name = "hello"
-- s:Init()
-- function s:Init(  )
-- 	print("这是子类s的Init")
-- end
-- s:Init()
-- q = BaseClass:New(s)
-- print(q.name)
-- s:Delete()
-- print(s.name)

--3
--怪物类
Monster =BaseClass:Creat() 
function Monster:Init( )
	Monster:Idle()
	Monster:Attack()
	Monster:Dead()
end

function Monster:Idle(  )
	print("Monster类的Idel") 	
end

function Monster:Attack(  )
	print("Monster类的Attack") 	
end 

function Monster:Dead(  )
	print("Monster类的Dead") 	
end  
--BOSS类继承Monster类
-- BOSS = BaseClass:New(Monster)
-- function BOSS:Init()
	
-- end
-- function CreatMonster(  )
-- 	return BaseClass:New(Monster)
-- end
	


