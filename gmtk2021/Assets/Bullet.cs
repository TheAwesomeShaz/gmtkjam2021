using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isDeflected;
    public float bulletSpeed;
    public float bulletLifetime = 10f;
    public Vector3 bulletOffset = new Vector3(0f, 2f, 0f);
    private Vector3 oldPosition = Vector3.zero;
    public Transform Playerboi;
    public Transform Camboi;

    public Material[] matarr;


    // Start is called before the first frame update
    void Start()
    {
        Playerboi = FindObjectOfType<PlayerHover>().transform;
        Camboi = FindObjectOfType<Camera>().transform;
        Destroy(gameObject, bulletLifetime);
    }
    private void Update()
    {
        this.transform.position += this.transform.forward * Time.deltaTime * bulletSpeed;
        if (isDeflected)
        {
            // this.gameObject.GetComponent<MeshRenderer>().material = matarr[1];
            // this.gameObject.GetComponent<TrailRenderer>().material = matarr[1];
        }
        else
        {
            // this.gameObject.GetComponent<MeshRenderer>().material = matarr[0];
            // this.gameObject.GetComponent<TrailRenderer>().material = matarr[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && isDeflected)
        {
            other.GetComponent<Enemy>().Die();
            Destroy(this.gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<HoverBoard>().KillPlayer();
            other.GetComponent<PlayerHover>().Die();
            Debug.Log("akjsdbakjsdnbaksjdb");
            Destroy(this.gameObject);
        }
    }
}