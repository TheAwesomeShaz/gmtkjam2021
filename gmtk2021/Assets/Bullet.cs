using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isDeflected;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifetime = 10f;
    Transform player;
    Transform enemy;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerHover>().transform;
        enemy = FindObjectOfType<Enemy>().transform;
        StartCoroutine(DestroyBulletAfterTime(bulletLifetime));
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDeflected)
        {
            MoveForward();
        }
        if (isDeflected)
        {
            MoveTowardsEnemy();
        }
    }

    public void MarkEnemy(Transform target)
    {
        enemy = target;
    }

    void MoveForward()
    {
        transform.position += enemy.forward * bulletSpeed * Time.deltaTime;
        // transform.position = Vector3.MoveTowards(transform.position, enemy.GetComponent<Enemy>().bulletPos.forward, bulletSpeed * Time.deltaTime);

    }

    public void MoveTowardsEnemy()
    {
        if (isDeflected && enemy.GetComponent<Enemy>().isAlive)
        {
            transform.position = Vector3.Lerp(transform.position, enemy.position, bulletSpeed * Time.deltaTime);

        }
    }

    IEnumerator DestroyBulletAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyBullet();
    }

    public void DestroyBullet()
    {
        //make destroyed particle effect
        Destroy(gameObject);
    }


}
