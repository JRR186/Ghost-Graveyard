using TMPro;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public GunSystem gunSystem;
    public AudioSource pickupSound;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            if (gunSystem != null)
            {
                gunSystem.currentMagazines++;
           Debug.Log("Mags are now " +  gunSystem.currentMagazines);
                pickupSound.Play();
                Destroy(gameObject);
            }

        }
    }
}
