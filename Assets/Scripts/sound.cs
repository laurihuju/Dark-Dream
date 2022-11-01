using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio;       //tarvitaan äänen kanssa 

[System.Serializable] // näkyy inspectorissa
public class sound 
{
    // Ääni 
    public AudioClip clip;

    // Äänen nimi 
    public string name;

    // liukkukytkimet 
    [Range(0f, 1f)]
    public float volume; // voimakkuus ( 0 - 1)
    [Range(0.1f, 3f)]
    public float pitch; // sävelkorkeus (0.1 -3)

    // merkkilippu joka kertoo soitetaanko ääni loopissa 
    public bool loop;

    // Äänen lähde, joka piilotetaan 
    [HideInInspector]
    public AudioSource source;

}
