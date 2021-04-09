using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCamera : MonoBehaviour
{
    [SerializeField] float radius;

    [SerializeField] float speed;

    private void Awake()
    {
        GameStateManager.Instance.ONLevelStartExit += () =>
        {
            var up = GameStateManager.Instance.GolfBall.transform.position - GameStateManager.Instance.Planet.transform.position;
            up.Normalize();

            transform.position = GameStateManager.Instance.Planet.transform.position + up * radius;

        };

        Cursor.visible = false;

    }

    void LateUpdate()
    {

        //if (!Input.GetMouseButton(0)) return;

        //if(Input.GetMouseButtonUp(0)) _oldMousePos = Input.mousePosition;

        var currentPos = transform.position - GameStateManager.Instance.Planet.transform.position;

        var rot1 = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * speed * Time.deltaTime, Vector3.up);
        var rot2 = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * speed * Time.deltaTime, Vector3.right);

        transform.position = GameStateManager.Instance.Planet.transform.position + rot1 * rot2 * currentPos;

        transform.LookAt(GameStateManager.Instance.Planet.transform);
    }
}
