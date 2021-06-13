using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    public PlayerHover mainChar;

    private void OnTriggerStay(Collider other)
    {

        if (other.GetComponent<Bullet>())
        {
            Debug.DrawLine(this.transform.position, other.transform.position);
            mainChar.canDeflect = true;
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (Input.GetMouseButton(0) && mainChar.canDeflect)
            {
                StartCoroutine(MoveTowardsEnemyStart(bullet));
                mainChar.DeflectAnim();
            }

        }
        else { return; }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.GetComponent<Bullet>())
    //     {
    //         Debug.DrawLine(this.transform.position, other.transform.position);
    //         mainChar.canDeflect = true;
    //         Bullet bullet = other.gameObject.GetComponent<Bullet>();
    //         if (Input.GetMouseButton(0) && mainChar.canDeflect)
    //         {
    //             StartCoroutine(MoveTowardsEnemyStart(bullet));
    //             mainChar.DeflectAnim();
    //         }

    //     }
    // }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Bullet>())
        {
            mainChar.canDeflect = false;

        }

    }

    IEnumerator MoveTowardsEnemyStart(Bullet bullet)
    {
        bullet.MoveTowardsEnemy();
        bullet.isDeflected = true;
        yield return null;
    }
}