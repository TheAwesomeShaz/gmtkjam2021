using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHover : MonoBehaviour
{

    Animator anim;
    [SerializeField] bool canDeflect;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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

    private void Die()
    {
        anim.enabled = !anim.enabled;
    }
}
