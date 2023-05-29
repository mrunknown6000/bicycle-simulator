using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) {
        if (collider.CompareTag("Player")) {
            bikeController bike = collider.transform.root.GetComponent<bikeController>();
            bike.AddReward(-5f);
            bike.EndEpisode();
        }
    }
}
