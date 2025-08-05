using UnityEngine;

public class Floating : MonoBehaviour
{
    //float up and down
    public float floatSpeed = 2f;  // Speed of floating
    public float floatHeight = 0.5f; // Height of floating movement

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Create a floating effect using a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
