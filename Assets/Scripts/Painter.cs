using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    public GameObject prefab;
    public float minScale = 0.5f;
    public float maxScale = 1.5f;
    public int density = 5;
    public float brushSize = 5.0f;
    public float paintIntensity = 5.0f;
    public int layerMask;
    public List<GameObject> instances;
    public bool randomizeColor = false;
    public Color minColor = Color.white;
    public Color maxColor = Color.white;

    public void Draw(Vector3 position)
    {
        if (prefab)
        {
            for (int i = 0; i < density; i++)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-brushSize, brushSize), 100.0f, Random.Range(-brushSize, brushSize));
                float randomScale = Random.Range(minScale, maxScale);
                Color randomColor = randomizeColor ? RandomBetweenTwoColors(minColor, maxColor) : minColor;

                Vector3 truePosition;

                if (FindPosition(position + randomOffset, out truePosition))
                {
                    GameObject instance = (GameObject)Instantiate(prefab, truePosition, prefab.transform.rotation);
                    instance.transform.localScale = prefab.transform.localScale * randomScale;
                    instance.name = prefab.name;
                    SetInstanceColor(instance.GetComponent<MeshRenderer>(), randomColor);

                    instances.Add(instance);
                }
            }
        }
    }

    public void Undo()
    {
        if (instances.Count > 0)
        {
            for (int i = 0; i < density; i++)
            {
                int index = instances.Count - 1;

                DestroyImmediate(instances[index]);
                instances.RemoveAt(index);
            }
        }
    }

    private bool FindPosition(Vector3 castFrom, out Vector3 position)
    {
        RaycastHit hit;
        Ray ray = new Ray(castFrom, Vector3.down);

        if (Physics.Raycast(ray, out hit, castFrom.y + 1.0f, layerMask))
        {
            position = hit.point;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    private Color RandomBetweenTwoColors(Color A, Color B)
    {
        float t = Random.Range(0.0f, 1.0f);
        float r = A.r + ((B.r - A.r) * t);
        float g = A.g + ((B.g - A.g) * t);
        float b = A.b + ((B.b - A.b) * t);

        return new Color(r, g, b);
    }

    private void SetInstanceColor(MeshRenderer renderer, Color color)
    {
        Color diffuse = renderer.sharedMaterial.GetColor("_Color") * color;
        Color emission = renderer.sharedMaterial.GetColor("_EmissionColor") * color;

        MaterialLibrary.Get().SetRenderColor(renderer, diffuse, emission);
    }
}
