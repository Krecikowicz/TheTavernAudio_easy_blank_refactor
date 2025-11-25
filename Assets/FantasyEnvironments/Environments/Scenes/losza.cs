using UnityEngine;

public class RotateRight : MonoBehaviour
{
    public float rotationSpeed = 90f; // stopnie na sekundê

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
