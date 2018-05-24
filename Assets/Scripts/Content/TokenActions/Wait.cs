using Gamepackage;

public class Wait : TokenAction
{
    public override int TimeCost
    {
        get
        {
            return 250;
        }
    }

    public override bool ShouldEnd
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
