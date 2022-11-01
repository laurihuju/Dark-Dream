using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] GameObject winNote;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        winNote.SetActive(true);
    }


}
