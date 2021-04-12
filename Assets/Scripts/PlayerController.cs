using System.Linq;
using System.Transactions;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    // the current strength of the hit
    private float _strength = 5;

    // the verticle angle
    private float _verticalAngle = 1f;

    // how fast the hit direction can be changed horizontally
    [SerializeField] private float horizontalSpeed;
    // how fast the hit direction can be changed vertically
    [SerializeField] private float verticalSpeed;
    // the maximum verticle angle
    [SerializeField] private float maxVerticalAngle;
    // how fast the strength of the initial hit can be changed
    [SerializeField] private float strengthSpeed;

    // fields for min and max strength
    [SerializeField] private float maxStrength;
    [SerializeField] private float minStrength;

    // getter and setter for init velocity
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
        // reset up transform based on planet
        transform.up = (GameManager.Instance.GolfBall.transform.position - GameManager.Instance.Planet.transform.position).normalized;
    }
    

    private void Update()
    {
        // only let player input when in input state
        if (GameManager.Instance.CurrentState != GameManager.GameState.Input) return;

        // Calculate parameters based on user input;

        var turnDir = Input.GetAxis("Horizontal");

        transform.rotation *= Quaternion.AngleAxis(turnDir * horizontalSpeed * Time.deltaTime, Vector3.up);

        var angleIncrease = Input.GetAxis("Vertical");

        _verticalAngle = Mathf.Min(Mathf.Max(_verticalAngle + angleIncrease * Time.deltaTime * verticalSpeed, 0), maxVerticalAngle);

        // allow strength change only when not zooming
        if (!Input.GetKey(KeyCode.LeftControl))
        {
            var strengthDelta = Input.GetAxis("Mouse ScrollWheel");

            _strength = Mathf.Min(Mathf.Max(minStrength, _strength + strengthDelta * Time.deltaTime * strengthSpeed), maxStrength);
        }
        
        // Calculate initial velocity based on parameters.
        
        var rotation = Quaternion.AngleAxis(_verticalAngle, -transform.right);
        var direction = rotation * transform.forward;
        InitVelocity = _strength * direction;
        
        // start ball movement when space is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartBallMovement(InitVelocity);
        }
        

    }
}
