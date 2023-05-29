using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    private GameControl gameControl;
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")){
            gameControl.CheckpointTriggered(this, other.transform.root.GetComponent<bikeController>());
        }
    }

    public void SetTrackCheckpoints(GameControl gameControl) {
        this.gameControl = gameControl;
    }
}
