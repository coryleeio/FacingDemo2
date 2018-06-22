using Gamepackage;

public class Wait : Action
{
    public override int TimeCost
    {
        get
        {
            return 250;
        }
    }

    public override bool IsEndable
    {
        get
        {
            return true;
        }
    }

    public override void Enter()
    {
        base.Enter();
    }
}
