using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField] private float horizontalSpeed = 10f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private float clampAngleUp = 40f;
    [SerializeField] private float clampAngleDown = 60f;

    PInputManager pInputManager;
    Vector3 startingRotation;

    protected override void Awake()
    {
        pInputManager = FindAnyObjectByType<PInputManager>();
        base.Awake();
        startingRotation = transform.localRotation.eulerAngles;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = new Vector2(pInputManager.cameraInputX, pInputManager.cameraInputY);
                startingRotation.x += deltaInput.x * verticalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * horizontalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngleDown, clampAngleUp);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x,0f);

            }
        }
    }
}

