using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR.WSA;

public class TrajectorySimulation : MonoBehaviour
{

    [SerializeField] private int steps = 10;

    private PhysicsScene _trajSimScene;
    private Scene _scene;
    private Rigidbody _simTarget;
    private LineRenderer _renderer;

    private Rigidbody simGolfableRb;

    [SerializeField] private GameObject golfBallSimPrefab;

    public void CreateTrajectory()
    {
        
        simGolfableRb.transform.SetPositionAndRotation(GameStateManager.Instance.GolfBall.transform.position, GameStateManager.Instance.GolfBall.transform.rotation);

        simGolfableRb.velocity = GameStateManager.Instance.Controller.InitVelocity;

        for (int i = 0; i < steps; i++)
        {
            
            // Calculate vector between this object and target
            Vector3 between = _simTarget.transform.position - simGolfableRb.transform.position;
            
            // Calculate the acceleration that needs to be applied to this GameObject
            Vector3 acceleration = GameSettings.Instance.GravitationalConstant  * _simTarget.mass /
                between.sqrMagnitude * between.normalized;

            // Apply acceleration to rigidbody
            simGolfableRb.AddForce(acceleration, ForceMode.Acceleration);

            _trajSimScene.Simulate(Time.fixedDeltaTime);

            if (i % 5 == 0)
            {
                _renderer.SetPosition(i / 5, simGolfableRb.transform.position);
            }
        }
        
        simGolfableRb.velocity = Vector3.zero;
        
    }

    public void ResetTrajectory()
    {
        _renderer.positionCount = 0;
        _renderer.positionCount = steps / 5;
    }

    private void Awake()
    {
        _renderer = GetComponent<LineRenderer>();
        _renderer.positionCount = steps / 5;

        GameStateManager.Instance.ONLevelStart += () =>
        {
            var param = new LoadSceneParameters(LoadSceneMode.Additive, LocalPhysicsMode.Physics3D);
            _scene = SceneManager.LoadScene("Scenes/TrajectorySimulationScene", param);
            _trajSimScene = _scene.GetPhysicsScene();

            _simTarget = Instantiate(GameStateManager.Instance.Planet).GetComponent<Rigidbody>();
            Destroy(_simTarget.GetComponent<MeshRenderer>());
            Destroy(_simTarget.GetComponent<MeshFilter>());
            _simTarget.tag = "Untagged";

            foreach (var meshRenderer in _simTarget.GetComponentsInChildren<MeshRenderer>())
            {
                Destroy(meshRenderer);
            }

            foreach (var meshFilter in _simTarget.GetComponentsInChildren<MeshFilter>())
            {
                Destroy(meshFilter);
            }

            SceneManager.MoveGameObjectToScene(_simTarget.gameObject, _scene);


            simGolfableRb = Instantiate(golfBallSimPrefab).GetComponent<Rigidbody>();

            SceneManager.MoveGameObjectToScene(simGolfableRb.gameObject, _scene);
        };

        GameStateManager.Instance.ONBallMovementEnter += velocity => ResetTrajectory();
        
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.Input) return;

        CreateTrajectory();
    }
}
