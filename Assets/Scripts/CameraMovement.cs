using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
            return;

        if (keyboard.aKey.isPressed)
        {
            transform.position += speed * Time.deltaTime * Vector3.left;
        }
        if (keyboard.dKey.isPressed)
        {
            transform.position += speed * Time.deltaTime * Vector3.right;
        }
        if (keyboard.wKey.isPressed)
        {
            transform.position += speed * Time.deltaTime * Vector3.up;
        }
        if (keyboard.sKey.isPressed)
        {
            transform.position += speed * Time.deltaTime * Vector3.down;
        }
    }
}