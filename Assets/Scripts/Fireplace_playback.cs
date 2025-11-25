using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace_playback : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter fireplaceEmitter;

    private void OnTriggerStay(Collider other)
    {
        fireplaceEmitter.SetParameter("ogien", 1);
    }

    private void OnTriggerExit(Collider other)
    {
        fireplaceEmitter.SetParameter("ogien", 0);
    }
}
