using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using TMPro;

public class GunSystem : MonoBehaviour
{
    public int damage, magazineSize, totalMagazines, bulletsPerTap, bulletsLeft, currentMagazines;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public bool automatic;

    int bulletsShot;
    bool shooting, readyToShoot, reloading;

    public CinemachineCamera cam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask enemy;
    public LayerMask player;
    public Animator animator;

    public GameObject muzzleFlash, bulletHoleGraphic;
    public CinemachineImpulseSource impulseSource;
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI magText;
    EnemyAi enemies;

    

    private void Start()
    {
        bulletsLeft = magazineSize;
        currentMagazines = totalMagazines - 1;
        readyToShoot = true;
    }
    private void Update()
    {
        if (automatic)
        {
            if (shooting && readyToShoot && !reloading && bulletsLeft > 0)
            {
                Shoot();
            }
        }
        ammoText.SetText(bulletsLeft.ToString());
        magText.SetText("/" + currentMagazines.ToString());
    }

    public void OnFire(InputAction.CallbackContext context)
{
        if (automatic)
    {
        shooting = context.ReadValueAsButton();
        if (shooting && readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }
    else
    {
        if (context.performed && readyToShoot && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }
}

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed && bulletsLeft < magazineSize && !reloading && currentMagazines > 0)
        {
            Reload();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        

        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);


        if (Physics.Raycast(cam.transform.position, direction, out rayHit, range, enemy))
        {
            if (rayHit.collider.CompareTag("Enemy")) {
                rayHit.collider.GetComponent<EnemyAi>().TakeDamage(damage); 
            } 
        }

            impulseSource.GenerateImpulse();
            animator.SetTrigger("Fire");

        Quaternion bulletHoleRotation = Quaternion.LookRotation(rayHit.normal);

        if (Mathf.Abs(rayHit.normal.y) > 0.7f)
        {
            bulletHoleRotation = Quaternion.LookRotation(Vector3.forward, rayHit.normal);
        }

        GameObject bulletHole = Instantiate(bulletHoleGraphic, rayHit.point, bulletHoleRotation);
        Destroy(bulletHole, 1f);

        GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        Destroy(flash, 0.1f);

        bulletsLeft--;
            bulletsShot++;

        Invoke("ResetShot", timeBetweenShooting);
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        if (currentMagazines > 0 && bulletsLeft < magazineSize)
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
    }
    private void ReloadFinished()
    {
        currentMagazines--;
        bulletsLeft = magazineSize;
        reloading = false;
    }
    
}
