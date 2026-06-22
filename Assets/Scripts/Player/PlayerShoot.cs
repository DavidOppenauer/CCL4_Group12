using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Transform aimCamera;
    [SerializeField] private PlayerInputs playerInputs;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private GameObject bloodParticlePrefab;

    private PlayerAnimator playerAnimator;
    private Ray ray;
    private RaycastHit hitData;

    // Ammo Logic
    private int maxAmmo = 6;
    private int currentAmmo;
    private float shotCooldown = 1f;
    private bool canShoot = true;
    private float timer = 0f;
    private int debugHitCount;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        currentAmmo = maxAmmo;
        timer = shotCooldown;
    }
    public void HandleShooting()
    {
        timer += Time.deltaTime;
        if (playerInputs.GetShootWasPressedThisFrame() && currentAmmo > 0 && canShoot == true && timer >= shotCooldown)
        {           
            Debug.Log(currentAmmo);
            canShoot = false;
            //reduce ammo
            currentAmmo--;
            // play shoot animation
            playerAnimator.PlayShootAnimation();
            // Fire Ray
            FireRay();
            // reset Timer
            timer = 0f;
        }
    }

    public void ResetBullets()
    {
        currentAmmo = maxAmmo;
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
        if (Physics.Raycast(ray, out hitData, 100, hitLayer))
        {
            Vector3 hitPosition = hitData.point;
            float hitDistance = hitData.distance;
            GameObject hitObject = hitData.transform.gameObject;
            debugHitCount++;
            Debug.Log("hitted object name = " + hitObject.name);
            Debug.Log("hitted object position = " + hitPosition);
            Debug.Log("hitted object distance = " + hitDistance);
            Debug.Log("hitcount = " + debugHitCount);
            if (hitData.collider.tag == "Enemy")
            {
                EnemyBase hitEnemy = hitData.collider.GetComponentInParent<EnemyBase>();
                Debug.Log("You hit the enemy: " + hitEnemy.name);
                Instantiate(bloodParticlePrefab, hitData.point, Quaternion.LookRotation(hitData.normal));
                hitEnemy.OnHit();
            }
        }
        canShoot = true;
    }
}