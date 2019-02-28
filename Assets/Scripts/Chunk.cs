// File: Chunk.cs
// Author: Brendan Robinson
// Date Created: 02/22/2019
// Date Last Modified: 02/28/2019

using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {

    #region Fields

    public Int2 position;

    [HideInInspector] public int width;
    [HideInInspector] public int length;

    private MeshFilter   filter;
    private MeshCollider collider;

    #endregion

    #region Methods

    // Start is called before the first frame update
    private void Awake() {
        filter = GetComponent<MeshFilter>();
        collider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    private void Update() { }

    public void UpdateChunk(ref Vector3[,] meshPoints) {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int i = position.x; i < position.x + width; i++) {
            for (int j = position.y; j < position.y + length; j++) {
                vertices.Add(meshPoints[i, j]);
                vertices.Add(meshPoints[i + 1, j]);
                vertices.Add(meshPoints[i + 1, j + 1]);
                vertices.Add(meshPoints[i, j + 1]);

                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 4);
                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 4);
            }
        }

        Destroy(filter.mesh);
        filter.mesh = new Mesh();
        filter.mesh.vertices = vertices.ToArray();
        filter.mesh.triangles = triangles.ToArray();
        filter.mesh.RecalculateNormals();
        vertices.Clear();
        triangles.Clear();

        collider.sharedMesh = filter.mesh;
    }

    #endregion

}