using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    [SerializeField] bool followY;
    public Vector3 offset = new Vector3(0f, 0f, 0f);

    // Update is called once per frame
    void Update()
    {
        if (followY)
        {

            transform.position = new Vector3(target.position.x, target.position.y, target.position.z);
        }
        else
        {
            transform.position = new Vector3(target.position.x, transform.position.y, target.position.z);
        }
    }
}
