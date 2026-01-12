using UnityEngine;

public class RandomCloneMover : MonoBehaviour
{
    void Start()
    {
        // Tworzymy klona dok³adnie w tym samym miejscu
        GameObject clone = Instantiate(gameObject, transform.position, transform.rotation);

        // Usuwamy ten skrypt z klona (¿eby nie klonowa³ siê w pêtli)
        Destroy(clone.GetComponent<RandomCloneMover>());

        // Dodajemy skrypt ruchu
        clone.AddComponent<CloneMovement>();
    }
}
