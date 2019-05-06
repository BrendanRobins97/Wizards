// File: LaunchArc.cs
// Contributors: Brendan Robinson
// Date Created: 05/05/2019
// Date Last Modified: 05/05/2019

using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LaunchArc : MonoBehaviour {

    #region Fields

    [Range(0, 0.5f)]
    public float meshWidth;
    [Range(0, 200)]
    public float velocity;
    [Range(0, 90)]
    public float angle;
    [Range(1, 100)]
    public int resolution = 10;

    private float maxDistance = 30;

    public Color startColor;
    public Color endColor;

    private Mesh mesh;
    private float g;
    private float radianAngle;

    #endregion

    #region Methods

    private void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
        g = Mathf.Abs(Physics.gravity.y);
        angle = 0;
    }

    public void MakeArcMesh(float velocity, float gravity, float maxDistance = 25f) {
        g = gravity;
        this.velocity = velocity;
        this.maxDistance = maxDistance;
        Vector3[] arcVerts = CalculateArcArray();
        mesh.Clear();
        Vector3[] vertices = new Vector3[(resolution + 1) * 2];
        int[] triangles = new int[resolution * 12];
        Color[] colors = new Color[(resolution + 1) * 2];

        for (int i = 0; i <= resolution; i++) {
            // Make an arrow at the end
            if (i == resolution) {
                vertices[i * 2] = new Vector3(0, arcVerts[i].y, arcVerts[i].x);
                vertices[i * 2 + 1] = new Vector3(0, arcVerts[i].y, arcVerts[i].x);
            } else if (i == resolution - 1) {
                vertices[i * 2] = new Vector3(meshWidth * 1.25f, arcVerts[i].y, arcVerts[i].x);
                vertices[i * 2 + 1] = new Vector3(meshWidth * -2f, arcVerts[i].y, arcVerts[i].x);
            } else if (i == resolution - 2) {
                vertices[i * 2] = new Vector3(meshWidth * 2.5f, arcVerts[i].y, arcVerts[i].x);
                vertices[i * 2 + 1] = new Vector3(meshWidth * -2.5f, arcVerts[i].y, arcVerts[i].x);
            }
            else {
                vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y, arcVerts[i].x);
                vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x);
            }
           
            if (i != resolution) {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }
            
            colors[i * 2] = Color.Lerp(startColor, endColor, (float)(i + resolution / 3f) / resolution);
            colors[i * 2 + 1] = Color.Lerp(startColor, endColor, (float)(i + resolution / 3f) / resolution);
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;
    }

    private Vector3[] CalculateArcArray() {
        Vector3[] arcArray = new Vector3[resolution + 1];

        radianAngle = angle * Mathf.Deg2Rad;
        for (int i = 0; i <= resolution; i++) {
            float t = i / (float) resolution;
            arcArray[i] = CalculateArcPoint(t);
        }

        return arcArray;
    }

    private Vector3 CalculateArcPoint(float t) {
        float x = t * maxDistance;
        float cosAngle = Mathf.Cos(radianAngle);
        float y = x * Mathf.Tan(radianAngle) - g * x * x / (2 * velocity * velocity * cosAngle * cosAngle);
        return new Vector3(x, y);
    }

    #endregion

}