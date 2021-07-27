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

    public float k_p = 1;
    public float k_d = 1;
    public float k_i = 1;
    public float offsetSigma;
    public float SigmaMultiplier;
    public bool isAlive = true;
    Bird bird;

    public bool canTeleport;
    public Transform teleportTarget;

    public Transform[] anchors = new Transform[4];

    public float[] OldForce = new float[]{0,0,0,0};


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

    void ApplyForce(Transform anchor, RaycastHit hit, float oldforce , int Index)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit, 4f))
        {
            Debug.DrawLine(this.transform.position,anchor.position);
            
            float force = 0;
            force = Mathf.Abs(1 / SigmaMultiplier*Mathf.Exp( hit.point.y - anchor.position.y - offsetSigma));
            //PID Controller Implementation
            var proportionalParameter = k_p * multiplier *force;
            var differentialParameter = k_d * (force - oldforce)/ 0.5f*Time.deltaTime;
            var integralParameter = k_i; // um how do i store buffer values?
            var PIDForce = (proportionalParameter + differentialParameter + integralParameter);
            Debug.DrawLine(anchor.position , anchor.position + PIDForce*0.5f *-transform.up, Color.blue); //force bein applied?
            rb.AddForceAtPosition( transform.up * PIDForce, anchor.position, ForceMode.Acceleration);
            OldForce[Index] = force;
        }
    }

    void CalculateCentroid(){

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
            gameController.PlayTeleportSFX();
        }

        //hoverboardSFX.pitch = rb.velocity.z / 5f;
        //Mathf.Clamp(hoverboardSFX.pitch, 1f, 2f);

    }

    public void KillPlayer()
    {
        player.Die();
        isAlive = false;
        gameController.RestartLevel();
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
            ApplyForce(anchors[i], hits[i] , OldForce[i] , i);
        }


        rb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * Time.deltaTime * transform.up);
        if (shiftPressed)
        {
            rb.AddForce(Input.GetAxis("Vertical") * moveForce * 3f * transform.forward);
        }

        else if (!shiftPressed)
        {
            rb.AddForce(Input.GetAxis("Vertical") * moveForce * 2f *transform.forward);
        }
    }
}
