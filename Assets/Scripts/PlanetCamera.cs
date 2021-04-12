using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    // rotation and zoom speed
    [SerializeField] float speed;
    [SerializeField] float zoomSpeed;

    // how much forward vector should be rotated around x and y axis respectively
    private float xAngle;
    private float yAngle;

    // the current radius where the camera is
    private float radius;

    private void Awake()
    {
        radius = 15;
        // make cursor visible when level is completed
        GameManager.Instance.ONLevelCompleted += () => Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.LevelComplete) return;
        
        // hide mouse cursor while rotating
        if(Input.GetMouseButtonDown(0)) Cursor.visible = false;
        if (Input.GetMouseButtonUp(0)) Cursor.visible = true;

        // increase x and y angle proportionate to mouse delta
        if (Input.GetMouseButton(0))
        {
            yAngle += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            xAngle += Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

            // clamp xangle between -60 and 60 degrees
            xAngle = Mathf.Max(Mathf.Min(60, xAngle), -60);
        }

        // zoom based on scroll wheel when left ctrl is held
        if (Input.GetKey(KeyCode.LeftControl))
        {
            radius -= Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            radius = Mathf.Max(Mathf.Min(radius, 30), 15);
        }

        // calculate rotation based on angles
        var rot = Quaternion.Euler(xAngle, yAngle, 0);

        // calculate camera offset
        var offset = rot * Vector3.forward * radius;

        // set camera position
        transform.position = GameManager.Instance.Planet.transform.position + offset;

        // look at planet
        transform.LookAt(GameManager.Instance.Planet.transform);
    }
}
