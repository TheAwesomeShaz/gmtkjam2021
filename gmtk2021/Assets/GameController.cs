using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    CamFollow cam;
    HoverBoard player;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CamFollow>();
        player = FindObjectOfType<HoverBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }



        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }



        if (Input.GetKey(KeyCode.Q))
        {
            cam.ChangeTargetToBird();
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            cam.ChangeTargetToMCHoverboard();
        }
        if (!player.isAlive)
        {
            cam.ChangeTargetToRig();
        }

    }

    private static void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
