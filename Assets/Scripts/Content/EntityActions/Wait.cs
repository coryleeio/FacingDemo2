using Gamepackage;

public class Wait : EntityAction
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

    public override bool IsAMovementAction
    {
        get
        {
            return false;
        }
    }

    public override bool IsStartable
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
