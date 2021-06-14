using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;



public class GameController : MonoBehaviour
{
    // VolumeProfile profile;

    int currentSceneIndex;

    [SerializeField] GameObject aboutButton;
    [SerializeField] GameObject aboutMenu;

    public GameObject endLevelScreen;
    [SerializeField] GameObject pauseMenu;
    bool isPaused;

    public AudioClip slowMoOn;
    public AudioClip slowMoOff;

    public AudioSource sfx;


    CamFollow cam;
    HoverBoard player;
    public bool isDetected;
    public float musicVolume = 0.7f;
    bool slomosfxplayed;

    public float decreasePitch = 0.7f;

    [SerializeField] AudioSource ambientMusic;
    [SerializeField] AudioSource drumsMusic;

    [SerializeField] float slowdownFactor = 0.05f;
    // [SerializeField] AudioSource gameMusic;
    [SerializeField] float slowdownLength;
    [SerializeField] AudioClip deflectSFX;
    [SerializeField] AudioClip teleportSFX;

    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<CamFollow>();
        player = FindObjectOfType<HoverBoard>();

        if (pauseMenu)
        {
            pauseMenu.SetActive(false);
        }

        ambientMusic.volume = 0.7f;
        drumsMusic.volume = 0f;

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex != 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1;
        }

        if (currentSceneIndex == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        endLevelScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDetected)
        {
            IncreaseDrumsOverTime();
            drumsMusic.volume += (1f / slowdownLength) * Time.unscaledDeltaTime;
        }
        else if (!isDetected)
        {
            DecreaseDrumsOverTime();
        }

        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     isPaused = !isPaused;
        //     if (isPaused)
        //     {
        //         Pause();
        //     }
        //     else
        //     {
        //         Resume();
        //     }
        // }

        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }




        if (!isPaused)
        {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        }

        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

        drumsMusic.pitch += (1f / slowdownLength) * Time.unscaledDeltaTime;
        ambientMusic.pitch += (1f / slowdownLength) * Time.unscaledDeltaTime;
        ambientMusic.volume += (1f / slowdownLength) * Time.unscaledDeltaTime;

        drumsMusic.pitch = Mathf.Clamp(drumsMusic.pitch, 0f, 1f);
        drumsMusic.volume = Mathf.Clamp(drumsMusic.volume, 0f, musicVolume);


        ambientMusic.pitch = Mathf.Clamp(ambientMusic.pitch, 0f, 1f);
        ambientMusic.volume = Mathf.Clamp(ambientMusic.volume, 0f, musicVolume);

        Time.fixedDeltaTime += (0.01f / slowdownLength) * Time.unscaledDeltaTime;
        Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f);

        // Debug.Log(Time.timeScale);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        Controls();

    }

    private void IncreaseDrumsOverTime()
    {
        if (drumsMusic.volume < 0.7)
        {
            drumsMusic.volume += 0.5f * Time.deltaTime;
        }
    }

    private void DecreaseDrumsOverTime()
    {
        if (drumsMusic.volume > 0)
        {
            drumsMusic.volume -= 0.5f * Time.deltaTime;
        }
    }

    private void Controls()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadLevel();
        }


        // if (Input.GetKey(KeyCode.Escape))
        // {
        //     Cursor.lockState = CursorLockMode.None;
        // }

        if (Input.GetMouseButton(0) && currentSceneIndex != 0)
        {

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButton(1))
        {
            SlowDownTime();
        }
        if (Input.GetMouseButtonDown(1) && !slomosfxplayed)
        {
            PlaySlomoSFX(1);
            slomosfxplayed = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            PlaySlomoSFX(0);
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

    void SlowDownTime()
    {
        // PlaySlomoSFX(1);
        Time.timeScale = slowdownFactor;
        ambientMusic.pitch = decreasePitch;
        drumsMusic.pitch = decreasePitch;
        // gameMusic.pitch = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        // ChromaticAbberationEffect(1.5f);
        // ColorAdjustmentEffect(100f);

    }

    private static void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        StartCoroutine(ReloadLevelAfterTime());
    }

    IEnumerator ReloadLevelAfterTime()
    {
        yield return new WaitForSeconds(4f);
        ReloadLevel();
    }

    public void PlaySlomoSFX(int i)
    {
        if (i == 1)
        {
            sfx.PlayOneShot(slowMoOn, 1.5f);
        }
        if (i == 0)
        {
            sfx.PlayOneShot(slowMoOff, 1.5f);
            slomosfxplayed = false;
        }

    }

    public void EndLevel()
    {
        endLevelScreen.SetActive(true);
        StartCoroutine(LoadMenuAfterTime());
    }

    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator LoadMenuAfterTime()
    {
        yield return new WaitForSeconds(5f);
        LoadMainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        SlowDownTime();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void PlayDeflectSFX()
    {
        sfx.PlayOneShot(deflectSFX, 1.5f);
    }

    public void PlayTeleportSFX()
    {
        sfx.PlayOneShot(teleportSFX, 1.5f);
    }

    public void About()
    {
        aboutButton.SetActive(false);
        aboutMenu.SetActive(true);
    }

    public void Back()
    {
        aboutButton.SetActive(true);
        aboutMenu.SetActive(false);
    }

}
