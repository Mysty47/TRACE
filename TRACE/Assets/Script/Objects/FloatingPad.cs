using UnityEngine;

public class FloatingPad : MonoBehaviour
{
    [Header("Settings")]
    public float amplitude = 0.1f;
    public float frequency = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float y = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0, y, 0);
    }
}