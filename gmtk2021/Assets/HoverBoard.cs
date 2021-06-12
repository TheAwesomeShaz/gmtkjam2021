using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBoard : MonoBehaviour
{
    Rigidbody rb;
    PlayerHover player;
    [SerializeField] float multiplier;
    [SerializeField] float moveForce, turnTorque;
    [SerializeField] bool shiftPressed;


    public Transform[] anchors = new Transform[4];

    RaycastHit[] hits = new RaycastHit[4];

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GetComponentInChildren<PlayerHover>();
    }

    void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            rb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }
    }



    void Update()
    {
        shiftPressed = Input.GetKey(KeyCode.LeftShift);

        // if (transform.rotation.eulerAngles.x >= 90 || transform.rotation.eulerAngles.x <= -90 || transform.rotation.eulerAngles.z >= 90 || transform.rotation.eulerAngles.z <= -90)
        // {
        //     player.Die();
        // }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
        {
            ApplyForce(anchors[i], hits[i]);
        }


        rb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * transform.up);
        if (shiftPressed)
        {
            rb.AddForce(Input.GetAxis("Vertical") * moveForce * 2 * transform.forward);
        }

        else if (!shiftPressed)
        {
            rb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
        }
    }
}
