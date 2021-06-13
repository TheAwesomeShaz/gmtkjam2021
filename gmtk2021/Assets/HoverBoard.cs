using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBoard : MonoBehaviour
{
    Rigidbody rb;
    //[SerializeField] AudioSource hoverboardSFX;
    [SerializeField] AudioClip hoverboardClip;
    PlayerHover player;
    [SerializeField] float multiplier;
    [SerializeField] float moveForce, turnTorque;
    [SerializeField] bool shiftPressed;
    public bool isAlive = true;
    Bird bird;

    public bool canTeleport;
    public Transform teleportTarget;

    public Transform[] anchors = new Transform[4];

    RaycastHit[] hits = new RaycastHit[4];

    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        bird = FindObjectOfType<Bird>();
        rb = GetComponent<Rigidbody>();
        player = GetComponentInChildren<PlayerHover>();
        gameController = FindObjectOfType<GameController>();
    }

    void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit, 4f))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            rb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }
    }



    void Update()
    {
        shiftPressed = Input.GetKey(KeyCode.LeftShift);

        if (transform.up.y < 0f)
        {
            KillPlayer();
            gameController.RestartLevel();
        }

        if (Input.GetKeyDown(KeyCode.E) && canTeleport)
        {
            Teleport(teleportTarget);
        }

        //hoverboardSFX.pitch = rb.velocity.z / 5f;
        //Mathf.Clamp(hoverboardSFX.pitch, 1f, 2f);

    }

    public void KillPlayer()
    {
        player.Die();
        isAlive = false;

    }

    public void Teleport(Transform target)
    {
        Vector3 teleportpos = target.position;
        Destroy(target.gameObject);
        // bird.gameObject.SetActive(false);
        transform.position = teleportpos;
        bird.ResetTransform();
        canTeleport = false;
    }

    IEnumerator IncreaseMultiplierAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        multiplier = 2.5f;

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
