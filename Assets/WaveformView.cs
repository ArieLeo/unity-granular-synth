using UnityEngine;

public class WaveformView : MonoBehaviour
{
    public AudioClip clip;
    public int resolution = 256;

    private MeshFilter meshFilter;

    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
    }

    void Start()
    {
        float[] samples = new float[clip.channels * clip.samples];
        clip.GetData(samples, 0);

        Vector3[] vertices = new Vector3[resolution];
        for (int i = 0; i < resolution; i++)
        {
            float level = samples[(samples.Length * i) / resolution];
            vertices[i] = new Vector3(((3.4f / resolution) * i) - 1.7f, level / 2, 0);
        }
        meshFilter.mesh.vertices = vertices;

        int[] indices = new int[resolution];
        for (int i = 0; i < resolution; i++)
        {
            indices[i] = i;
        }
        meshFilter.mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
    }
}