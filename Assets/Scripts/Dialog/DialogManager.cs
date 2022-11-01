using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;

    [SerializeField] private Flowchart flowchart;

    private List<ushort> activatedDialogs;
    private bool isDialogActive;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        //Jos aktivoitujen dialogien listaa ei ole vielä luotu, luodaan se.
        if(activatedDialogs == null)
            activatedDialogs = new List<ushort>();
    }

    private void Update()
    {
        //Jos flowchartin dialogien aktiivisuus on muuttunut edellisestä Updaten kutsumisesta, asetetaan GameManageriin uusi tila.
        if(flowchart.HasExecutingBlocks() != isDialogActive)
        {
            isDialogActive = flowchart.HasExecutingBlocks();

            GameManager.instance.SetDialogState(isDialogActive);
        }
    }

    /// <summary>
    /// Palauttaa flowchartin.
    /// </summary>
    /// <returns></returns>
    public Flowchart GetFlowchart()
    {
        return flowchart;
    }

    /// <summary>
    /// Listaa parametrina annetun dialogin aktivoiduksi.
    /// </summary>
    /// <param name="dialogActivatorID"></param>
    public void DialogActivated(ushort dialogActivatorID)
    {
        activatedDialogs.Add(dialogActivatorID);
    }

    /// <summary>
    /// Kertoo, onko parametrina annettu dialogi aktivoitu.
    /// </summary>
    /// <param name="dialogActivatorID"></param>
    /// <returns></returns>
    public bool IsDialogActivated(ushort dialogActivatorID)
    {
        return activatedDialogs.Contains(dialogActivatorID);
    }

    /// <summary>
    /// Asettaa aktivoitujen dialogien ID:iden listan arvoiksi parametrina annetun taulukon sisällön.
    /// </summary>
    /// <param name="activatedDialogs"></param>
    public void SetActivatedDialogs(ushort[] activatedDialogs)
    {
        this.activatedDialogs = new List<ushort>(activatedDialogs);
    }

    /// <summary>
    /// Palauttaa taulukon aktivoitujen dialogien ID:istä.
    /// </summary>
    /// <returns></returns>
    public ushort[] GetActivatedDialogs()
    {
        return activatedDialogs.ToArray();
    }

    public bool IsDialogActive()
    {
        return isDialogActive;
    }
}
