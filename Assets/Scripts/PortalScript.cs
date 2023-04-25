using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PortalScript : MonoBehaviour
{
    [SerializeField] GameModes gameMode;
    [SerializeField] Speeds speed;
    [SerializeField] Gravity gravity;
    [SerializeField] int state;

    void OnCollisionEnter2D(Collision2D collision)
    {
        try
        {
            Movement movement = collision.gameObject.GetComponent<Movement>();

            movement.ChangeThroughPortal(gameMode, speed, gravity, state);
        }
        catch { }
    }
}
