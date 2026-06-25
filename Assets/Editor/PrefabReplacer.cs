using UnityEngine;
using UnityEditor;

public class PrefabReplacer : EditorWindow
{
    public GameObject replacementPrefab;

    [MenuItem("Tools/Prefab Replacer")]
    public static void ShowWindow() => GetWindow<PrefabReplacer>("Prefab Replacer");

    void OnGUI()
    {
        replacementPrefab = (GameObject)EditorGUILayout.ObjectField("New Prefab", replacementPrefab, typeof(GameObject), false);
        if (GUILayout.Button("Replace Selected") && replacementPrefab != null)
        {
            ReplaceSelected();
        }
    }

    void ReplaceSelected()
    {
        GameObject[] selected = Selection.gameObjects;
        Undo.SetCurrentGroupName("Replace Prefabs");
        int group = Undo.GetCurrentGroup();

        foreach (GameObject obj in selected)
        {
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(replacementPrefab);
            Undo.RegisterCreatedObjectUndo(newObj, "Replace Prefab");

            newObj.transform.position = obj.transform.position;
            newObj.transform.rotation = obj.transform.rotation;
            newObj.transform.localScale = obj.transform.localScale;
            newObj.transform.SetParent(obj.transform.parent);
            newObj.name = obj.name;

            Undo.DestroyObjectImmediate(obj);
        }
        Undo.CollapseUndoOperations(group);
    }
}