using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(ParticleSystem))]
public class ProceduralBloodSplatter : MonoBehaviour
{
    public GameObject[] bloodDecalPrefabs; 
    
    private ParticleSystem partSystem;
    private List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        partSystem = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = partSystem.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            Vector3 normal = collisionEvents[i].normal;

            SpawnDecal(pos, normal);
        }
    }

    void SpawnDecal(Vector3 position, Vector3 normal)
    {

        GameObject decalPrefab = bloodDecalPrefabs[Random.Range(0, bloodDecalPrefabs.Length)];
        
        Quaternion rotation = Quaternion.LookRotation(-normal);
        rotation *= Quaternion.Euler(0, 0, Random.Range(0f, 360f));

        GameObject splat = Instantiate(decalPrefab, position, rotation);

        float randomScale = Random.Range(0.5f, 1.5f);
        splat.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }
}