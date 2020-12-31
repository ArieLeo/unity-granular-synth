using UnityEngine;

public class PositionView : MonoBehaviour
{
    public float position = 0f;
    public float width = 1f;

    void Update()
    {
        Vector3 localPosition = transform.localPosition;
        Vector3 localScale = transform.localScale;

        localPosition.x = 3.4f * (position + width / 2) - 1.7f;
        localScale.x = 3.4f * width;

        transform.localPosition = localPosition;
        transform.localScale = localScale;
    }
}