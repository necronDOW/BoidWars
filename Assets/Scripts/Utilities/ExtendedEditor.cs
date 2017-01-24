using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

public class ExtendedEditor : MonoBehaviour
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

    public static bool RaycastFromScreen(out Vector3 point, Vector3 target, LayerMask targetLayer)
    {
        RaycastHit hit;
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        float distance = Vector3.Distance(ray.origin, target);

        if (Physics.Raycast(ray, out hit, distance, targetLayer))
        {
            point = hit.point;
            return true;
        }
        else
        {
            point = Vector3.zero;
            return false;
        }
    }
}
