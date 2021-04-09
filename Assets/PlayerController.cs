using System.Linq;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float _strength = 0;

    private float _verticalAngle = 1f;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private float strengthSpeed;

    public Vector3 InitVelocity { get; private set; }

    private void Awake()
    {

        GameStateManager.Instance.ONInputEnter += HandleInputEnter;
    }

    private void HandleInputEnter()
    {
        _verticalAngle = 0;
        _strength = 1f;

        transform.up = (GameStateManager.Instance.GolfBall.transform.position - GameStateManager.Instance.Planet.transform.position).normalized;
    }
    

    private void Update()
    {

        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.Input) return;

        // Calculate parameters based on user input;

        var turnDir = Input.GetAxis("Horizontal");

        transform.rotation *= Quaternion.AngleAxis(turnDir * horizontalSpeed * Time.deltaTime, Vector3.up);

        var angleIncrease = Input.GetAxis("Vertical");

        _verticalAngle = Mathf.Min(Mathf.Max(_verticalAngle + angleIncrease * Time.deltaTime * verticalSpeed, 0), maxVerticalAngle);

        var strengthDelta = Input.GetAxis("Mouse ScrollWheel");

        _strength = Mathf.Max(1f, _strength + strengthDelta * Time.deltaTime * strengthSpeed);

        // Calculate initial velocity based on parameters.
        
        var rotation = Quaternion.AngleAxis(_verticalAngle, -transform.right);
        var direction = rotation * transform.forward;
        InitVelocity = _strength * direction;
        
        // Hit golf ball and apply initial velocity when space is pressed.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameStateManager.Instance.StartBallMovement(InitVelocity);
        }
        

    }
}
