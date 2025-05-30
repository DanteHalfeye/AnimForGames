using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour, ICharacterComponent
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private FloatDampener speedX;
    [SerializeField] private FloatDampener speedY;
    [SerializeField] private float angularSpeed;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float rotationThreshold;

    private PlayerHitboxController playerHitbox;
    private Animator animator;
    private int speedXHash, speedYHash;

    private Quaternion targetRotation;

    private void Awake()
    {
        playerHitbox = GetComponent<PlayerHitboxController>();
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");
        
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            animator.SetTrigger("Dodge");
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 inputValue = ctx.ReadValue<Vector2>();
        speedX.TargetValue = inputValue.x;
        speedY.TargetValue = inputValue.y;

    }
    public void OnRun(InputAction.CallbackContext ctx)
    {
        animator.SetBool("Run", ctx.ReadValue<float>() > 0.5f);
    }


    public void TogglePlayerHitbox(int hitboxId)
    {
        playerHitbox.TogglePlayerHitboxes(hitboxId);
    }

    public void CleanupPlayerHitboxes()
    {
        playerHitbox.CleanupPlayerHitboxes();
    }

    private void SolveCharacterRotation()
    {
        Vector3 floorNormal = transform.up;
        Vector3 cameraRealForward = m_Camera.transform.forward;

        float angleInterpolator = Mathf.Abs(Vector3.Dot(cameraRealForward, floorNormal));
        Vector3 cameraForward = Vector3.Lerp(cameraRealForward, m_Camera.transform.up, angleInterpolator).normalized;

        Vector3 characterForward = Vector3.ProjectOnPlane(cameraForward, floorNormal).normalized;
        Debug.DrawLine(transform.position, transform.position + characterForward * 2, Color.magenta, 5f);
        targetRotation = Quaternion.LookRotation(characterForward, floorNormal);

    }

    private void Update()
    {
        speedX.Update();
        speedY.Update();
        

        animator.SetFloat(speedXHash, speedX.CurrentValue); 
        animator.SetFloat(speedYHash, speedY.CurrentValue);
        SolveCharacterRotation();
        if (!ParentCharacter.IsAiming)
        {
            ApplyCharacterRotation();
        }
        // else
        //     ApplyCharacterRotationForAim();
    }

    private void ApplyCharacterRotation()
    {
        float motionMagnitude = Mathf.Sqrt(speedX.TargetValue * speedX.TargetValue + speedY.TargetValue * speedY.TargetValue);
        float rotationSpeed = Mathf.SmoothStep(0, .1f, motionMagnitude);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);
    }

    private void ApplyCharacterRotationForAim()
    {
        //angulo de piez y torso comparados para ver si se pasa del umbral
        Vector3 aimForeward = Vector3.ProjectOnPlane(aimTarget.forward, transform.up).normalized; //normalizado para el angulo
        Vector3 characterForward = transform.forward;
        float angleCos = Vector3.Dot(characterForward, aimForeward) ;//desde -1 a 1
        float rotationSpeed = Mathf.SmoothStep(0, 1, Mathf.Acos(angleCos) * Mathf.Rad2Deg/ rotationThreshold);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed * rotationSpeed);
    }
    public Character ParentCharacter { get; set; }
}
