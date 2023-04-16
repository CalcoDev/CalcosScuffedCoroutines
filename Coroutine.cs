using System;
using System.Collections;
using System.Collections.Generic;

namespace CalcosScuffedCoroutines;

public class Coroutine
{
    public bool Finished { get; private set; }

    private Optional<IYieldable> _yieldable;
    
    public Action OnFinished;
    public Action<IYieldable> OnYielded;

    private readonly Stack<IEnumerator> _enumerators;

    public Coroutine(IEnumerator? function, bool removeOnComplete = true)
    {
        _enumerators = new Stack<IEnumerator>();

        if (function != null)
            _enumerators.Push(function);

        Finished = false;
    }

    public void Update(float delta)
    {
        if (_yieldable is { HasValue: true, Value.IsDone: false })
        {
            _yieldable.Value.Update(delta);
            return;
        }

        if (_enumerators.Count == 0)
        {
            Finish();
            return;
        }
        Finished = false;

        IEnumerator now = _enumerators.Peek();
        if (now.MoveNext())
        {
            if (now.Current is not IYieldable yieldable)
                return;

            _yieldable.Value = yieldable;
            OnYielded?.Invoke(_yieldable.Value);
        }
        else
        {
            _enumerators.Pop();
            if (_enumerators.Count == 0)
                Finish();
        }
    }

    public void Finish()
    {
        Finished = true;

        OnFinished?.Invoke();
        if (_yieldable.HasValue)
            OnYielded?.Invoke(_yieldable.Value);

        _yieldable.Clear();
    }

    public void Cancel()
    {
        Finished = true;
        _yieldable.Clear();
        _enumerators.Clear();
    }

    public void Replace(IEnumerator function)
    {
        Finished = false;

        _yieldable.Clear();
        _enumerators.Clear();
        _enumerators.Push(function);
    }
}