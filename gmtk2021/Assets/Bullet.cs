using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isDeflected;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifetime = 10f;
    [SerializeField] Vector3 bulletOffset = new Vector3(0f, 2f, 0f);

    Transform player;
    [SerializeField] Transform enemy;
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
        this.enemy = target;
    }

    void MoveForward()
    {

        transform.position = Vector3.Lerp(transform.position, player.position, bulletSpeed * Time.deltaTime);

        // transform.position = Vector3.MoveTowards(transform.position, enemy.GetComponent<Enemy>().bulletPos.forward, bulletSpeed * Time.deltaTime);

    }

    public void MoveTowardsEnemy()
    {
        if (isDeflected && enemy.GetComponent<Enemy>().isAlive)
        {
            // transform.position += enemy.position * bulletSpeed * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, enemy.position + bulletOffset, bulletSpeed * Time.deltaTime);

        }
        if (transform.position == enemy.position)
        {
            Destroy(gameObject);
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
