using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    [SerializeField] private PlayerRailMovement playerMovement;
    // You can ether make a Serialized field or use getComponent to get a different script/component
    private Animator animator;
    
    private const string WALKING = "IsWalking";

    private void Awake() {
        animator = GetComponent<Animator>(); // Get the animator Component on this Obeject
    }

    private void Update()
    {
        animator.SetBool(WALKING, playerMovement.GetIsPlayerWalking());
    }


}
