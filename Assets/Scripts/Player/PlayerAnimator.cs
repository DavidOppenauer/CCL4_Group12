using UnityEngine;

public class PlayerAnimator : MonoBehaviour {

    [SerializeField] private PlayerBrain playerBrain;
    [SerializeField] private PlayerRailMovement playerMovement;
    // You can ether make a Serialized field or use getComponent to get a different script/component
    private Animator animator;
    
    private const string WALKING = "IsWalking";
    private const string AIMING = "IsAiming";
    private const string SHOOT = "Shoot";
    private const string RELOAD = "Reload";

    private void Awake() {
        animator = GetComponent<Animator>(); // Get the animator Component on this Obeject
    }

    private void Update()
    {
        animator.SetBool(WALKING, playerMovement.GetIsPlayerWalking());
        animator.SetBool(AIMING, playerBrain.GetIsPlayerAiming());
    }

    public void PlayShootAnimation()
    {
        animator.SetTrigger(SHOOT);
    }
    public void PlayReloadAnimation()
    {
        animator.ResetTrigger(SHOOT);
        animator.SetTrigger(RELOAD);
    }
}