using System;

public class Timer
{
    public float Duration;
    public float LeftTime;
    private Action _updateAction;
    private Action _callAction;
    private bool _isPause;
    private string _name ;
    private bool _isLoop;
    public string name
    {
        get{
            return _name;
        }
        private set{}
    }

    public bool isPause
    {
        get{
            return _isPause;
        }
        private set{}
    }

    public Timer(string name ,float duartion,Action callAction = null,bool isLoop = false)
    {
        _name = name;
        LeftTime = duartion;
        Duration = duartion;
        _updateAction = null;
        _callAction = callAction;
        _isPause = false;
        _isLoop = isLoop; 
    }

    public Timer(string name ,float duartion,Action updateAction = null, Action callAction = null,Action initAction = null,bool isLoop = false)
    {
        _name = name;
        LeftTime = duartion;
        Duration = duartion;
        if(initAction != null) initAction.Invoke();
        _updateAction = updateAction;
        _callAction = callAction;
        _isPause = false;
        _isLoop = isLoop; 
    }

    public void OnUpdate(float deltaTime)
    {
        if(isPause)
        return ;

        LeftTime -= deltaTime;
        if(LeftTime <= 0)
        {
            if(_callAction != null )
            {
                LeftTime = Duration;
                _callAction.Invoke();
            }
            if(!_isLoop)
            {
                isPause = true;
            }
        }
        else
        {
            if(_updateAction != null )
                _updateAction.Invoke();
        }
    }

    public void SetTimerTrcik(bool b)
    {
        isPause = b;
    }

}