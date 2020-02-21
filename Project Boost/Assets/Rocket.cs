using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip levelLoadSound;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        // todo somewhere stop sound on death
        if(state == State.Alive) {
            RespondToThrustInput();
            Rotate();
        }
    }

    // Detects if Rocket Ship has collided with an object
    private void OnCollisionEnter(Collision collision) {

        // Ignore collisions when dead
        if(state != State.Alive) { return ; }

        switch(collision.gameObject.tag) {
            case "Friendly":
                // Do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence() {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelLoadSound);
        Invoke("LoadNextScene", 1f); // parameterise time
    }

    private void StartDeathSequence() {
        // Kill player
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
        Invoke("RestartGame", 1f);
    }

    private void RestartGame() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
        // todo allow for more than 2 levels
        SceneManager.LoadScene(1);
    }

    private void RespondToThrustInput() {
        // Can thrust while rotating
        if (Input.GetKey(KeyCode.Space)) {
            ApplyThrust();
        } else {
            audioSource.Stop();
        }
    }

    private void ApplyThrust() {
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);
        // So the audio doesn't layer
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void Rotate() {

        // Take manual control of rotation
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        } else if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        // Resume physics control
        rigidBody.freezeRotation = false; 
    }
}
