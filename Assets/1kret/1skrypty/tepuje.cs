using UnityEngine;
using FMODUnity; // FMOD

public class tepuje : MonoBehaviour
{
    [Header("Gdzie nas wyrzuciæ?")]
    public Transform celTeleportacji;

    [Header("Opcje")]
    public bool przeniesRotacje = true;

    [Header("Audio")]
    public EventReference dzwiekTeleportu;

    private void OnTriggerEnter(Collider other)
    {
        if (celTeleportacji == null) return;

        if (other.CompareTag("Player"))
        {
            TeleportujObiekt(other.transform);
        }
    }

    void TeleportujObiekt(Transform obiekt)
    {
        // FMOD: DŸwiêk teleportu (2D lub 3D w zale¿noœci od ustawieñ eventu)
        if (!dzwiekTeleportu.IsNull)
        {
            RuntimeManager.PlayOneShot(dzwiekTeleportu, transform.position);
        }

        CharacterController cc = obiekt.GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;
            obiekt.position = celTeleportacji.position;
            if (przeniesRotacje) obiekt.rotation = celTeleportacji.rotation;
            cc.enabled = true;
        }
        else
        {
            obiekt.position = celTeleportacji.position;
            if (przeniesRotacje) obiekt.rotation = celTeleportacji.rotation;
        }
    }
}