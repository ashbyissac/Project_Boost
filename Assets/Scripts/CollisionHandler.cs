using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] AudioClip crashSFX;
    [SerializeField] AudioClip successSFX;

    [SerializeField] ParticleSystem successParticlesFX;
    [SerializeField] ParticleSystem crashParticlesFX;
    
    PlayerMovement playerMovementScript;
    AudioSource audioSource;

    bool isTransitioning = false;
    bool isCollisionDisabled;

    void Start()
    {
        playerMovementScript = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //ProcessDebugging();    /// todo :: remove 
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || isCollisionDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Start":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    private void ProcessDebugging()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            isCollisionDisabled = !isCollisionDisabled;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        playerMovementScript.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(successSFX);
        successParticlesFX.Play();

        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        playerMovementScript.enabled = false;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSFX);
        crashParticlesFX.Play();

        Invoke("ReloadLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int scenesCount = SceneManager.sceneCountInBuildSettings;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == scenesCount)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
