using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deflection : MonoBehaviour
{
    public Transform Camboi;
    public PlayerHover mainChar;
    public List<GameObject> myBullets = new List<GameObject>();
    bool sfxplayed;

    GameController gameController;

    private void Start()
    {
        Camboi = FindObjectOfType<Camera>().transform;
        gameController = FindObjectOfType<GameController>();
    }
    private void Update()
    {

        foreach (GameObject bullet in myBullets)
        {
            // Debug.DrawLine(this.transform.position, bullet.transform.position, Color.red);
            mainChar.canDeflect = true;
            Bullet myOwnBullet = bullet.GetComponent<Bullet>();
            if (Input.GetMouseButton(0))
            {
                mainChar.DeflectAnim();
                if (!sfxplayed)
                {
                    gameController.PlayDeflectSFX();
                    sfxplayed = true;
                }

                if (!myOwnBullet.isDeflected)
                {
                    myOwnBullet.transform.forward *= -1;
                    myOwnBullet.isDeflected = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            sfxplayed = false;

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            myBullets.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            myBullets.Remove(other.gameObject);
        }
    }



}