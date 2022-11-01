using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCheck : MonoBehaviour
{
    [SerializeField] GameObject doorsOpened;
    [SerializeField] GameObject doorsClosed;

    [SerializeField] private string dialogBlockToActivate;

    private bool activated;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.CompareTag("Box") && !activated)
        {
            doorsClosed.SetActive(false);
            doorsOpened.SetActive(true);

            activated = true;

            if (dialogBlockToActivate != "")
                DialogManager.instance.GetFlowchart().ExecuteBlock(dialogBlockToActivate);
        }

    }
}
