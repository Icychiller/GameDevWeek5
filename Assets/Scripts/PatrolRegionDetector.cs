using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRegionDetector : MonoBehaviour
{
    // Start is called before the first frame update
    public EnemyController enemyController;

    private bool playerDetected = false;

    // Update is called once per frame
    void Update()
    {
        // If no player has been detected in THIS frame, change player detection to false
        if(!playerDetected)
        {
            enemyController.detectPlayer = false;
        }
        playerDetected = false;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            enemyController.detectPlayer = true;
            playerDetected = true;
        }
        
    }
}
