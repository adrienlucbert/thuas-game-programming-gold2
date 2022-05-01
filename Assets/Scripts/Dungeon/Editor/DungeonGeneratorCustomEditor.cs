using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ADungeonGenerator), true)]
public class ADungeonGeneratorCustomEditor : Editor
{
    private ADungeonGenerator _generator;

    private void Awake()
    {
        this._generator = (ADungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Dungeon"))
        {
            this._generator.Generate();
        }
    }
}
