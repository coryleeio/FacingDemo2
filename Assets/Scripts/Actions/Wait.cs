using Gamepackage;
using Newtonsoft.Json;

public class Wait : Action
{
    [JsonIgnore]
    public override Entity Source
    {
        get; set;
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
