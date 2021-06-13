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


    // Start is called before the first frame update
    void Start()
    {
        Playerboi = FindObjectOfType<PlayerHover>().transform;
        Camboi = FindObjectOfType<Camera>().transform;
        Destroy(gameObject, bulletLifetime);
    }
    private void Update() {
        this.transform.position += this.transform.forward*Time.deltaTime*bulletSpeed;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Enemy") && isDeflected){
            other.GetComponent<Enemy>().Die();
            Destroy(this.gameObject);
        }
    }
}
