using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float angularSpeed, rotationThreshold;
    [SerializeField] private FloatDampener speedX, speedY;

    private Animator animator;

    private int speedXHash;
    private int speedYHash;
    private void SolveCharacterRotation()
    {
        Vector3 floorNormal = transform.up;
        Vector3 cameraRealForward = camera.transform.forward;
        float angleInterpolator = Mathf.Abs(Vector3.Dot(cameraRealForward, floorNormal));

        Vector3 cameraForward = Vector3.Lerp(cameraRealForward, camera.transform.up, angleInterpolator).normalized;

        Debug.DrawLine(transform.position, transform.position + cameraForward * 2, Color.blue, 5);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 inputValue = context.ReadValue<Vector2>();

        speedX.TargetValue = inputValue.x;
        speedY.TargetValue = inputValue.y;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");
    }

#if UNITY_EDITOR
    private void Update()
    {
        speedX.Update();
        speedY.Update();

        animator.SetFloat(speedXHash, speedX.CurrentValue);
        animator.SetFloat(speedYHash, speedY.CurrentValue);

        SolveCharacterRotation();
    }
#endif
}