using UnityEngine;

/// <summary>
/// Luokka, joka sis‰lt‰‰ mapin tiedot.
/// </summary>
[System.Serializable]
public class Map
{
    [SerializeField] private string name; //Mapin nimi
    [SerializeField] private GameObject mapObject; //Mapin GameObject

    /// <summary>
    /// Palauttaa mapin nimen.
    /// </summary>
    /// <returns></returns>
    public string GetName()
    {
        return name;
    }

    /// <summary>
    /// Palauttaa mapin GameObjectin.
    /// </summary>
    /// <returns></returns>
    public GameObject GetMapObject()
    {
        return mapObject;
    }
}
