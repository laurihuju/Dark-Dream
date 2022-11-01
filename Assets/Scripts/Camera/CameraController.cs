using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Kameran kohteen sijainti ja mappi, jonka rajat laitetaan kameran rajoiksi pelin alussa.
    [SerializeField] private Transform targetPosition;

    //Kameran liikkumisen tasaisuus
    [SerializeField] private float smoothness;

    //Kameran rajat
    private float minX = 0;
    private float maxX = 15;
    private float minY = 0;
    private float maxY = 15;

    /// <summary>
    /// Asetetaan kameran aloitusmappi.
    /// </summary>
    private void Start()
    {
        SetBounds(GameManager.instance.GetCurrentMap());
    }

    /// <summary>
    /// Liikuttaa kameraa tasaisesti niin, että kamera seuraa pelaajaa, mutta pysyy mapin rajojen sisällä.
    /// </summary>
    private void Update()
    {
        Vector3 newPosition = new Vector3(Mathf.Clamp(Mathf.Lerp(transform.position.x, Mathf.Clamp(targetPosition.position.x, minX, maxX), smoothness * Time.unscaledDeltaTime), minX, maxX), Mathf.Clamp(Mathf.Lerp(transform.position.y, Mathf.Clamp(targetPosition.position.y, minY, maxY), smoothness * Time.unscaledDeltaTime), minY, maxY), -10);
        transform.position = newPosition;
    }

    /// <summary>
    /// Asettaa kameran rajat parametriksi annetun Tiled-mapin rajoiksi.
    /// </summary>
    /// <param name="map"></param>
    public void SetBounds(string mapName)
    {
        GameObject map = GameManager.instance.GetMapByName(mapName);
        if (map == null)
            return;

        SuperTiled2Unity.SuperMap config = map.GetComponent<SuperTiled2Unity.SuperMap>();

        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = Camera.main.aspect * cameraHeight;

        minX = map.transform.position.x + cameraWidth;
        maxX = map.transform.position.x + config.m_Width - cameraWidth;

        minY = map.transform.position.y - config.m_Height + cameraHeight;
        maxY = map.transform.position.y - cameraHeight;
    }
}
