namespace Gamepackage
{
    public class TokenPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public ShapeType ShapeType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string BehaviourClassName { get; set; }
        public string EquipmentClassName { get; set; }
        public string InventoryClassName { get; set; }
        public string MotorClassName { get; set; }
        public string PersonaClassName { get; set; }
        public string TriggerBehaviourClassName { get; set; }
        public string ViewClassName { get; set; }
    }
}
