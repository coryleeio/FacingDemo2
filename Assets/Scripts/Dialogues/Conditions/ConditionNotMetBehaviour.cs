namespace Gamepackage
{
    // When I don't meet the requirements of an option is it visible or not?
    public enum ConditionNotMetBehaviour
    {
        NotSet,
        Disable, // Requirements not met is shown.
        Hide,    // Option does not appear in list.
    }
}
