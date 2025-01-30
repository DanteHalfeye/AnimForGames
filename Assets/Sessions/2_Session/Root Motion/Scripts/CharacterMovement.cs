using UnityEngine;
[RequireComponent(typeof(Animator))]
public class CharacterMovement : MonoBehaviour
{
    [Range(-1,1)][SerializeField] private float speedX, speedY;

    private Animator animator;

    int speedXHash, speedYHash;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        speedXHash = Animator.StringToHash("SpeedX");
        speedYHash = Animator.StringToHash("SpeedY");
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        if (animator == null)
        {
            return;
        }
        animator.SetFloat(speedXHash, speedX);
        animator.SetFloat(speedYHash, speedY);
    }
#endif
}
