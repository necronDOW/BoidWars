using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float waitTime = 1.0f;

	public IEnumerator meshSplit (bool destroy, int division = 1)
    {
        if(GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null)
        {
            yield return null;
        }

        if(GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = false;
        }

        Mesh M = new Mesh();
        if(GetComponent<MeshFilter>())
        {
            M = GetComponent<MeshFilter>().mesh;
        }
        else if(GetComponent<SkinnedMeshRenderer>())
        {
            M = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        Material[] materials = new Material[0];
        if(GetComponent<MeshRenderer>())
        {
            materials = GetComponent<MeshRenderer>().materials;
        }
        else if(GetComponent<SkinnedMeshRenderer>())
        {
            materials = GetComponent<SkinnedMeshRenderer>().materials;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;

        for (int subMesh = 0; subMesh < M.subMeshCount; subMesh++)
        {
            int[] indices = M.GetTriangles(subMesh);

            for (int i = 0; i < indices.Length; i += (3*division))
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];

                for (int j = 0; j < 3; j++)
                {
                    int index = indices[i + j];
                    newVerts[j] = verts[index];
                    newUvs[j] = uvs[index];
                    newNormals[j] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject go = new GameObject("Triangle " + (i / 3));
                go.transform.localScale = transform.localScale;
                go.layer = LayerMask.NameToLayer("Particle");
                go.transform.position = transform.position;
                go.transform.rotation = transform.rotation;
                go.AddComponent<MeshRenderer>().material = materials[subMesh];
                go.AddComponent<MeshFilter>().mesh = mesh;
                go.AddComponent<BoxCollider>();
                Vector3 explosionPos = new Vector3(
                    transform.position.x + Random.Range(-0.5f, 0.5f),
                    transform.position.y + Random.Range(-0.5f, 0.5f),
                    transform.position.z + Random.Range(-0.5f, 0.5f));
                go.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(50, 100), explosionPos, 5);
                Destroy(go, 2 + Random.Range(0.0f, 2.0f));
            }
        }

        GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(waitTime);
        if (destroy == true)
        {
            //Destroy(gameObject);
        }
    }
}
