using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GenerateCollider : MonoBehaviour
{
    public Transform[] targets;
    [HideInInspector]
    [SerializeField]
    public int colliderCount = 0;
    [HideInInspector]
    public List<BoxCollider> colliders = new List<BoxCollider>();

    private bool _visible = false;

    private void Awake()
    {
        foreach (Transform t in targets)
        {
            MeshFilter tMeshFilter = t.GetComponent<MeshFilter>();

            if (t && tMeshFilter && tMeshFilter.mesh.vertexCount > 0)
            {
                Transform child = new GameObject(t.name + "_collider").transform;
                child.parent = t;

                BoxCollider bc = child.gameObject.AddComponent<BoxCollider>();
                bc.size = GetDimensions(t);

                child.localPosition = new Vector3(0f, 0f, bc.size.z / 2.0f);
                child.localEulerAngles = Vector3.zero;
            }
        }
    }

    public void UpdateColliders()
    {
        for (int i = colliderCount; i < targets.Length; i++)
        {
            MeshFilter tMeshFilter = targets[i].GetComponent<MeshFilter>();

            if (targets[i] && tMeshFilter && tMeshFilter.sharedMesh.vertexCount > 0)
            {
                Transform child = new GameObject(targets[i].name + "_collider").transform;
                child.parent = targets[i];

                BoxCollider bc = child.gameObject.AddComponent<BoxCollider>();
                bc.size = GetDimensions(targets[i]);

                child.localPosition = new Vector3(0f, 0f, bc.size.z / 2.0f);
                child.localEulerAngles = Vector3.zero;

                child.transform.parent = transform;

                colliders.Add(bc);
            }
        }

        //UpdateCollidersVisible(colliderCount);

        colliderCount = colliders.Count;
    }

    public void DeleteCollider(int index)
    {
        if (index >= 0 && index < colliders.Count)
        {
            if (colliders[index])
            {
                DestroyImmediate(colliders[index].gameObject);
            }
            colliders.RemoveAt(index);
        }
    }

    private void UpdateCollidersVisible(int start = 0)
    {
        HideFlags flag = _visible ? HideFlags.None : HideFlags.HideInHierarchy;

        for (int i = start; i < colliders.Count; i++)
        {
            colliders[i].gameObject.hideFlags = flag;
        }
    }

    public void ToggleCollidersVisible()
    {
        _visible = !_visible;
        UpdateCollidersVisible();
    }

    public string VisibleText()
    {
        return _visible ? "Hide Colliders" : "Show Colliders";
    }

    private Vector3 GetDimensions(Transform t)
    {
        Vector3 min = Vector3.one * Mathf.Infinity; 
        Vector3 max = Vector3.one * Mathf.NegativeInfinity;

        Mesh m = t.GetComponent<MeshFilter>().sharedMesh;
        foreach (Vector3 v in m.vertices)
        {
            min = Vector3.Min(min, v);
            max = Vector3.Max(max, v);
        }

        return Vector3.Scale(max-min, t.localScale);
    }
}
