using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frog : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public Animator animator;
    public Source source;
    public Joystick joystick;
    public float speed = 2;

    public enum Source
    {
        Keyboard,
        Joystick,
        Accelerometer,
        Gyroscope
    }

    Vector2 moveDir;

    private void Start()
    {
        Debug.Log("Accelerometer : " + SystemInfo.supportsAccelerometer);
        Debug.Log("Gyroscope : " + SystemInfo.supportsGyroscope);

        Input.gyro.enabled = true;
    }
    private void Update()
    {
        switch (source)
        {
            case Source.Keyboard:
                moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                break;
            case Source.Joystick:
                moveDir = joystick.Direction;
                break;
            case Source.Accelerometer:
                moveDir = Input.acceleration;
                break;
            case Source.Gyroscope:
                moveDir = (Vector2)Input.gyro.gravity;
                break;
        }

        transform.Translate(moveDir * speed * Time.deltaTime);

        spriteRend.flipX = (moveDir.x < 0);

        animator.SetBool("isMoving", moveDir != Vector2.zero);
    }
}
