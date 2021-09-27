using System.Collections;
using UnityEngine;

public class DisableGround : MonoBehaviour
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
            StartCoroutine(DelayDespawn());
        }
    }

    private IEnumerator DelayDespawn()
    {
        yield return new WaitForSeconds(2f);
        transform.parent.gameObject.SetActive(false);
    }
}
