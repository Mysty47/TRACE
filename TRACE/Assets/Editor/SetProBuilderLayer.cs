using UnityEngine;
using UnityEditor;
using UnityEngine.ProBuilder;

public class SetProBuilderLayer : EditorWindow
{
    private int targetLayer = 0;

    [MenuItem("Tools/ProBuilder/Set Layer for All ProBuilder Objects")]
    public static void ShowWindow()
    {
        GetWindow<SetProBuilderLayer>("Set ProBuilder Layer");
    }

    private void OnGUI()
    {
        targetLayer = EditorGUILayout.LayerField("Target Layer", targetLayer);

        if (GUILayout.Button("Apply to All ProBuilder Objects"))
        {
            ApplyLayerToProBuilderObjects(targetLayer);
        }
    }

    private void ApplyLayerToProBuilderObjects(int layer)
    {
        ProBuilderMesh[] pbMeshes = FindObjectsOfType<ProBuilderMesh>();
        int count = 0;

        foreach (var pb in pbMeshes)
        {
            pb.gameObject.layer = layer;
            count++;
        }

        Debug.Log($"✅ Set layer '{LayerMask.LayerToName(layer)}' on {count} ProBuilder objects.");
    }
}