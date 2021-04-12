using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    [SerializeField] float minRadius = 15;

    [SerializeField] float speed;
    [SerializeField] float zoomSpeed;

    private float rightAngle;
    private float upAngle;

    private float radius;

    private void Awake()
    {
        radius = minRadius;
        GameManager.Instance.ONInputEnter += () => rightAngle = Mathf.Max(Mathf.Min(60, rightAngle), -60);
        GameManager.Instance.ONLevelCompleted += () => Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.LevelComplete) return;
        
        if(Input.GetMouseButtonDown(0)) Cursor.visible = false;
        if (Input.GetMouseButtonUp(0)) Cursor.visible = true;

        if (Input.GetMouseButton(0))
        {
            upAngle += Input.GetAxis("Mouse X") * speed * Time.deltaTime;
            rightAngle += Input.GetAxis("Mouse Y") * speed * Time.deltaTime;

            rightAngle = Mathf.Max(Mathf.Min(60, rightAngle), -60);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            radius -= Time.deltaTime * Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
            radius = Mathf.Max(Mathf.Min(radius, 30), 15);
        }

        var rot = Quaternion.Euler(rightAngle, upAngle, 0);

        var currentPos = rot * Vector3.forward * radius;

        transform.position = GameManager.Instance.Planet.transform.position + currentPos;

        transform.LookAt(GameManager.Instance.Planet.transform);
    }
}
