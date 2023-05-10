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
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip portalSFX;

    public float GroundCheckRadius;
    public LayerMask GroundMask;

    bool upsideDown;
    float[] speedValues = { 8.6f, 10.4f, 12.96f, 15.6f, 19.27f };

    BoxCollider2D myCollider;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        transform.position += Vector3.right * speedValues[(int)currentSpeed] * Time.deltaTime;

        Invoke(currentGameMode.ToString(), 0);

        if (rb.velocity.y < -24.2f)
        {
            rb.velocity = new Vector2(rb.velocity.y, -24.2f);
        }

        Die();
    }

    public void OnJump(InputValue value)
    {

        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) && value.isPressed)
        {
            upsideDown = false;
            rb.velocity = Vector2.zero;
            AudioSource.PlayClipAtPoint(jump, gameObject.transform.position, 0.25f);
            rb.AddForce(Vector2.up * 26.65f, ForceMode2D.Impulse);
        }

        else if (myCollider.IsTouchingLayers(LayerMask.GetMask("UpSideDown")) && value.isPressed)
        {
            upsideDown = true;
            rb.velocity = Vector2.zero;
            AudioSource.PlayClipAtPoint(jump, gameObject.transform.position, 0.25f);
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
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Destroy(gameObject);
            SceneManager.LoadScene(currentSceneIndex);

        }

        if (collision.gameObject.tag == "Portal")
        {
            AudioSource.PlayClipAtPoint(portalSFX, gameObject.transform.position, 0.30f);
        }
    }

    void Die()
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Danger")))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            Destroy(gameObject);
            SceneManager.LoadScene(currentSceneIndex);

        }
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
