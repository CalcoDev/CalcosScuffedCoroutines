namespace CalcosScuffedCoroutines;

public class WaitForCondition : IYieldable
{
    public bool IsDone => Condition?.Invoke() ?? true;

    public Func<bool> Condition { get; }

    public WaitForCondition(Func<bool> condition)
    {
        Condition = condition;
    }

    public void Update(float delta)
    {
    }
}