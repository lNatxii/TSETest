using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeSpawner))]
public class CubeSpawnerEditor : Editor
{
    private SerializedProperty spawnAreaSize;
    private SerializedProperty cubePrefab;
    private SerializedProperty gizmoColor;
    private SerializedProperty spawnedObjectsParent;
    private SerializedProperty cubeSpawnMultiplyer;
    private SerializedProperty inGameMultiplierSlider;
    private SerializedProperty inGameCubeCounter;

    private void OnEnable()
    {
        if (serializedObject == null) return; // Evita errores si serializedObject es nulo

        spawnAreaSize = serializedObject.FindProperty("spawnAreaSize");
        cubePrefab = serializedObject.FindProperty("cubePrefab");
        gizmoColor = serializedObject.FindProperty("gizmoColor");
        spawnedObjectsParent = serializedObject.FindProperty("spawnedObjectsParent");
        inGameMultiplierSlider = serializedObject.FindProperty("inGameMultiplierSlider");
        inGameCubeCounter = serializedObject.FindProperty("inGameCubeCounter");
        cubeSpawnMultiplyer = serializedObject.FindProperty("cubeSpawnMultiplyer");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.LabelField("Spawned cube options", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(cubePrefab, new GUIContent("Cube Prefab", "Cube prefab to be spawned."));
        EditorGUILayout.PropertyField(spawnedObjectsParent, new GUIContent("Spawned Objects Parent", "Parent object for the spawned cubes."));
        EditorGUILayout.IntSlider(cubeSpawnMultiplyer, 1, 10, new GUIContent("Cubes to spawn", "Number of cubes to be spawned."));
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Spawn area", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(gizmoColor, new GUIContent("Area spawn color", "Color for the spawn area gizmo."));
        EditorGUILayout.PropertyField(spawnAreaSize, new GUIContent("Spawn Area Size", "Size of the area where cubes will be spawned."));
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Spawn counter", EditorStyles.boldLabel);
        CubeSpawner spawner = (CubeSpawner)target; // Se obtiene el target directamente
        EditorGUILayout.LabelField("Spawned cubes: ", spawner.GetActiveCubesCounter().ToString());
        
        EditorGUILayout.Space();
        
        EditorGUILayout.PropertyField(inGameMultiplierSlider, new GUIContent("In-game multiplier slider", "Slider to modify the amount of cubes to spawn."));
        EditorGUILayout.PropertyField(inGameCubeCounter, new GUIContent("In-game cube counter", "Text to show the amount of spawned cubes in-game."));
        
        serializedObject.ApplyModifiedProperties();
        
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
        if (GUILayout.Button("Spawn Cube(s)"))    
        {
            spawner.SpawnCube();   
        }        
        if (spawner.GetActiveCubesCounter() > 0 && GUILayout.Button("Destroy Cube(s)"))    
        {
            spawner.DestroySpawnedCubes();   
        }      
    }

    private void OnSceneGUI()
    {
        CubeSpawner spawner = (CubeSpawner)target;
        if (spawner == null || spawnAreaSize == null) return;

        Transform spawnerTransform = spawner.transform;
        Vector3 center = spawnerTransform.position;
        Vector3 size = spawnAreaSize.vector3Value; // Se usa la propiedad serializada

        Handles.color = new Color(1f, 1f, 1f, 0.5f);
        Handles.DrawWireCube(center, size);

        EditorGUI.BeginChangeCheck();
        Vector3 newSize = size;

        Handles.color = Color.red;
        newSize.x = Handles.ScaleSlider(size.x, center, Vector3.right, Quaternion.identity, HandleUtility.GetHandleSize(center), 0.1f);

        Handles.color = Color.green;
        newSize.y = Handles.ScaleSlider(size.y, center, Vector3.up, Quaternion.identity, HandleUtility.GetHandleSize(center), 0.1f);

        Handles.color = Color.blue;
        newSize.z = Handles.ScaleSlider(size.z, center, Vector3.forward, Quaternion.identity, HandleUtility.GetHandleSize(center), 0.1f);

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(spawner, "Resize Spawn Area");
            spawnAreaSize.vector3Value = newSize; // Se modifica la propiedad serializada
            serializedObject.ApplyModifiedProperties(); // Se aplican los cambios correctamente
            EditorUtility.SetDirty(spawner);
        }
    }
}
