using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform birdPos;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] bool foundTeleportCrystal;

    HoverBoard hoverBoard;

    // Start is called before the first frame update
    void Start()
    {
        birdPos = target;
        hoverBoard = FindObjectOfType<HoverBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            target = birdPos;
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        transform.LookAt(target);

        if (transform.position == target.position && foundTeleportCrystal)
        {
            hoverBoard.canTeleport = true;
            hoverBoard.teleportTarget = target;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<TeleportCrystal>())
        {
            target = other.transform;
            foundTeleportCrystal = true;
        }
    }

    public void ResetTarget()
    {
        target = birdPos;
    }

    public void ResetTransform()
    {
        transform.position = birdPos.position;
        foundTeleportCrystal = false;

    }

}
