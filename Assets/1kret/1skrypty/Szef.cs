using UnityEngine;
using System.Collections;
using FMODUnity; // WA¯NE: To pozwala korzystaæ z FMOD w kodzie

public class Szef : MonoBehaviour
{
    [Header("Statystyki Ruchu")]
    public float predkoscChodzenia = 3f;
    public float predkoscDasha = 25f;
    public float czasOstrzegania = 1.0f;
    public float czasMiedzyDashami = 3f;

    [Header("DŸwiêki FMOD")]
    // EventReference pozwala wybraæ dŸwiêk z listy w Inspektorze (z lupk¹)
    public EventReference dzwiekOstrzezenia;
    public EventReference dzwiekDasha;

    [Header("Komponenty")]
    public LineRenderer liniaCelowania;

    private Transform gracz;
    private Vector3 celDasha;
    private bool czyDashuje = false;

    void Start()
    {
        if (liniaCelowania == null) liniaCelowania = GetComponent<LineRenderer>();
        liniaCelowania.positionCount = 2;
        liniaCelowania.enabled = false;

        GameObject graczObj = GameObject.FindGameObjectWithTag("Player");
        if (graczObj != null) gracz = graczObj.transform;

        StartCoroutine(CyklZyciaBossa());
    }

    IEnumerator CyklZyciaBossa()
    {
        while (true)
        {
            // --- 1. FAZA CHODZENIA ---
            float czasDoAtaku = czasMiedzyDashami;
            Vector3 losowyPunktRuchu = ArenaSzefa.instancja.LosowyPunktWArenie();
            losowyPunktRuchu.y = transform.position.y;
            transform.LookAt(losowyPunktRuchu);

            while (czasDoAtaku > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, losowyPunktRuchu, predkoscChodzenia * Time.deltaTime);

                if (Vector3.Distance(transform.position, losowyPunktRuchu) < 0.5f)
                {
                    losowyPunktRuchu = ArenaSzefa.instancja.LosowyPunktWArenie();
                    losowyPunktRuchu.y = transform.position.y;
                    transform.LookAt(losowyPunktRuchu);
                }

                czasDoAtaku -= Time.deltaTime;
                yield return null;
            }

            // --- 2. FAZA CELOWANIA ---
            if (gracz != null)
            {
                Vector3 kierunekDoGracza = (gracz.position - transform.position).normalized;
                kierunekDoGracza.y = 0;
                Vector3 teoretycznyCel = transform.position + (kierunekDoGracza * 50f);
                celDasha = ArenaSzefa.instancja.OgraniczDoAreny(teoretycznyCel);
                celDasha.y = transform.position.y;

                transform.LookAt(celDasha);

                liniaCelowania.enabled = true;
                liniaCelowania.SetPosition(0, transform.position);
                liniaCelowania.SetPosition(1, celDasha);

                // --- FMOD: Zagraj dŸwiêk ³adowania (Ostrze¿enie) ---
                if (!dzwiekOstrzezenia.IsNull)
                {
                    // PlayOneShotAttached gra dŸwiêk "przyklejony" do bossa (3D sound)
                    RuntimeManager.PlayOneShotAttached(dzwiekOstrzezenia, gameObject);
                }
            }

            yield return new WaitForSeconds(czasOstrzegania);
            liniaCelowania.enabled = false;

            // --- 3. FAZA DASHA ---
            czyDashuje = true;

            // --- FMOD: Zagraj dŸwiêk ataku (Dash) ---
            if (!dzwiekDasha.IsNull)
            {
                RuntimeManager.PlayOneShotAttached(dzwiekDasha, gameObject);
            }

            float limitCzasu = 3f;

            while (Vector3.Distance(transform.position, celDasha) > 0.5f && limitCzasu > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, celDasha, predkoscDasha * Time.deltaTime);
                limitCzasu -= Time.deltaTime;
                yield return null;
            }
            czyDashuje = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && czyDashuje)
        {
            Debug.Log("TRAFIONY!");
        }
    }
}