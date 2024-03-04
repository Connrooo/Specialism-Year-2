using UnityEngine;
using Cinemachine;
using Unity.VisualScripting.Antlr3.Runtime;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngleUp = 40f;
    [SerializeField] private float clampAngleDown = 60f;
    [SerializeField] private GameManagerStateMachine gameManager;
    PlayerStateMachine playerStateMachine;
    Vector3 startingRotation;
    Quaternion currentRot;

    protected override void Awake()
    {
        playerStateMachine = FindAnyObjectByType<PlayerStateMachine>();
        gameManager= FindAnyObjectByType<GameManagerStateMachine>();
        base.Awake();
        startingRotation = transform.localRotation.eulerAngles;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow&&gameManager.canMove)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = new Vector2(playerStateMachine.cameraInputX, playerStateMachine.cameraInputY);
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngleDown, clampAngleUp);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x,0f);
                currentRot = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);

            }
        }
        else
        { state.RawOrientation = currentRot; }
    }
}

