using System.Linq;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private float _strength = 5;

    private float _verticalAngle = 1f;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private float strengthSpeed;

    [SerializeField] private float maxStrength;
    [SerializeField] private float minStrength;

    public Vector3 InitVelocity { get; private set; }

    private void Awake()
    {

        GameManager.Instance.ONInputEnter += HandleInputEnter;
        GameManager.Instance.ONLevelStartExit += () => {
            _strength = 5;
        };
    }

    private void HandleInputEnter()
    {
        transform.up = (GameManager.Instance.GolfBall.transform.position - GameManager.Instance.Planet.transform.position).normalized;
    }
    

    private void Update()
    {

        if (GameManager.Instance.CurrentState != GameManager.GameState.Input) return;

        // Calculate parameters based on user input;

        var turnDir = Input.GetAxis("Horizontal");

        transform.rotation *= Quaternion.AngleAxis(turnDir * horizontalSpeed * Time.deltaTime, Vector3.up);

        var angleIncrease = Input.GetAxis("Vertical");

        _verticalAngle = Mathf.Min(Mathf.Max(_verticalAngle + angleIncrease * Time.deltaTime * verticalSpeed, 0), maxVerticalAngle);

        if (!Input.GetKey(KeyCode.LeftControl))
        {
            var strengthDelta = Input.GetAxis("Mouse ScrollWheel");

            _strength = Mathf.Min(Mathf.Max(minStrength, _strength + strengthDelta * Time.deltaTime * strengthSpeed), maxStrength);
        }
        
        // Calculate initial velocity based on parameters.
        
        var rotation = Quaternion.AngleAxis(_verticalAngle, -transform.right);
        var direction = rotation * transform.forward;
        InitVelocity = _strength * direction;
        
        // Hit golf ball and apply initial velocity when space is pressed.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartBallMovement(InitVelocity);
        }
        

    }
}
