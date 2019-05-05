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
    [Range(1, 100)]
    public float maxDistance = 30;

    private Mesh mesh;
    private float g;
    private float radianAngle;

    #endregion

    #region Methods

    // Start is called before the first frame update
    private void Awake() {
        mesh = GetComponent<MeshFilter>().mesh;
        g = Mathf.Abs(Physics.gravity.y);
        MakeArcMesh(velocity, g);
        angle = 0;
    }

    // Update is called once per frame
    private void Update() { }

    private void OnValidate() {
        if (mesh && Application.isPlaying) { MakeArcMesh(velocity, g); }
    }

    public void MakeArcMesh(float velocity, float gravity, float maxDistance = 30f) {
        g = gravity;
        this.velocity = velocity;
        this.maxDistance = maxDistance;
        Vector3[] arcVerts = CalculateArcArray();
        mesh.Clear();
        Vector3[] vertices = new Vector3[(resolution + 1) * 2];
        int[] triangles = new int[resolution * 12];

        for (int i = 0; i <= resolution ; i++) {
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y, arcVerts[i].x);
            vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x);
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
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
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