using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{

    PlayerHover player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerHover>())
        {
            other.GetComponent<PlayerHover>().Die();
        }
    }


}
