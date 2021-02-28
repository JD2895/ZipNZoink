using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;

[CustomEditor(typeof(ProcGenTiler))]
public class ProcGenEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        ProcGenTiler procGenTiler = (ProcGenTiler)target;

        procGenTiler.plannerTMap = (Tilemap)EditorGUILayout.ObjectField("Planner Tilemap", procGenTiler.plannerTMap, typeof(Tilemap), true);
        procGenTiler.baseTMap = (Tilemap)EditorGUILayout.ObjectField("Base Tilemap", procGenTiler.baseTMap, typeof(Tilemap), true);
        procGenTiler.hazardTMap = (Tilemap)EditorGUILayout.ObjectField("Hazard Tilemap", procGenTiler.hazardTMap, typeof(Tilemap), true);

        procGenTiler.plannerGroundTile = (TileBase)EditorGUILayout.ObjectField("Planner Ground Tile", procGenTiler.plannerGroundTile, typeof(TileBase), true);
        procGenTiler.plannerHazardTile = (TileBase)EditorGUILayout.ObjectField("Planner Ground Tile", procGenTiler.plannerHazardTile, typeof(TileBase), true);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Generate Tiles"))
        {
            procGenTiler.GenerateTiles();
        }

        if (GUILayout.Button("Clear Tiles"))
        {
            procGenTiler.ClearTiles();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Hide/Show Planner"))
        {
            procGenTiler.HideShowPlanner();
        }

        if (GUILayout.Button("Hide/Show Actual"))
        {
            procGenTiler.HideShowActual();
        }
        GUILayout.EndHorizontal();

        serializedObject.Update();
        Show(serializedObject.FindProperty("center_TileList"), procGenTiler.center_TileList);
        Show(serializedObject.FindProperty("bottom_TileList"), procGenTiler.bottom_TileList);
        Show(serializedObject.FindProperty("left_TileList"), procGenTiler.left_TileList);
        Show(serializedObject.FindProperty("top_TileList"), procGenTiler.top_TileList);
        Show(serializedObject.FindProperty("right_TileList"), procGenTiler.right_TileList);
        Show(serializedObject.FindProperty("topRight_TileList"), procGenTiler.topRight_TileList);
        Show(serializedObject.FindProperty("bottomRight_TileList"), procGenTiler.bottomRight_TileList);
        Show(serializedObject.FindProperty("bottomLeft_TileList"), procGenTiler.bottomLeft_TileList);
        Show(serializedObject.FindProperty("topLeft_TileList"), procGenTiler.topLeft_TileList);
        Show(serializedObject.FindProperty("topLeftInner_TileList"), procGenTiler.topLeftInner_TileList);
        Show(serializedObject.FindProperty("topRightInner_TileList"), procGenTiler.topRightInner_TileList);
        Show(serializedObject.FindProperty("bottomRightInner_TileList"), procGenTiler.bottomRightInner_TileList);
        Show(serializedObject.FindProperty("bottomLeftInner_TileList"), procGenTiler.bottomLeftInner_TileList);
        serializedObject.ApplyModifiedProperties();
    }

    public static void Show(SerializedProperty list, List<TileAndChance> procGenTilerList)
    {
        float listTotal = 0f;

        EditorGUILayout.PropertyField(list, false);
        EditorGUI.indentLevel += 1;
        if (list.isExpanded)
        {
            EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
            List<TileAndChance> TnClist = SerializeTools.SerializedPropertyToObject<List<TileAndChance>>(list);
            for (int i = 0; i < TnClist.Count; i++)
            {
                GUILayout.BeginHorizontal();
                procGenTilerList[i].tile = (TileBase)EditorGUILayout.ObjectField(TnClist[i].tile, typeof(TileBase), true);
                EditorGUI.BeginChangeCheck();
                procGenTilerList[i].chance = EditorGUILayout.Slider(TnClist[i].chance, 0f, 100f, GUILayout.MaxWidth(150));

                //Change other values
                listTotal += procGenTilerList[i].chance;
                if (EditorGUI.EndChangeCheck())
                {
                    for (int restCheck = i + 1; restCheck < TnClist.Count; restCheck++)
                    {
                        listTotal += TnClist[restCheck].chance;
                    }

                    float listTotalLessNewChange = listTotal - procGenTilerList[i].chance;
                    float maxTotalLessNewChange = 100f - procGenTilerList[i].chance;
                    float averageDifference = (maxTotalLessNewChange - listTotalLessNewChange) / (procGenTilerList.Count - 1);

                    // Change everything before the entry that was changed
                    for (int changeBefore = 0; changeBefore < i; changeBefore++)
                    {
                        procGenTilerList[changeBefore].chance += averageDifference;
                    }

                    // Change everything after the entry that was changed
                    for (int changeAfter = i + 1; changeAfter < TnClist.Count; changeAfter++)
                    {
                        procGenTilerList[changeAfter].chance += averageDifference;
                    }
                    //labelText = sliderValue.ToString();
                }

                GUILayout.EndHorizontal();
            }
        }
        EditorGUI.indentLevel -= 1;
    }
}

