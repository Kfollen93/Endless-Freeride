using UnityEngine;

public class SnowboardParticles : MonoBehaviour
{
    public ParticleSystem snowTrailPS; // Accessed in Player script
    [SerializeField] private GameObject player;
    private Player playerScript;
    private bool currentlyPlayingParticles;

    private void Start()
    {
        playerScript = player.GetComponent<Player>();
    }

    private void Update()
    {
        if (!currentlyPlayingParticles && playerScript.Grounded)
        {
            currentlyPlayingParticles = true;
            snowTrailPS.Play();
        }
        else if (currentlyPlayingParticles && !playerScript.Grounded)
        {
            currentlyPlayingParticles = false;
            snowTrailPS.Stop();
        }
    }
}
