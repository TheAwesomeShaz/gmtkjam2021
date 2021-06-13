using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    public PlayerHover mainChar;
    public List<GameObject> myBullets = new List<GameObject>();

    private void Update()
    {
        foreach (GameObject bullet in myBullets)
        {
            mainChar.canDeflect = true;
            Bullet myOwnBullet = bullet.GetComponent<Bullet>();
            if (Input.GetMouseButtonDown(0))
            {
                myOwnBullet.isDeflected = true;
                StartCoroutine(MoveTowardsEnemyStart(myOwnBullet));
                mainChar.DeflectAnim();
            }
        }
    }

    // private void OnTriggerStay(Collider other)
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
    //     else { return; }
    // }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Bullet>())
        {
            myBullets.Add(other.gameObject);

            Debug.DrawLine(this.transform.position, other.transform.position);

            // mainChar.canDeflect = true;
            // Bullet bullet = other.gameObject.GetComponent<Bullet>();
            // if (Input.GetMouseButton(0) && mainChar.canDeflect)
            // {
            //     StartCoroutine(MoveTowardsEnemyStart(bullet));
            //     mainChar.DeflectAnim();
            // }

        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Bullet>())
        {
            myBullets.Remove(other.gameObject);
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