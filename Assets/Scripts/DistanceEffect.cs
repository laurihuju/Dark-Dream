using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DistanceEffect : MonoBehaviour
{
    private Volume volume;
    private FilmGrain filmGrain;
    private float intensity;

    [SerializeField] Transform player;
    [SerializeField] Transform enemy;

    [SerializeField] float filmGrainEf = 15;
    float distance;

    
    private void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out filmGrain);
    }

    private void Update()
    {
        distance = Vector3.Distance(player.position, enemy.position);

        intensity = 2f - distance / filmGrainEf;

        if (intensity < 0)
        {
            intensity = 0;


        }
        else if (intensity > 1)
        {

            intensity = 1;



        }

        filmGrain.intensity.value = intensity;

    }



}
