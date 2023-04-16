namespace CalcosScuffedCoroutines;

public class WaitForSeconds : IYieldable
{
    public bool IsDone => TimeLeft <= 0f;

    public float TimeLeft { get; private set; }

    public WaitForSeconds(float time)
    {
        TimeLeft = time;
    }

    public void Update(float delta)
    {
        TimeLeft -= delta;
    }
}