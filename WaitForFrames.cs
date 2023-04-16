namespace CalcosScuffedCoroutines;

public class WaitForFrames : IYieldable
{
    public bool IsDone => FrameCount == 0;

    public uint FrameCount { get; private set; }

    public WaitForFrames(uint frameCount)
    {
        FrameCount = frameCount;
    }

    public void Update(float delta)
    {
        FrameCount -= 1;
    }
}