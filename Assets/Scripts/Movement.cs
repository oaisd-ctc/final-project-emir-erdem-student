using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public enum Speeds { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 };
public enum GameModes { Cube = 0, Ship = 1 };
public class Movement : MonoBehaviour
{
    [SerializeField] Speeds currentSpeed;
    [SerializeField] GameModes currentGameMode;
    [SerializeField] AudioClip deathSound;

    public float GroundCheckRadius;
    public LayerMask GroundMask;

    bool upsideDown;
    float[] speedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };
    int currentSceneIndex;

    BoxCollider2D myCollider;
    Rigidbody2D rb;
    bool dead;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!dead)
        {
            transform.position += Vector3.right * speedValues[(int)currentSpeed] * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * 0.1f * Time.deltaTime;
        }
        Invoke(currentGameMode.ToString(), 0);

        if (rb.velocity.y < -24.2f)
        {
            rb.velocity = new Vector2(rb.velocity.y, -24.2f);
        }

        Die();
    }

    void OnJump(InputValue value)
    {

        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            upsideDown = false;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * 26.65f, ForceMode2D.Impulse);
        }

        else if (myCollider.IsTouchingLayers(LayerMask.GetMask("UpSideDown")) && value.isPressed)
        {
            upsideDown = true;
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.down * 30.65f, ForceMode2D.Impulse);
        }


    }

    void Cube()
    {
        if (!myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && !myCollider.IsTouchingLayers(LayerMask.GetMask("UpSideDown")))
        {
            if (upsideDown == false)
            {
                transform.Rotate(Vector3.back, 452.415f * Time.deltaTime);
            }
            else
            {
                transform.Rotate(Vector3.forward, 452.415f * Time.deltaTime);
            }
        }
        else
        {
            Vector3 Rotation = transform.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z / 90) * 90;
            transform.rotation = Quaternion.Euler(Rotation);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BoxEdge")
        {
            dead = true;
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);

            StartCoroutine(WaitDeath());

        }
    }

    void Die()
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Danger")))
        {
            dead = true;
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position);

            StartCoroutine(WaitDeath());

        }
    }

    IEnumerator WaitDeath()
    {
        dead = true;
        yield return new WaitForSeconds(0.25f);
        Destroy(gameObject);
        SceneManager.LoadScene(currentSceneIndex);

    }

    public void ChangeThroughPortal(GameModes gameModes, Speeds speed, int gravity, int State)
    {
        switch (State)
        {
            case 0:
                currentSpeed = speed;
                break;
            case 1:
                currentGameMode = gameModes;
                break;
            case 2:
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * (int)gravity;
                break;
        }
    }
}
