using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameController gameController;
    public Transform target;
    [SerializeField]
    Animator anim;
    public bool playerInRange;
    [SerializeField] float timeBtwnShots = 3f;

    [SerializeField] Bullet bullet;
    public Transform bulletPos;

    public bool isAlive;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isAlive = true;
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && isAlive)
        {
            gameController.isDetected = true;
            transform.LookAt(target);
            StartCoroutine(WaitAndShoot(timeBtwnShots));
        }



    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Bullet>() && other.gameObject.GetComponent<Bullet>().isDeflected && isAlive)
        {
            Destroy(other.gameObject);
            Die();
        }

        if (other.gameObject.GetComponent<Deflection>())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Die();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // if (other.GetComponent<HoverBoard>() && other.GetComponent<HoverBoard>().isAlive)
        if (other.GetComponent<HoverBoard>())
        {
            playerInRange = true;
            target = other.transform;
        }
        if (other.gameObject.GetComponent<Bullet>() && other.gameObject.GetComponent<Bullet>().isDeflected && isAlive)
        {
            Destroy(other.gameObject);
            Die();
        }


    }

    IEnumerator WaitAndShoot(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("Shoot");
        //shoot function will be called via an animation event
    }

    private void Shoot()
    {
        Bullet thisBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        bullet.MarkEnemy(transform);

    }

    public void Die()
    {
        gameController.isDetected = false;
        anim.enabled = false;
        isAlive = false;
    }
}
