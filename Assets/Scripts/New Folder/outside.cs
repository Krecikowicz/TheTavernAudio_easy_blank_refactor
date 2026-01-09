using UnityEngine;
using UnityEngine.UI;

public class outside : MonoBehaviour

{
    private FMOD.Studio.VCA vca;
    private Slider slider;

    [SerializeField] float vcaVolume;

    void Start()
    {
        vca = FMODUnity.RuntimeManager.GetVCA("vca:/Outside");
        vca.getVolume(out vcaVolume);
    }

    public void SetVolume(float volume)

    {
        vca.setVolume(volume);
    }
}