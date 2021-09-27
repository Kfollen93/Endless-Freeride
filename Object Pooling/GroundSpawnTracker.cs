using UnityEngine;

public class GroundSpawnTracker : MonoBehaviour
{
    private ObjectPooler objPooler;
    public GameObject spawnTrigger;
    [HideInInspector] public bool spawnGroundNow = false;
    private string[] randomGroundArray;
    private string spawnRandomGround;
    private string previousGround = "Ground"; // The starting ground is "Ground" so this becomes the previousGround from the start.

    private void Start()
    {
        randomGroundArray = new string[] { "Ground", "Ground2", "Ground3" };
        objPooler = ObjectPooler.Instance;
    }

    private void Update()
    {
        // Switching with triggers when to activate next ground
        if (spawnGroundNow)
        {
            spawnRandomGround = randomGroundArray[Random.Range(0, randomGroundArray.Length)];

            // This check is too reduce the chance of having subsequent ground spawns be the same as the previous spawned ground.
            // It is intended to only lower the chance, NOT completely remove the chance.
            if (previousGround == spawnRandomGround)
            {
                spawnRandomGround = randomGroundArray[Random.Range(0, randomGroundArray.Length)];
                objPooler.SpawnFromPool(spawnRandomGround);
            }
            else
            {
                objPooler.SpawnFromPool(spawnRandomGround);
            }

            previousGround = spawnRandomGround;
            spawnGroundNow = false;
        }
    }
}