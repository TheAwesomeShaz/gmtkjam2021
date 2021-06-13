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

    private void OnTriggerStay(Collider other)
    {
        // if (other.GetComponent<HoverBoard>() && other.GetComponent<HoverBoard>().isAlive)
        if (other.GetComponent<PlayerHover>())
        {
            playerInRange = true;
            target = other.transform;
        }
    }
    IEnumerator WaitAndShoot(float time)
    {
        yield return new WaitForSeconds(time);
        anim.SetTrigger("Shoot");
    }

    private void Shoot()
    {
        Bullet thisBullet = Instantiate(bullet, bulletPos.position, Quaternion.identity);
        thisBullet.transform.forward = (target.transform.position - this.transform.position).normalized;

    }

    public void Die()
    {
        gameController.isDetected = false;
        anim.enabled = false;
        isAlive = false;
    }
}
