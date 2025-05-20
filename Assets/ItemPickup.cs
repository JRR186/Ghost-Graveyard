using TMPro;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public PlayerController playerController;
    public GunSystem gunSystem;
    public AudioSource pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (CompareTag("Ammo"))
            {
                gunSystem.currentMagazines++;
            }
            if (CompareTag("Health"))
            {
                playerController.playerHealth += 10;
            }

          pickupSound.Play();
          Destroy(gameObject);
        }
    }
}