//From Unity Answers. Hell if I know how it works
public static class SerializeTools
{
    public static T SerializedPropertyToObject<T>(SerializedProperty property)
    {
        return GetNestedObject<T>(property.propertyPath, GetSerializedPropertyRootComponent(property), true); //The "true" means we will also check all base classes
    }

    public static Component GetSerializedPropertyRootComponent(SerializedProperty property)
    {
        return (Component)property.serializedObject.targetObject;
    }

    public static T GetNestedObject<T>(string path, object obj, bool includeAllBases = false)
    {
        foreach (string part in path.Split('.'))
        {
            obj = GetFieldOrPropertyValue<object>(part, obj, includeAllBases);
        }
        return (T)obj;
    }

    public static T GetFieldOrPropertyValue<T>(string fieldName, object obj, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, bindings);
        if (field != null) return (T)field.GetValue(obj);

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
        if (property != null) return (T)property.GetValue(obj, null);

        if (includeAllBases)
        {

            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                field = type.GetField(fieldName, bindings);
                if (field != null) return (T)field.GetValue(obj);

                property = type.GetProperty(fieldName, bindings);
                if (property != null) return (T)property.GetValue(obj, null);
            }
        }

        return default(T);
    }

    public static void SetFieldOrPropertyValue<T>(string fieldName, object obj, object value, bool includeAllBases = false, BindingFlags bindings = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
    {
        FieldInfo field = obj.GetType().GetField(fieldName, bindings);
        if (field != null)
        {
            field.SetValue(obj, value);
            return;
        }

        PropertyInfo property = obj.GetType().GetProperty(fieldName, bindings);
        if (property != null)
        {
            property.SetValue(obj, value, null);
            return;
        }

        if (includeAllBases)
        {
            foreach (Type type in GetBaseClassesAndInterfaces(obj.GetType()))
            {
                field = type.GetField(fieldName, bindings);
                if (field != null)
                {
                    field.SetValue(obj, value);
                    return;
                }

                property = type.GetProperty(fieldName, bindings);
                if (property != null)
                {
                    property.SetValue(obj, value, null);
                    return;
                }
            }
        }
    }

    public static IEnumerable<Type> GetBaseClassesAndInterfaces(this Type type, bool includeSelf = false)
    {
        List<Type> allTypes = new List<Type>();

        if (includeSelf) allTypes.Add(type);

        if (type.BaseType == typeof(object))
        {
            allTypes.AddRange(type.GetInterfaces());
        }
        else
        {
            allTypes.AddRange(
                    Enumerable
                    .Repeat(type.BaseType, 1)
                    .Concat(type.GetInterfaces())
                    .Concat(type.BaseType.GetBaseClassesAndInterfaces())
                    .Distinct());
            //I found this on stackoverflow
        }

        return allTypes;
    }
}