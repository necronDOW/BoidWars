using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateCollider))]
public class GenerateCollider_Editor : Editor
{
    private GenerateCollider _target;

    private void OnEnable()
    {
        _target = (GenerateCollider)target;
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

            Transform parent = ExtendedEditor.GetObject<Transform>(targets);
            if (parent && !parent.GetComponent<MeshFilter>())
            {
                AddForChildren(ExtendedEditor.GetObject<Transform>(targets), _target);
            }
        }
        
        // Generate Colliders button press
        GUILayout.Space(5);
        if (GUILayout.Button("Update Colliders"))
        {
            _target.UpdateColliders();
        }

        if (_target.targets != null)
        {
            int colliderDifference = _target.targets.Length - _target.colliderCount;
            if (colliderDifference <= 0)
            {
                for (int i = 0; i < (colliderDifference * -1); i++)
                {
                    _target.DeleteCollider(_target.colliders.Count - 1);
                }

                _target.colliderCount += colliderDifference;
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
