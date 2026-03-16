using UnityEngine;

public class WeaponSound : MonoBehaviour
{
    public AudioClip shootSound;

    public void PlayShoot()
    {
        AudioSource.PlayClipAtPoint(shootSound, transform.position);
    }
}