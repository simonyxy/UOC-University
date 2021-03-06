using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

///帮助自己回头理解状态机
///默认的简易状态机

///
//状态类，直接做成接口，每一个状态都要初始化都要去实现IState才能完整的成为一个状态
//一个状态就是一个IState
public interface IState
{
    void Tick();
    void OnEnter();
    void OnExit();
    void TiggerEvent(string sevent);
}
//真正的FSM，用来保存和控制状态的转化
public class StateMachine 
{
    
    //首先必须去理解Transition类，这是一个保存某一状态下所有可转化的其他状态以及转化为其他状态的相应要满足的条件的保存状态的类
    private class Transition
    {
        //声明一个可转移状态
        public IState To { get; }
        //声明转化条件
        public Func<bool> Condition { get; }

        //构造函数
        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    //当前的状态机的状态
    private IState _currentState;
    public IState CurrentState{
        get{return _currentState;}
    }
    //当前的状态的所有转化条件
    private List<Transition> _currentTransitions = new List<Transition>();
    //可任意切换为的状态条件
    private List<Transition> _anyTransitions = new List<Transition>();
    //当一个状态下没有任何转化条件的时候默认的空的转化条件存进去
    private static List<Transition> EmptyTransitions = new List<Transition>(0);

    //_transitions这个是整个状态机所有的状态和所有状态下的转化条件的保存
    //状态机里面的所有保存的状态和转化到该状态的条件
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();

    //状态机在每一帧都会调用的执行方法
    public void Tick()
    {
        //先遍历当前状态下是否有可转化为另一个状态的条件
        var transition = GetTransition();
        if (transition != null)
        {
            //如果有满足的可转化条件，转化为该状态
            SetState(transition.To);
        }

        //执行新的状态的Tick
        _currentState?.Tick();
    }

    //设置状态机当前状态
    public void SetState(IState state)
    {
        //防止重复设置
        if (state == _currentState)
            return;
        //调用上一个状态的离开函数，让他走的很体面
        // _currentState?.Tick();   --12/21
        _currentState?.OnExit();
        //设置状态机当前状态
        _currentState = state;
        //获得当前的状态的所有转化条件，设置为当前转化条件
        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions);
        //如果当前状态的所有转化条件为空，那就设置为空
        if (_currentTransitions == null)
            _currentTransitions = EmptyTransitions;
        //进入状态执行方法，让他体面的离开
        _currentState.OnEnter();
    }

    //添加某一状态的转化方法
    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        //先获得这个状态的所有可转化条件
        //如果这个状态没有转化条件，那就给他生成一个
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            //生成一个转化条件类
            transitions = new List<Transition>();
            //把这个转化条件类保存到整个状态机的转化方法当中
            _transitions[from.GetType()] = transitions;
        }
        //如果有转化状态类，那就把这个方法加入到该状态类的转化条件中
        transitions.Add(new Transition(to, predicate));
    }
    //跟上述方法同理
    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransitions.Add(new Transition(state, predicate));
    }

    //遍历是否存在有转化条件可执行的，这个方法用在Tick中，用于每次Tick检查是否存在状态转化
    private Transition GetTransition()
    {
        foreach(var transition in _anyTransitions)
        {
            if (transition.Condition())
                return transition;
        }

        foreach (var transition in _currentTransitions)
            if (transition.Condition())
                return transition;

        return null;
    }
}

// Start is called before the first frame update


