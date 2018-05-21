using Gamepackage;

public class Wait : TokenAction
{
    private TurnSystem _turnSystem;

    public Wait(TurnSystem turnSystem)
    {
        _turnSystem = turnSystem;
    }

    public override int TimeCost
    {
        get
        {
            return 250;
        }
    }

    public override bool IsComplete
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
