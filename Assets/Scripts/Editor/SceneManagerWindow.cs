using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class SceneManagerWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private BuildScenesTable sceneTable; //Objeto BuildScenesTable 
    private bool isDraggingOver = false; // Variable para indicar si un objeto está en la zona de drop, para habilitar elementos visuales de interacción
    private Texture2D dropTexture; // Variable local para almacenar la textura de drop y reutilizarla cuando sea necesario
    
    [MenuItem("Window/Scene Manager")]
    public static void ShowWindow()
    {
        GetWindow<SceneManagerWindow>("Scene Manager");
    }
    
    private Texture2D GetDropTexture()
    {
        if(dropTexture == null)
            dropTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/UI/drop.png");
        return dropTexture;
    }
    

    private void OnEnable()
    {
        sceneTable = new BuildScenesTable();
    }

    private void OnGUI()
    {
        // Lista de escenas con scroll
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        sceneTable.DrawTable(position);
        EditorGUILayout.EndScrollView();

        // Línea divisoria
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // Zona de arrastrar y soltar escenas
        GUILayout.Label("Drag a scene here to add to Build Settings", EditorStyles.boldLabel);
        Rect dropArea = GUILayoutUtility.GetRect(0f, position.height * 0.4f, GUILayout.ExpandWidth(true));

        // Estilo del recuadro interactivo
        GUIStyle dropBoxStyle = new GUIStyle(EditorStyles.helpBox);
        if (isDraggingOver)
        {
            dropBoxStyle.normal.background = GetDropTexture();
        }

        GUI.Box(dropArea, "", dropBoxStyle);

        // Estilo del texto centrado
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = isDraggingOver ? Color.white : Color.gray },
            fontStyle = FontStyle.Bold
        };

        GUI.Label(dropArea, "Drop Scene Here", centeredStyle);

        HandleDragAndDrop(dropArea);
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event currentEvent = Event.current;

        switch (currentEvent.type)
        {
            case EventType.DragUpdated:
                if (dropArea.Contains(currentEvent.mousePosition))
                {
                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    isDraggingOver = true;
                    Event.current.Use();
                }
                else
                {
                    isDraggingOver = false;
                }
                break;

            case EventType.DragPerform:
                if (dropArea.Contains(currentEvent.mousePosition))
                {
                    DragAndDrop.AcceptDrag();
                    foreach (Object draggedObject in DragAndDrop.objectReferences)
                    {
                        if (draggedObject is SceneAsset sceneAsset)
                        {
                            AddSceneToBuildSettings(sceneAsset);
                        }
                    }
                    Event.current.Use();
                }
                isDraggingOver = false;
                break;

            case EventType.DragExited:
                isDraggingOver = false;
                break;
        }
    }

    private void AddSceneToBuildSettings(SceneAsset sceneAsset)
    {
        string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        if (!scenes.Exists(s => s.path == scenePath))
        {
            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log("Scene added to Build Settings: " + scenePath);
        }
    }

}
