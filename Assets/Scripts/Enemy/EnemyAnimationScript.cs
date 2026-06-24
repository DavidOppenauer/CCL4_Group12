using UnityEngine;

public class EnemyAnimationScript : MonoBehaviour
{
    public void Footstep_DeadZ()
    {
        AkUnitySoundEngine.PostEvent("Play_Footsteps_DeadZ", gameObject);
    }
}
