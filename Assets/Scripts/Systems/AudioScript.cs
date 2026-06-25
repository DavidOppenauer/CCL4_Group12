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
    public void Eject_Bullets()
    {
        AkUnitySoundEngine.PostEvent("Play_Eject_Bullets", gameObject);
    }
    public void Cylinder_Spin()
    {
        AkUnitySoundEngine.PostEvent("Play_Cylinder_Spin", gameObject);
    }
    
}
