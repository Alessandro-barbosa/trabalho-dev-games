using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEffectsPlayer : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx1;

    public void shootAudio()
    {
        src.clip = sfx1;
        src.Play();
    }
}
