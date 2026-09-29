using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform muzzle;

    [Header("Recoil")]
    [SerializeField] private float recoilForce = 5f;

    [Header("Ammo")]
    [SerializeField] private int magazineSize = 6;
    [SerializeField] private float reloadTime = 1f;

    private int currentAmmo;
    private bool isReloading;

    public int CurrentAmmo => currentAmmo;
    public int MagazineSize => magazineSize;
    public bool IsReloading => isReloading;
    public float ReloadTime => reloadTime;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentAmmo = magazineSize;
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartReload();
        }
    }

    private void Shoot()
    {
        // Cannot shoot while reloading.
        if (isReloading)
        {
            return;
        }

        // Cannot shoot with an empty magazine.
        if (currentAmmo <= 0)
        {
            return;
        }

        currentAmmo--;

        Instantiate(
            bulletPrefab,
            muzzle.position,
            muzzle.rotation
        );

        Vector2 shootDirection = muzzle.right;

        rb.AddForce(
            -shootDirection * recoilForce,
            ForceMode2D.Impulse
        );

        Debug.Log("Ammo: " + currentAmmo);
    }

    private void StartReload()
    {
        // Cannot reload while already reloading.
        if (isReloading)
        {
            return;
        }

        // No need to reload a full magazine.
        if (currentAmmo == magazineSize)
        {
            return;
        }

        StartCoroutine(Reload());
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;

        Debug.Log("Reloaded! Ammo: " + currentAmmo);
    }
}