using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class SerializedPropertyExt
{
    public static T GetObject<T>(SerializedProperty property) where T : class
    {
        SerializedObject serializedObject = property.serializedObject;
        if (serializedObject == null)
        {
            return null;
        }

        Object targetObject = serializedObject.targetObject;
        object obj = targetObject.GetType().GetField(property.name).GetValue(targetObject);
        if (obj == null)
        {
            return null;
        }
        
        T actualObject = null;
        if (obj.GetType().IsArray)
        {
            int index = ((object[])obj).Length - 1;

            if (index >= 0)
            {
                actualObject = ((T[])obj)[0];
            }
        }
        else
        {
            actualObject = obj as T;
        }

        return actualObject;
    }
    public static T GetObject<T>(SerializedObject serializedObject) where T : class
    {
        Object targetObject = serializedObject.targetObject;
        object obj = targetObject;
        if (obj == null)
        {
            return null;
        }

        T actualObject = null;
        if (obj.GetType().IsArray)
        {
            int index = ((object[])obj).Length - 1;

            if (index >= 0)
            {
                actualObject = ((T[])obj)[0];
            }
        }
        else
        {
            actualObject = obj as T;
        }

        return actualObject;
    }
}

[CustomEditor(typeof(GenerateCollider))]
public class GenerateCollider_Editor : Editor
{
    private GenerateCollider gc;

    private void OnEnable()
    {
        gc = (GenerateCollider)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // List drag & drop event
        EditorGUI.BeginChangeCheck();
        SerializedProperty targets = serializedObject.FindProperty("targets");
        EditorGUILayout.PropertyField(targets, true);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();

            Transform parent = SerializedPropertyExt.GetObject<Transform>(targets);
            if (parent && !parent.GetComponent<MeshFilter>())
            {
                AddForChildren(SerializedPropertyExt.GetObject<Transform>(targets), gc);
            }
        }
        
        // Generate Colliders button press
        GUILayout.Space(5);
        if (GUILayout.Button("Update Colliders"))
        {
            gc.UpdateColliders();
        }

        if (gc.targets != null)
        {
            int colliderDifference = gc.targets.Length - gc.colliderCount;
            if (colliderDifference <= 0)
            {
                for (int i = 0; i < (colliderDifference * -1); i++)
                {
                    gc.DeleteCollider(gc.colliders.Count - 1);
                }

                gc.colliderCount += colliderDifference;
            }
        }

        // Show colliders
        //if (GUILayout.Button(gc.VisibleText()))
        //{
        //    gc.ToggleCollidersVisible();
        //}
    }

    private void AddForChildren(Transform parent, GenerateCollider gc)
    {
        if (parent && parent.childCount > 0)
        {
            int gcLenModified = gc.targets.Length - 1;

            Transform[] newTargets = new Transform[gcLenModified + parent.childCount];

            for (int i = 0; i < gcLenModified; i++)
            {
                newTargets[i] = gc.targets[i + 1];
            }

            for (int i = 0; i < parent.childCount; i++)
            {
                newTargets[gcLenModified + i] = parent.GetChild(i);
            }

            gc.targets = newTargets;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
