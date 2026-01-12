using UnityEngine;
using FMODUnity; // FMOD

public class Kwiatek : MonoBehaviour
{
    [Header("Audio")]
    public EventReference dzwiekZebrania;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // FMOD: Odegraj dŸwiêk w miejscu kwiatka
            if (!dzwiekZebrania.IsNull)
            {
                RuntimeManager.PlayOneShot(dzwiekZebrania, transform.position);
            }

            // Powiadom managera
            if (ArenaSzefa.instancja != null)
            {
                ArenaSzefa.instancja.ZebralemKwiatka();
            }

            Destroy(gameObject);
        }
    }
}