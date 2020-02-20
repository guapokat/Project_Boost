using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

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
            Thrust();
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
                state = State.Transcending;
                Invoke("LoadNextScene", 1f); // parameterise time
                break;
            default:
                // Kill player
                state = State.Dying;
                Invoke("RestartGame", 1f);
                break;
        }
    }

    private void RestartGame() {
        SceneManager.LoadScene(0);
    }

    private void LoadNextScene() {
        // todo allow for more than 2 levels
        SceneManager.LoadScene(1);
    }

    private void Thrust() {
        // Can thrust while rotating
        if (Input.GetKey(KeyCode.Space)) {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            // So the audio doesn't layer
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        } else {
            audioSource.Stop();
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
