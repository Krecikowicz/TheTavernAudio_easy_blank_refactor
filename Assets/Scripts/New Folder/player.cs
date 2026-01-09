using UnityEngine;
using UnityEngine.UI;

public class player : MonoBehaviour

{
    private FMOD.Studio.VCA vca;
    private Slider slider;

    [SerializeField] float vcaVolume;

    void Start()
    {
        vca = FMODUnity.RuntimeManager.GetVCA("vca:/Player");
        vca.getVolume(out vcaVolume);
    }

    public void SetVolume(float volume)

    {
        vca.setVolume(volume);
    }
}