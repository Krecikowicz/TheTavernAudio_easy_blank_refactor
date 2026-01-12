using UnityEngine;
using System.Collections.Generic;

public class ide : MonoBehaviour
{
    [Header("Ustawienia Ścieżki")]
    [Tooltip("Przeciągnij tutaj swoje puste obiekty (Empty GameObjects)")]
    public List<Transform> punktySciezki;

    [Header("Ustawienia Ruchu")]
    public float predkosc = 5f;
    public float tolerancjaOdleglosci = 0.1f; // Jak blisko musi być, żeby uznać, że dotarł

    private int aktualnyIndeks = 0;
    private bool idzieDoPrzodu = true;

    void Start()
    {
        // Opcjonalnie: Ustawienie obiektu w pierwszym punkcie na start
        if (punktySciezki != null && punktySciezki.Count > 0)
        {
            transform.position = punktySciezki[0].position;
        }
    }

    void Update()
    {
        // Zabezpieczenie: jeśli nie ma wystarczającej liczby punktów, nie rób nic
        if (punktySciezki == null || punktySciezki.Count < 2) return;

        RuszajSie();
    }

    void RuszajSie()
    {
        // Pobierz cel
        Transform cel = punktySciezki[aktualnyIndeks];

        // Przesuń obiekt w stronę celu
        transform.position = Vector3.MoveTowards(transform.position, cel.position, predkosc * Time.deltaTime);

        // Opcjonalnie: Obróć obiekt w stronę, w którą idzie
        transform.LookAt(cel);

        // Sprawdź czy dotarliśmy do celu
        if (Vector3.Distance(transform.position, cel.position) < tolerancjaOdleglosci)
        {
            ZmienCel();
        }
    }

    void ZmienCel()
    {
        if (idzieDoPrzodu)
        {
            aktualnyIndeks++; // Idź do następnego

            // Jeśli wyszliśmy poza listę (dotarliśmy do końca)
            if (aktualnyIndeks >= punktySciezki.Count)
            {
                idzieDoPrzodu = false; // Zmień kierunek na wsteczny
                aktualnyIndeks = punktySciezki.Count - 2; // Cofnij się do przedostatniego punktu
            }
        }
        else
        {
            aktualnyIndeks--; // Cofaj się

            // Jeśli indeks spadł poniżej zera (dotarliśmy do początku)
            if (aktualnyIndeks < 0)
            {
                idzieDoPrzodu = true; // Zmień kierunek na do przodu
                aktualnyIndeks = 1; // Idź do drugiego punktu
            }
        }
    }

    // Dodatek: Rysowanie linii w edytorze, żebyś widział ścieżkę (Gizmos)
    void OnDrawGizmos()
    {
        if (punktySciezki == null || punktySciezki.Count < 2) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < punktySciezki.Count - 1; i++)
        {
            if (punktySciezki[i] != null && punktySciezki[i + 1] != null)
            {
                Gizmos.DrawLine(punktySciezki[i].position, punktySciezki[i + 1].position);
                Gizmos.DrawSphere(punktySciezki[i].position, 0.2f);
            }
        }
        // Rysuj ostatni punkt
        if (punktySciezki[punktySciezki.Count - 1] != null)
            Gizmos.DrawSphere(punktySciezki[punktySciezki.Count - 1].position, 0.2f);
    }
}