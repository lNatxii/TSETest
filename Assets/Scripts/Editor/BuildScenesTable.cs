using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildScenesTable
{
    public struct SceneItem
    {
        public string name;
        public string path;
    }
    //Extraído de https://github.com/redclock/SimpleEditorTableView
    private SimpleEditorTableView<SceneItem> _tableView;
    private SceneItem[] _sceneInBuildSettings = Array.Empty<SceneItem>();

    private SimpleEditorTableView<SceneItem> CreateTable(Rect position)
    {
        SimpleEditorTableView<SceneItem> tableView = new SimpleEditorTableView<SceneItem>();

        GUIStyle labelGUIStyle = new GUIStyle(GUI.skin.label)
        {
            padding = new RectOffset(left: 10, right: 10, top: 2, bottom: 2)
        };
        
        var nameColumn = tableView.AddColumn("Name", 80, (rect, item) =>
        {
            GUIStyle style = labelGUIStyle;
            EditorGUI.LabelField(
                position: rect,
                label: item.name,
                style: style
            );
        });
    
        nameColumn.SetTooltip("Scene name");
        nameColumn.SetAutoResize(true);
        nameColumn.SetSorting((a, b) => String.Compare(a.name, b.name, StringComparison.Ordinal));
        
    
        var loadColumn = tableView.AddColumn("Load", 80, (rect, item) =>
        {
            bool isActiveScene = IsCurrentSceneTheActiveScene(item);
            Texture2D buttonTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(
                isActiveScene ? "Assets/Editor/Icons/reload.png" : "Assets/Editor/Icons/load.png"
            );
            if (GUI.Button(rect, buttonTexture, GUI.skin.button))
            {
                EditorSceneManager.OpenScene(item.path, OpenSceneMode.Single);
            }
        });
        loadColumn.SetTooltip("Click to load this scene");
        loadColumn.SetAutoResize(false);
        loadColumn.SetWidth(80);
        

        var removeColumn = tableView.AddColumn("Remove", 80, (rect, item) =>
        {
            Texture2D removeTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Icons/remove.png");
            if (GUI.Button(rect, removeTexture, GUI.skin.button))
            {
                RemoveSceneFromBuildSettings(item.path);
            }
        });
        removeColumn.SetTooltip("Click to delete this scene");
        removeColumn.SetAutoResize(false);
        removeColumn.SetWidth(80);
        return tableView;
    }

    static bool IsCurrentSceneTheActiveScene(SceneItem item)
    {
        return SceneManager.GetActiveScene().path == item.path;
    }
    
    public void DrawTable(Rect position)
    {
        if (_tableView == null)
            _tableView = CreateTable(position);
        
        _sceneInBuildSettings = GetScenesInBuildSettings();
        _tableView.DrawTableGUI(_sceneInBuildSettings);

        StatusGUI();
    }
    
    private void StatusGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Total Scene Count: " + _sceneInBuildSettings.Length);
        EditorGUILayout.EndHorizontal();
    }

    private SceneItem[] GetScenesInBuildSettings()
    {
        var sceneInBuildSettings = new List<SceneItem>();
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if(scene.enabled)
            {
                sceneInBuildSettings.Add(new SceneItem
                {
                    name = Path.GetFileNameWithoutExtension(scene.path),
                    path = scene.path,
                });
            }
        }
        return sceneInBuildSettings.ToArray();
    }

    public void RemoveSceneFromBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        scenes.Remove(scenes.Find(s => s.path == scenePath));
        EditorBuildSettings.scenes = scenes.ToArray();
     
        Debug.Log("Scene removed from Build Settings: " + scenePath);
    }
}