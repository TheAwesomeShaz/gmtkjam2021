using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamFollow : MonoBehaviour
{
    CinemachineFreeLook cam;
    [SerializeField] Transform MCharacter;
    [SerializeField] Transform PlayerRig;
    [SerializeField] Transform bird;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<CinemachineFreeLook>();
        ChangeTargetToMCHoverboard();
    }

    public void ChangeTargetToRig()
    {
        cam.Follow = PlayerRig;
        cam.LookAt = PlayerRig;
    }

    public void ChangeTargetToBird()
    {
        cam.Follow = bird;
        cam.LookAt = bird;
    }
    public void ChangeTargetToMCHoverboard()
    {
        cam.Follow = MCharacter;
        cam.LookAt = MCharacter;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
