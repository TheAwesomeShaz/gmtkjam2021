using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHover : MonoBehaviour
{

    Animator anim;
    HoverBoard hoverBoard;
    [SerializeField] bool canDeflect;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hoverBoard = GetComponentInParent<HoverBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        Controls();
    }

    private void Controls()
    {

        if (Input.GetMouseButtonDown(0) && !canDeflect)
        {
            anim.SetTrigger("Slash");
        }
        else if (Input.GetMouseButtonDown(0) && canDeflect)
        {
            anim.SetTrigger("Deflect");
        }
        if (Input.GetMouseButton(1))
        {
            canDeflect = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            canDeflect = false;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Die();
        }
    }

    public void Die()
    {
        anim.enabled = false;
    }


}
