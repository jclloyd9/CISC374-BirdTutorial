using Unity.VisualScripting;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myBird;
    public float flapStrength = 5f;
    public LogicScript logic;
    public bool birdIsAlive = true;
    public AudioClip jumpSound;
    public AudioClip birdLose;
    public AudioSource audioSource;
    
    public float maxRotation = 30f; // Max tilt angle (upwards)
    public float minRotation = -60f; // Max tilt angle (downwards)
    public float rotationSpeed = 10f; // Smooth rotation

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            myBird.linearVelocity = Vector2.up * flapStrength;
            audioSource.PlayOneShot(jumpSound);
        }

        // Rotate based on velocity
        RotateBird();

        if (birdIsAlive && transform.position.y <= -14)
        {
            killBird();
        }
    }

    private void killBird()
    {
        logic.GameOver();
        birdIsAlive = false;
        audioSource.PlayOneShot(birdLose);
    }

    private void RotateBird()
    {
        if (birdIsAlive)
        {
            float angle = Mathf.Clamp(myBird.linearVelocity.y * 5f, minRotation, maxRotation);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (birdIsAlive)
        {
        killBird();
        }
    }
}
