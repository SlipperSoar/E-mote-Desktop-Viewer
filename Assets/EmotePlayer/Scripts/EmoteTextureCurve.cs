using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("Emote Player/Emote Texture Curve")]
[ExecuteInEditMode]
public class EmoteTextureCurve : MonoBehaviour
{
    [HeaderAttribute("Target")]
    public EmotePlayer targetPlayer;
    [HeaderAttribute("Parameters")]
    [SerializeField, Range(0,1)]
    public float topRate = 0;
    [SerializeField, Range(-1,1)]
    public float topShift = 0;
    [SerializeField, Range(0,1)]
    public float bottomRate = 0;
    [SerializeField, Range(-1,1)]
    public float bottomShift = 0;
    [SerializeField, Range(1,30)]
    public int meshCount = 10;

    private Mesh mesh;
    private float originOffset = 0;
    
    void Start() {
        if (targetPlayer == null)
            targetPlayer = this.GetComponent(typeof(EmotePlayer)) as EmotePlayer;
        mesh = new Mesh();
        mesh.name = "Quad";
    }

    void Update() {
        if (targetPlayer != null) {
            float offset = targetPlayer.renderTextureOriginOffet;
            if (offset != originOffset) {
                originOffset = offset;
                UpdateMesh();
            }
        }
    }

    public void UpdateMesh() {
        if (mesh == null)
            return;
        if (targetPlayer != null)
            targetPlayer.renderTextureMesh = mesh;
        
        Vector3[] vertices = new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, 0),
                new Vector3(0.5f, -0.5f, 0),
                new Vector3(0.5f, 0.5f, 0),
                new Vector3(-0.5f, 0.5f, 0),
            };
        Vector2[] uv = new Vector2[]
            {
                new Vector2(0.0f, 0.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(0.0f, 1.0f)
            };
        
        int xvcount = meshCount + 1;
        int yvcount = 2;
        Vector3[] orig_vertices = vertices;
        Vector2[] orig_uv = uv;
        vertices = new Vector3[xvcount * yvcount];
        uv = new Vector2[xvcount * yvcount];
        Vector3[] top_vertices = new Vector3[xvcount];
        Vector3[] bottom_vertices = new Vector3[xvcount];
        float xMin, xMax;
        if (originOffset < 0) {
            xMin = -(originOffset - (-0.5f)) * 2;
            xMax = 1.0f;
        } else {
            xMin = -1.0f;
            xMax = (0.5f - originOffset) * 2;
        }
                 
        for (int x = 0; x < xvcount; x++) {
            float xr = 1.0f * x / (xvcount - 1);
            Vector3 t_vert = Vector3.Lerp(orig_vertices[3], orig_vertices[2], xr);
            Vector3 b_vert = Vector3.Lerp(orig_vertices[0], orig_vertices[1], xr);
            t_vert.z += (0.25f -Mathf.Sqrt(1 - Mathf.Pow((xr * (xMax - xMin) + xMin), 2)) * 0.5f - topShift * 0.25f) * topRate;
            b_vert.z += (0.25f -Mathf.Sqrt(1 - Mathf.Pow((xr * (xMax - xMin) + xMin), 2)) * 0.5f - bottomShift * 0.25f) * bottomRate;
            top_vertices[x] = t_vert;
            bottom_vertices[x] = b_vert;
        }
        for (int y = 0; y < yvcount; y++) {
            float yr = 1.0f * y / (yvcount - 1);
            Vector2 l_uv = Vector2.Lerp(orig_uv[0], orig_uv[3], yr);
            Vector2 r_uv = Vector2.Lerp(orig_uv[1], orig_uv[2], yr);
            for (int x = 0; x < xvcount; x++) {
                float xr = 1.0f * x / (xvcount - 1);
                Vector3 a_vert = Vector3.Lerp(bottom_vertices[x], top_vertices[x], yr);
                Vector2 a_uv = Vector2.Lerp(l_uv, r_uv, xr);
                int index = y * xvcount + x;
                vertices[index] = a_vert;
                uv[index] = a_uv;
            }
        }
        int[] triangles = new int[(xvcount - 1) * (yvcount - 1) * 2 * 3];
        for (int y = 0; y < yvcount - 1; y++) {
            for (int x = 0; x < xvcount - 1; x++) {
                int t = (y * (xvcount - 1) + x) * 6;
                int index = y * xvcount + x;
                triangles[t + 0] = index;
                triangles[t + 2] = index + 1;
                triangles[t + 1] = index + 1 + xvcount;
                triangles[t + 3] = index;
                triangles[t + 5] = index + 1 + xvcount;
                triangles[t + 4] = index + xvcount;
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

#if UNITY_EDITOR
    void OnValidate() {
        UpdateMesh();
    }
#endif
};