using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            SEventSystem.EventIns.PLAYER_HIT_NPC.Invoke();
        }
    }
}
