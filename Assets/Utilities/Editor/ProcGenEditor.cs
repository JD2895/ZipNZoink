using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProcGenTiler))]
public class ProcGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProcGenTiler procGenTiler = (ProcGenTiler)target;

        procGenTiler.plannerTMap = (Tilemap)EditorGUILayout.ObjectField("Planner Tilemap", procGenTiler.plannerTMap, typeof(Tilemap), true);
        procGenTiler.baseTMap = (Tilemap)EditorGUILayout.ObjectField("Base Tilemap", procGenTiler.baseTMap, typeof(Tilemap), true);
        procGenTiler.hazardTMap = (Tilemap)EditorGUILayout.ObjectField("Hazard Tilemap", procGenTiler.hazardTMap, typeof(Tilemap), true);

        procGenTiler.plannerGroundTile = (TileBase)EditorGUILayout.ObjectField("Planner Ground Tile", procGenTiler.plannerGroundTile, typeof(TileBase), true);
        procGenTiler.plannerHazardTile = (TileBase)EditorGUILayout.ObjectField("Planner Ground Tile", procGenTiler.plannerHazardTile, typeof(TileBase), true);

        serializedObject.Update();
        EditorList.Show(serializedObject.FindProperty("center_TileList"));
        /*EditorList.Show(serializedObject.FindProperty("bottom_TileList"));
        EditorList.Show(serializedObject.FindProperty("left_TileList"), true);
        EditorList.Show(serializedObject.FindProperty("top_TileList"), true);
        EditorList.Show(serializedObject.FindProperty("right_TileList"), true);
        EditorList.Show(serializedObject.FindProperty("topRight_TileList"), true);
        EditorList.Show(serializedObject.FindProperty("bottomRight_TileList"), true);
        EditorList.Show(serializedObject.FindProperty("bottomLeft_TileList"), true);
        EditorList.Show(serializedObject.FindProperty("topLeft_TileList"), true);
        EditorList.Show(serializedObject.FindProperty("topLeftInner_TileList"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("topRightInner_TileList"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bottomRightInner_TileList"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bottomLeftInner_TileList"), true);*/
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();


        if (GUILayout.Button("Generate Tiles"))
        {
            procGenTiler.GenerateTiles();
        }

        if (GUILayout.Button("Clear Tiles"))
        {
            procGenTiler.ClearTiles();
        }
    }
}
public static class EditorList
{
    public static void Show(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list, false);
        EditorGUI.indentLevel += 1;
        if (list.isExpanded)
        {
            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            for (int i = 0; i < list.arraySize; i++)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i), false);
                //list.GetArrayElementAtIndex(i).objectReferenceValue = EditorGUILayout.ObjectField("Tile and chance", list.GetArrayElementAtIndex(i).objectReferenceValue, typeof(TileAndChance), true);
                //list.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(list.GetArrayElementAtIndex(i).floatValue);
                //EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).floatValue);

                //EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).FindPropertyRelative("Array.size"));
                //for (int j = 0; j < list.GetArrayElementAtIndex(i).arraySize; j++)
                //{
                //EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i).GetArrayElementAtIndex(j));
                //}
                GUILayout.EndHorizontal();

                //EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
            }
        }
        EditorGUI.indentLevel -= 1;
    }
}
