using UnityEngine;
using FMODUnity; // Wa¿ne dla FMOD
using FMOD.Studio; // Wa¿ne dla EventInstance i STOP_MODE

public class ArenaSzefa : MonoBehaviour
{
    public static ArenaSzefa instancja;

    [Header("Ustawienia Gry")]
    public int potrzebneKwiatki = 5;
    public float coIleSpawnowacKwiatek = 3f;

    [Header("Referencje")]
    public GameObject kwiatekPrefab;
    public GameObject portal;
    public GameObject boss;
    public Collider graniceAreny;

    [Header("Efekty Wizualne")]
    public GameObject efektPortalu;

    [Header("Audio - Muzyka")]
    public EventReference muzykaAreny;
    private EventInstance instancjaMuzyki;

    private int zebraneKwiatki = 0;
    private float licznikSpawnu = 0f;

    void Awake()
    {
        instancja = this;
        if (portal != null) portal.SetActive(false);
        if (efektPortalu != null) efektPortalu.SetActive(false);
    }

    void Start()
    {
        // Uruchom muzykê
        if (!muzykaAreny.IsNull)
        {
            instancjaMuzyki = RuntimeManager.CreateInstance(muzykaAreny);
            instancjaMuzyki.start();
        }
    }

    void Update()
    {
        if (boss != null && boss.activeSelf)
        {
            licznikSpawnu -= Time.deltaTime;
            if (licznikSpawnu <= 0)
            {
                SpawnujKwiatek();
                licznikSpawnu = coIleSpawnowacKwiatek;
            }
        }
    }

    void SpawnujKwiatek()
    {
        Vector3 losowaPozycja = LosowyPunktWArenie();
        losowaPozycja.y = graniceAreny.bounds.max.y + 0.5f;
        Instantiate(kwiatekPrefab, losowaPozycja, Quaternion.identity);
    }

    public void ZebralemKwiatka()
    {
        zebraneKwiatki++;
        Debug.Log("Zebrano: " + zebraneKwiatki);

        if (zebraneKwiatki >= potrzebneKwiatki)
        {
            WygralemWalke();
        }
    }

    void WygralemWalke()
    {
        if (boss != null) Destroy(boss);
        if (portal != null) portal.SetActive(true);

        if (efektPortalu != null) efektPortalu.SetActive(true);

        // POPRAWKA: Pe³na œcie¿ka do STOP_MODE
        if (instancjaMuzyki.isValid())
        {
            instancjaMuzyki.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            instancjaMuzyki.release();
        }

        Debug.Log("BOSS POKONANY!");
    }

    public Vector3 LosowyPunktWArenie()
    {
        Bounds b = graniceAreny.bounds;
        return new Vector3(Random.Range(b.min.x, b.max.x), transform.position.y, Random.Range(b.min.z, b.max.z));
    }

    public Vector3 LosowyPunktNaKrawedzi()
    {
        Bounds b = graniceAreny.bounds;
        int sciana = Random.Range(0, 4);
        float x = 0, z = 0;

        switch (sciana)
        {
            case 0: x = b.min.x; z = Random.Range(b.min.z, b.max.z); break;
            case 1: x = b.max.x; z = Random.Range(b.min.z, b.max.z); break;
            case 2: z = b.min.z; x = Random.Range(b.min.x, b.max.x); break;
            case 3: z = b.max.z; x = Random.Range(b.min.x, b.max.x); break;
        }
        return new Vector3(x, transform.position.y, z);
    }

    public Vector3 OgraniczDoAreny(Vector3 punkt)
    {
        return graniceAreny.ClosestPoint(punkt);
    }

    // POPRAWKA: To musi byæ osobna funkcja, nie wklejona w inn¹
    void OnDestroy()
    {
        if (instancjaMuzyki.isValid())
        {
            // Pe³na œcie¿ka do STOP_MODE
            instancjaMuzyki.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            instancjaMuzyki.release();
        }
    }

} // Koniec klasy