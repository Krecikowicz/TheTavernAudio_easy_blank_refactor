using UnityEngine;

public class CloneMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 0.25f;
    public float rotationSpeed = 15f;

    [Header("Flight Area")]
    public Vector3 areaSize = new Vector3(3f, 2f, 3f);

    Vector3 areaCenter;
    Vector3 moveDirection;
    Vector3 rotationDirection;

    void Start()
    {
        // Ustawiamy pole lotu dok³adnie tam, gdzie powsta³ klon
        areaCenter = transform.position;

        // Bezpieczny losowy kierunek
        moveDirection = Random.onUnitSphere;
        moveDirection.y *= 0.5f;
        moveDirection.Normalize();

        // Delikatny losowy obrót
        rotationDirection = Random.insideUnitSphere * 0.5f;
    }

    void Update()
    {
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);

        KeepInsideArea();
    }

    void KeepInsideArea()
    {
        Vector3 pos = transform.position;

        Vector3 min = areaCenter - areaSize * 0.5f;
        Vector3 max = areaCenter + areaSize * 0.5f;

        // Odbicie od granic
        if (pos.x <= min.x || pos.x >= max.x) moveDirection.x *= -1;
        if (pos.y <= min.y || pos.y >= max.y) moveDirection.y *= -1;
        if (pos.z <= min.z || pos.z >= max.z) moveDirection.z *= -1;

        // Twarde ograniczenie (¿eby NIGDY nie wylecia³)
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        pos.z = Mathf.Clamp(pos.z, min.z, max.z);

        transform.position = pos;
    }

    // PODGL¥D POLA W SCENE VIEW
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(
            Application.isPlaying ? areaCenter : transform.position,
            areaSize
        );
    }
}
