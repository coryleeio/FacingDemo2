using Gamepackage;
using Newtonsoft.Json;

public class Wait : Action
{
    [JsonIgnore]
    public Entity Source;

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
