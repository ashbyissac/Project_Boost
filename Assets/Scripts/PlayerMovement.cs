using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float thrustForce = 1000f;
    [SerializeField] float rotationSpeed = 100f;

    [SerializeField] AudioClip thrustSFX;

    [SerializeField] ParticleSystem mainThrustParticles;
    [SerializeField] ParticleSystem leftThrustParticles;
    [SerializeField] ParticleSystem rightThrustParticles;

    Rigidbody rb;
    AudioSource audioSource;
    BoxCollider boxCollider;
    CapsuleCollider capsuleCollider;

    bool isPlaying = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessDebugging()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex;
            if (activeSceneIndex < SceneManager.sceneCountInBuildSettings - 1)
            {
                nextSceneIndex = activeSceneIndex + 1;
            }
            else
            {
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {

        }
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
            isPlaying = false;
            //audioSource.loop = false;
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * Time.deltaTime * thrustForce);
        if (!audioSource.isPlaying)
        {
            audioSource.clip = thrustSFX;
            audioSource.Play();
        }

        if (!mainThrustParticles.isPlaying)
        {
            mainThrustParticles.Play();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainThrustParticles.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationSpeed);
        leftThrustParticles.Stop();
        if (!rightThrustParticles.isPlaying)
        {
            rightThrustParticles.Play();
        }
        
        PlayAudio();
    }

    private void RotateRight()
    {
        ApplyRotation(-rotationSpeed);
        rightThrustParticles.Stop();
        if (!leftThrustParticles.isPlaying)
        {
            leftThrustParticles.Play();
        }
        
        PlayAudio();
    }

    public void PlayAudio()
    {
        if (!isPlaying)
        {
            audioSource.clip = thrustSFX;
            audioSource.Play();
            audioSource.loop = true;
            isPlaying = true;
        }
    }

    private void StopRotating()
    {
        leftThrustParticles.Stop();
        rightThrustParticles.Stop();
    }

    void ApplyRotation(float currentFrameRotation)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * Time.deltaTime * currentFrameRotation);
        rb.freezeRotation = false;
    }
}
