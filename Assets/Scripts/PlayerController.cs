using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool gameOver = false;
    public ParticleSystem explosion;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;

    private Rigidbody rb;
    private const float jumpForce = 30;
    private const float gravityModifier = 10;
    private bool onGround = true;
    private Quaternion startRotation;
    private Animator anim;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onGround == true && gameOver == false)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            onGround = false;
            dirtParticle.Stop();
            playerAudio.PlayOneShot(jumpSound, 1.0f);
        }
        if (onGround == false)
        {
            transform.Rotate(0.0f, 2000.0f * Time.deltaTime, 0.0f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            if (gameOver == false)
            {
                dirtParticle.Play();
            }
            transform.rotation = startRotation;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            anim.SetBool("Death_b", true);
            explosion.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            Debug.Log("GAME OVER!");
        }
    }
}
