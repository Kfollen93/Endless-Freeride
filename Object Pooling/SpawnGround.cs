using UnityEngine;

public class SpawnGround : MonoBehaviour
{
    private GroundSpawnTracker groundTracker;

    private void OnEnable()
    {
        groundTracker = GameObject.FindWithTag("Pooler").GetComponent<GroundSpawnTracker>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // using motor since it needs to have a RB to activate
        if (other.CompareTag("Motor")) 
        {
            groundTracker.spawnGroundNow = true;
        }
    }
}