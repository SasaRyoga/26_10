
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SpreadAreaView : MonoBehaviour
{
    [Header("Spread Settings")]
    [SerializeField] private float _spreadAngle = 4f;
    [SerializeField] private float _distance = 10f;
    [SerializeField] private int _segments = 20;


    private Mesh _mesh;

    private WeaponType _weaponType;

    private void Awake()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        _mesh = new Mesh();
        _mesh.name = "SpreadAreaMesh";

        meshFilter.sharedMesh = _mesh;

        UpdateMesh();
    }


    public void SetSpreadAngle(float angle)
    {
        _spreadAngle = Mathf.Max(0f, angle);
        UpdateMesh();
    }

    public void SetDistance(float distance)
    {
        _distance = Mathf.Max(0f, distance);
        UpdateMesh();
    }
    public void SetVisible(bool visible)
    {
        GetComponent<MeshRenderer>().enabled = visible;
    }


    private void UpdateMesh()
    {
        int segments = Mathf.Max(1, _segments);

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        // 扇形の中心
        vertices[0] = Vector3.zero;

        // 外周の頂点を作成
        for (int i = 0; i <= segments; i++)
        {
            float t = (float)i / segments;

            float angle = Mathf.Lerp(
                -_spreadAngle,
                _spreadAngle,
                t
            );

            Vector3 localDirection =
                Quaternion.AngleAxis(angle, Vector3.up)
                * Vector3.forward;

            // 射程距離まで伸ばす
            vertices[i + 1] = localDirection * _distance;
        }

        // 三角形を作成
        for (int i = 0; i < segments; i++)
        {
            int index = i * 3;

            triangles[index] = 0;
            triangles[index + 1] = i + 1;
            triangles[index + 2] = i + 2;
        }

        // Meshに反映
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();
    }
}
