using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHover : MonoBehaviour
{

    Animator anim;
    HoverBoard hoverBoard;
    public bool canDeflect;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hoverBoard = GetComponentInParent<HoverBoard>();
    }

    public void DeflectAnim()
    {
        anim.SetTrigger("Deflect");
    }

    public void Slash()
    {
        anim.SetTrigger("Slash");
    }

    public void Die()
    {
        anim.enabled = false;

    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.GetComponent<Bullet>())
    //     {
    //         canDeflect = true;
    //         Bullet bullet = other.gameObject.GetComponent<Bullet>();
    //         if (Input.GetMouseButtonDown(0) && canDeflect)
    //         {
    //             bullet.MoveTowardsEnemy();
    //             bullet.isDeflected = true;
    //             canDeflect = false;
    //         }
    //     }
    // }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Bullet>())
        {
            hoverBoard.KillPlayer();
        }
    }


}
