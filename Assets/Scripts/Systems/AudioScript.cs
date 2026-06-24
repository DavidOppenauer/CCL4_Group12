using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public void Footstep_DeadZ()
    {
        AkUnitySoundEngine.PostEvent("Play_Footsteps_DeadZ", gameObject);
    }
    public void Footstep_Player()
    {
        AkUnitySoundEngine.PostEvent("Play_Footsteps_Player", gameObject);
    }
}
