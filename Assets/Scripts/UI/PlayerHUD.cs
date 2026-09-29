using UnityEngine;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private TMP_Text ammoText;
    [SerializeField] private TMP_Text reloadText;

    private float reloadStartTime;
    private bool wasReloading;

    private void Start()
    {
        reloadText.gameObject.SetActive(false);

        UpdateAmmoText();
    }

    private void Update()
    {
        UpdateAmmoText();
        UpdateReloadText();
    }

    private void UpdateAmmoText()
    {
        ammoText.text =
            "Ammo: " +
            playerShoot.CurrentAmmo + " / " +
            playerShoot.MagazineSize;
    }

    private void UpdateReloadText()
    {
        if (playerShoot.IsReloading)
        {
            if (!wasReloading)
            {
                reloadStartTime = Time.time;
            }

            float elapsed =
                Time.time - reloadStartTime;

            float remaining =
                Mathf.Max(
                    0f,
                    playerShoot.ReloadTime - elapsed
                );

            reloadText.text =
                "Reloading... " +
                remaining.ToString("F1") +
                "s";

            reloadText.gameObject.SetActive(true);
        }
        else
        {
            reloadText.gameObject.SetActive(false);
        }

        wasReloading = playerShoot.IsReloading;
    }
}