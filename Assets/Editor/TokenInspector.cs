using UnityEditor;
using Gamepackage;

[CustomEditor(typeof(UnityTokenReference))]
public class TokenInspector : Editor
{
    SerializedProperty _shapeProp;

    void OnEnable()
    {
        // Fetch the objects from the GameObject script to display in the inspector
        _shapeProp = serializedObject.FindProperty("Owner.Shape");
    }


    public override void OnInspectorGUI()
    {
        UnityTokenReference myTarget = (UnityTokenReference)target;
        var token = myTarget.Owner;
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Position", token.Shape.Position.ToString());
        EditorGUILayout.LabelField("Width", token.Shape.Width.ToString());
        EditorGUILayout.LabelField("Height", token.Shape.Height.ToString());
        EditorGUILayout.LabelField("Behaviour", token.Behaviour.GetType().ToString());
        EditorGUILayout.LabelField("Motor", token.Motor.GetType().ToString());
        EditorGUILayout.LabelField("Inventory", token.Inventory.GetType().ToString());
        EditorGUILayout.LabelField("Equipment", token.Equipment.GetType().ToString());
        EditorGUILayout.LabelField("Behaviour", token.Behaviour.GetType().ToString());
        EditorGUILayout.LabelField("View", token.TokenView.GetType().ToString());
        EditorGUILayout.LabelField("Persona", token.Persona.GetType().ToString());
        EditorGUILayout.LabelField("TriggerBehaviour", token.TriggerBehaviour.GetType().ToString());
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
    }
}