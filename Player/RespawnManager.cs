using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    private Vector3 playerSpawnPos = new Vector3(342f, 313.65f, 145.57f);
    [SerializeField] private GameObject defaultGround;
    [SerializeField] private StartGame startScript;
    [SerializeField] private SphereMotorController sphereScript;

    // Public function to connect with RestartButton OnClick() event
    public void RestartButtonClick()
    {
        RespawnGround();
        RepositionPlayerAndSphere();
    }

    private void RepositionPlayerAndSphere()
    {
        sphereScript.rb.velocity = Vector3.zero; // Stop movement forces
        startScript.playerPrefab.transform.SetPositionAndRotation(playerSpawnPos, Quaternion.identity);
        startScript.sphereMotorPrefab.transform.SetPositionAndRotation(playerSpawnPos, Quaternion.identity);
    }

    private void RespawnGround()
    {
        // Turn off any of the active grounds
        foreach (var kvp in ObjectPooler.Instance.poolDictionary)
        {
            Queue<GameObject> grounds = kvp.Value;

            foreach (GameObject ground in grounds)
            {
                ground.SetActive(false);
            }
        }

        // Reset the ground counter
        ObjectPooler.Instance.groundCount = 1;

        // Turn back on the starting ground
        defaultGround.SetActive(true);
    }
}
