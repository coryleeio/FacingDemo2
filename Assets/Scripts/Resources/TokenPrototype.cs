namespace Gamepackage
{
    public class TokenPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public ShapeType ShapeType { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public BehaviourPrototype BehaviourPrototype { get; set; }
        public EquipmentPrototype EquipmentPrototype { get; set; }
        public InventoryPrototype InventoryPrototype { get; set; }
        public MotorPrototype MotorPrototype { get; set; }
        public PersonaPrototype PersonaPrototype { get; set; }
        public TriggerBehaviourPrototype TriggerBehaviourPrototype { get; set; }
        public TokenViewPrototype TokenViewPrototype { get; set; }
    }
}
