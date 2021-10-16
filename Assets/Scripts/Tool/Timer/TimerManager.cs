using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoSingleton<TimerManager>
{
    private List<Timer> _timers;
    private Dictionary<string,Timer> _timerDict;

    protected override void Awake()
    {
        base.Awake();
        _timers = new List<Timer>();
        _timerDict = new Dictionary<string,Timer>();
    }

    private void Update()
    {
        for(int i= _timers.Count -1 ; i >= 0 ; i --)
        {
            if(_timers[i].isPause)
            {
                RemoveTimer(_timers[i].name);
                if( _timers.Count == 0 )
                    break;
            }
            _timers[i].OnUpdate(Time.deltaTime);
        }
    }

    public void AddTimer( Timer timer )
    {
        if(_timerDict.ContainsKey(timer.name))
        {
            _timerDict[timer.name].LeftTime += _timerDict[timer.name].Duration;
            _timerDict[timer.name].SetTimerTrcik (false);
        }
        else
        {
            _timerDict.Add(timer.name,timer);
            _timers.Add(timer);
        }
    }

    public void RemoveTimer(string str)
    {
        var timer = _timerDict[str];
        if(timer != null)
        {
            _timers.Remove(timer);
            _timerDict.Remove(str);
        }
    }
}