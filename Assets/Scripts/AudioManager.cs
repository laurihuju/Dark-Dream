using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Audio; //‰‰net varten

public class AudioManager : MonoBehaviour
{
    //ƒ‰nil‰hteet sis‰lt‰v‰ oliotaulukko
    public sound[] sounds;

    //Vain yksi esiintym‰ (sinkelton) 
    public static AudioManager instance;

    [Header("Nykyinen musiikki")]
    /// <summary>
    /// Nykyinen musiiki, joka on p‰‰ll‰
    /// </summary>
   public string currentMusic;



    void Awake()
    {
       
        // onko AudioMaMAnagari olemassa ?
        if (instance == null)
            // audiomanage ei ole olemassa; joten luodaan se
            instance = this;
        else
        {
            // AudioMAnager on jom olemassa, joten tuhotaan se 
            Destroy(gameObject);

            // varmistetaan ett‰ muuta koodia ei en‰‰ suoriteta 
            return;
        }

        // ƒl‰ tuohoa GameObjektia ladattaessa 
        DontDestroyOnLoad(gameObject);

        // n‰ytt‰‰ oliotaulukon kaikki ‰‰nil‰hteet
        foreach (sound s in sounds)
        {
            // Yhetys ‰‰nil‰‰hteeseen 
            s.source = gameObject.AddComponent<AudioSource>();

            // ‰‰ni joka halutaan soitta 
            s.source.clip = s.clip;

            // p‰ivitt‰‰ tehdyt s‰‰dot audio-komponenttiin
            s.source.volume = s.volume; // voimakkuus 
            s.source.pitch = s.pitch;   //
            s.source.loop = s.loop;     // soitetaanko loopissa 
        }

    }
    

    /// <summary>
    /// Soittaa halutun ‰‰nen ja samlla p‰ivitt‰‰ currentMusic -muuttujan.
    /// </summary>
    /// <param name="name"></param>
    public void PlayWithSave(string name)
    {
        // P‰ivitet‰‰n nykyinen musiikki -metodi
        currentMusic = name;


        // Etsit‰‰n haluttu ‰‰ni
        sound s = Array.Find(sounds, sound => sound.name == name);
        // Onko ‰‰nt‰ olemassa?
        if (s == null)
            // Ei ole, joten hyp‰t‰‰n metodista pois.
            return;
        // Soitetaan ‰‰ni
        s.source.Play();

        


    }

    public void Play(string name)
    {
       


        // Etsit‰‰n haluttu ‰‰ni
        sound s = Array.Find(sounds, sound => sound.name == name);
        // Onko ‰‰nt‰ olemassa?
        if (s == null)
            // Ei ole, joten hyp‰t‰‰n metodista pois.
            return;
        // Soitetaan ‰‰ni
        s.source.Play();




    }

    /// < summary >
    /// Pys‰ytt‰‰ halutun ‰‰nen
    /// </summary>
    /// <param name="name"></param>
    public void StopPlay(string name)
    {
        // Etsit‰‰n haluttu ‰‰ni
        sound s = Array.Find(sounds, sound => sound.name == name);
        // Onko ‰‰nt‰ olemassa? 
        if (s == null)
            // Ei ole, joten hyp‰t‰‰n metodista pois
            return;
        // pys‰ytet‰‰n ‰‰ni
        s.source.Stop();






    }
    /// < summary >
    /// Pys‰ytt‰‰ vanhan musiikin ja k‰ynnist‰‰ uuden
    /// </summary>
    /// <param name="name"></param>
    public void ChangeMusic(string newMusic)
    {
        /////////////////////////Ei teht‰ mit‰‰n, jos jo k‰ynniss‰ oleva musiikki on sama kuin uusi musiikki///////////////////
        if (currentMusic == newMusic)
            return;

        /////////////////////////////////////////////Sammutetaan vahna musiikki////////////////////////////////////////////////
        StopPlay(currentMusic);
        currentMusic = newMusic;

        /////////////////////////////////////////////K‰ynnistet‰‰n uusi musiikki///////////////////////////////////////////////
        Play(newMusic);


    }


    



}