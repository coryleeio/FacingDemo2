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

    public override bool IsComplete
    {
        get
        {
            return true;
        }
    }
}
