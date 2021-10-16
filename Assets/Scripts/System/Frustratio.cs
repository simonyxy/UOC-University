using System;
//挫折类
public class Frustratio
{
    //0未开启，1开启，2完成
    private int _isOpen;
    public int isOpen{
        set{
            _isOpen = value;
        }
        get{
            return _isOpen;
        }
    }
    public readonly int? id ;
    public readonly string name;
    public Action OpenFunc;
    //查看是否满足开启条件
    public Func<bool> checkIsOpen;
    //查看是否满足关闭条件
    public Func<bool> checkIsClose;

    public Frustratio(int id, int isOpen,string name,Action OpenFunc,Func<bool> checkIsOpen , Func<bool> isClose)
    {
        this.id = id ;
        this.isOpen = isOpen;
        this.OpenFunc = OpenFunc;
        this.checkIsOpen = checkIsOpen;
        this.checkIsClose = isClose;
    }

    public int ToId()
    {
        return id ?? -1;
    }

    public void SetState(int isOpen)
    {
        this.isOpen = isOpen;
    }
}