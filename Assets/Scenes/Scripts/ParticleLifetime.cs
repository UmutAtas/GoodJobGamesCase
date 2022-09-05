using System.Collections;
using UnityEngine;

public class ParticleLifetime : MonoBehaviour
{
    private float lifetime = 1f;

    private void Start()
    {
        StartCoroutine(PlayParticleRoutine());
    }

    private IEnumerator PlayParticleRoutine()
    {
        while (lifetime > 0)
        {
            lifetime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
