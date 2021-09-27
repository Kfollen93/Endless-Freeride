using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    /* I've moved this into FixedUpdate() since it's tracking the Player which is tracking the Sphere, which moves in FixedUpdate
    * To match this, I've moved the Player following the Sphere, also into FixedUpdate. Now all tracking happens in FixedUpdate*/
    private void FixedUpdate()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        else
        {
            transform.position = player.transform.position;
        }
    }
}
