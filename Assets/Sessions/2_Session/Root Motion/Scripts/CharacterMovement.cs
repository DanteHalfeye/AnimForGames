using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private FloatDampener speedX;
    [SerializeField] private FloatDampener speedY;
    [SerializeField] private float angularSpeed;

    private Animator animator;
    private int speedXHash, speedYHash;

    private Quaternion targetRotation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");

        if (m_Camera == null)
        {
            Debug.LogError("Camera reference is missing in CharacterMovement!", this);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        speedX.TargetValue = Mathf.Clamp(inputValue.x, -1f, 1f);
        speedY.TargetValue = Mathf.Clamp(inputValue.y, -1f, 1f);

    }

    private void SolveCharacterRotation()
    {
        if (m_Camera == null) return;

        Vector3 floorNormal = transform.up;
        Vector3 cameraRealForward = m_Camera.transform.forward;

        float angleInterpolator = Mathf.Abs(Vector3.Dot(cameraRealForward, floorNormal));
        Vector3 cameraForward = Vector3.Lerp(cameraRealForward, m_Camera.transform.up, angleInterpolator).normalized;

        Vector3 characterForward = Vector3.ProjectOnPlane(cameraForward, floorNormal).normalized;
        Debug.DrawLine(transform.position, transform.position + characterForward * 2, Color.magenta, 5f);
        Quaternion lookRotation = Quaternion.LookRotation(cameraForward, floorNormal);
        targetRotation = Quaternion.RotateTowards(targetRotation, lookRotation, angularSpeed);

    }

    private void Update()
    {
        speedX.Update();
        speedY.Update();

        animator.SetFloat(speedXHash, speedX.CurrentValue);
        animator.SetFloat(speedYHash, speedY.CurrentValue);
        SolveCharacterRotation();
        float motionMagnitude = Mathf.Sqrt(speedX.TargetValue * speedX.TargetValue + speedY.TargetValue * speedY.TargetValue);
        float rotationSpeed = Mathf.SmoothStep(0, .1f, motionMagnitude);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);
        
    }
}
