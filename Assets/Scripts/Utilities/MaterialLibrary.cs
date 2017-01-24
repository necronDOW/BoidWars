using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MaterialLibrary
{
    private List<Material> _materialLibrary;

    private static MaterialLibrary _instance = null;
    public static MaterialLibrary Get()
    {
        if (_instance == null)
        {
            _instance = new MaterialLibrary();
            _instance.Initialize();
        }

        return _instance;
    }

    private void Initialize()
    {
        _materialLibrary = new List<Material>();
        Material[] materials = (Material[])Resources.FindObjectsOfTypeAll(typeof(Material));

        for (int i = 0; i < materials.Length; i++)
        {
            if (materials[i].name.Contains("_Material"))
            {
                NewMaterial(materials[i]);
            }
        }
    }

    public Material NewMaterial(Material m)
    {
        for (int i = 0; i < _materialLibrary.Count; i++)
        {
            if (CompareMaterials(m, _materialLibrary[i]))
            {
                return _materialLibrary[i];
            }
        }

        _materialLibrary.Add(new Material(m));
        return _materialLibrary[_materialLibrary.Count - 1];
    }

    private bool CompareMaterials(Material A, Material B)
    {
        if ((A.GetColor("_Color") == B.GetColor("_Color"))
            && (A.GetColor("_EmissionColor") == B.GetColor("_EmissionColor"))
            && (A.GetTexture("_MainTex") == B.GetTexture("_MainTex")))
        {
            return true;
        }

        return false;
    }
}
