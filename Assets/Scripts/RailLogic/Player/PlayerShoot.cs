using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform aimCamera;
    [SerializeField] private PlayerInputs playerInputs;
    private Ray ray;
    private RaycastHit hitData;

    private int debugHitCount;
    public void HandleShooting()
    {
        if (playerInputs.GetShootWasPressedThisFrame())
        {
            // Fire Ray and do some effect maybe later on
            FireRay();
        }
    }

    private void FireRay()
    {
        // Vector3.left: x axis
        // Vector3.up: y axis
        // Vector3.forward: z axis
        // This itself does nothing its just a line
        ray = new Ray(aimCamera.position, aimCamera.forward); // You have to provide origin and direction

        // debugging a ray, where you can make it visible
        Debug.DrawRay(ray.origin, ray.direction * 10);

        // ref, in, out
        if (Physics.Raycast(ray, out hitData, 100))
        {
            Vector3 hitPosition = hitData.point;
            float hitDistance = hitData.distance;
            GameObject hitObject = hitData.transform.gameObject;
            debugHitCount++;

            // Debug.Log("hitted object name = " + hitObject.name);
            // Debug.Log("hitted object position = " + hitPosition);
            // Debug.Log("hitted object distance = " + hitDistance);
            // Debug.Log("hitcount = " + debugHitCount);
            if (hitData.collider.tag == "Enemy")
            {
                EnemyBase hitEnemy = hitData.collider.GetComponentInParent<EnemyBase>();
                Debug.Log("You hit the enemy: " + hitEnemy.name);

                hitEnemy.OnHit();
            }
            // if (hitObject.tag == "Enemy")
            // {
            //     HealthSystem enemyHealth = hitObject.GetComponent<HealthSystem>();
            //     enemyHealth.TakeDamage(100);
            // }

        }
    }
}