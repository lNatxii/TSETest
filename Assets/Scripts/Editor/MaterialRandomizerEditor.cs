using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MaterialRandomizer))]
public class MaterialRandomizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MaterialRandomizer randomizer = (MaterialRandomizer)target;

        EditorGUILayout.LabelField("Material Randomizer", EditorStyles.boldLabel);

        // Guardar el estado de las checkboxes en variables locales
        bool newRandomizeColor = EditorGUILayout.Toggle("Randomize Color", randomizer.randomizeColor);
        bool newRandomizeMetallic = EditorGUILayout.Toggle("Randomize Metallic", randomizer.randomizeMetallic);
        bool newRandomizeSmoothness = EditorGUILayout.Toggle("Randomize Smoothness", randomizer.randomizeSmoothness);

        // Asignar los nuevos valores
        randomizer.randomizeColor = newRandomizeColor;
        randomizer.randomizeMetallic = newRandomizeMetallic;
        randomizer.randomizeSmoothness = newRandomizeSmoothness;

        if (GUI.changed) // Verifica si algo ha cambiado en la UI
            EditorUtility.SetDirty(randomizer); // Se marca como dirty
    }
}