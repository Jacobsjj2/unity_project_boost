using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    private Rigidbody rigidBody;
    private AudioSource audioSource;

    [SerializeField] float rcsThrust = 250f;
    [SerializeField] float mainThrust = 2000f;

    [SerializeField] AudioClip engineAudio;
    [SerializeField] AudioClip explosionAudio;
    [SerializeField] AudioClip successAudio;

    [SerializeField] ParticleSystem engineParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem successParticles;

    [SerializeField] float timeBetweenLevels = 1f;

    enum State
    {
        Alive,
        Dying,
        Transcending
    }

    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        currentState = State.Alive;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != State.Alive)
        {
            return;
        }

        RespondToThrustInput();
        RespondToRotateInput();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (currentState != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // Do nothing
                break;
            case "Finish":
                Transcend();
                break;
            default:
                Die();
                break;
        }
    }

    private void Transcend()
    {
        currentState = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(successAudio);
        engineParticles.Stop();
        successParticles.Play();
        Invoke("LoadNextLevel", timeBetweenLevels);
    }

    private void Die()
    {
        currentState = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(explosionAudio);
        engineParticles.Stop();
        explosionParticles.Play();
        Invoke("RestartGame", timeBetweenLevels);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrust();
        }
        else if (audioSource.isPlaying)
        {
            audioSource.Stop();
            engineParticles.Stop();
        }
    }

    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(engineAudio);
        }
        engineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // for manual control to avoid over-rotating

        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics control
    }
}
