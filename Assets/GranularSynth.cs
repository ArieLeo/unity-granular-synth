using UnityEngine;

public class GranularSynth : MonoBehaviour
{
    public AudioClip clip;

    public int playbackSpeed = 1;
    public int grainSize = 1000;
    public int grainStep = 1;

    public float guiPlaybackSpeed = 1f;
    public float guiGrainSize = 1000f;
    public float guiGrainStep = 1f;

    private int sampleLength;
    private float[] samples;

    private int position;
    private int interval;

    void Awake()
    {
        sampleLength = clip.samples;
        samples = new float[clip.samples * clip.channels];
        clip.GetData(this.samples, 0);
    }

    void Update()
    {
        PositionView cursor = (PositionView) UnityEngine.Object.FindObjectOfType(typeof(PositionView));
        cursor.position = (1f / sampleLength) * position;
        cursor.width = ((1f / sampleLength) * interval) * playbackSpeed;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(16, 16, Screen.width - 32, Screen.height - 32));
        GUILayout.FlexibleSpace();
        GUILayout.Label("Playback Speed: " + playbackSpeed, new GUILayoutOption[] {});
        guiPlaybackSpeed = GUILayout.HorizontalSlider(guiPlaybackSpeed, -4f, 4f, new GUILayoutOption[] {});
        GUILayout.FlexibleSpace();
        GUILayout.Label("Grain Size: " + grainSize, new GUILayoutOption[] {});
        guiGrainSize = GUILayout.HorizontalSlider(guiGrainSize, 2f, 10000f, new GUILayoutOption[] {});
        GUILayout.FlexibleSpace();
        GUILayout.Label("Grain Step: " + grainStep, new GUILayoutOption[] {});
        guiGrainStep = GUILayout.HorizontalSlider(guiGrainStep, -3000f, 3000f, new GUILayoutOption[] {});
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("RANDOMIZE!", new GUILayoutOption[] {}))
        {
            guiPlaybackSpeed = Random.Range(-2f, 2f);
            guiGrainSize = Random.Range(200f, 1000f);
            guiGrainStep = Random.Range(-1500f, 1500f);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();
        playbackSpeed = Mathf.RoundToInt(guiPlaybackSpeed);
        if (playbackSpeed == 0)
        {
            playbackSpeed = 1;
        }
        grainSize = Mathf.RoundToInt(guiGrainSize);
        grainStep = Mathf.RoundToInt(guiGrainStep);
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        for (int i = 0; i < data.Length; i += 2)
        {
            data[i] = samples[position * 2];
            data[i + 1] = samples[position * 2 + 1];

            if (--interval <= 0)
            {
                interval = grainSize;
                position += grainStep;
            }
            else
            {
                position += playbackSpeed;
            }

            while (position >= sampleLength)
            {
                position -= sampleLength;
            }
            while (position < 0)
            {
                position += sampleLength;
            }
        }
    }
}