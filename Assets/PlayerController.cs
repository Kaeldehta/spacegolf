using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{

    private Golfable _golfable;

    private float _strength;

    private float _verticalAngle;

    private bool _inputAllowed;

    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private float strengthSpeed;
    
    void Start()
    {
        _golfable = FindObjectOfType<Golfable>();
        _inputAllowed = true;
        _verticalAngle = 0;
        _strength = 0.01f;
    }

    private void Update()
    {
        if (!_inputAllowed) return;

        // Calculate parameters based on user input;

        var turnDir = Input.GetAxis("Horizontal");
        
        transform.rotation *= Quaternion.AngleAxis(turnDir * horizontalSpeed * Time.deltaTime, Vector3.up);

        var angleIncrease = Input.GetAxis("Vertical");

        _verticalAngle = Mathf.Min(Mathf.Max(_verticalAngle + angleIncrease * Time.deltaTime * verticalSpeed, 0), maxVerticalAngle);

        var strengthDelta = Input.GetAxis("Mouse ScrollWheel");

        _strength = Mathf.Max(0.01f, _strength + strengthDelta * Time.deltaTime * strengthSpeed);

        // Calculate initial velocity based on parameters.
        
        var rotation = Quaternion.AngleAxis(_verticalAngle, -transform.right);
        var direction = rotation * transform.forward;
        var initVelocity = _strength * direction;
        
        Debug.DrawLine(_golfable.transform.position, _golfable.transform.position + initVelocity, Color.red);
        
        // Hit golf ball and apply initial velocity when space is pressed.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            _golfable.Hit(initVelocity);
            _inputAllowed = false;
        }
        

    }
}
