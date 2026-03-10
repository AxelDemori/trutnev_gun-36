using UnityEngine;

namespace Sounds
{
    public class SoundDamage : MonoBehaviour
    {
        [SerializeField] private AudioClip _sound;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player") && _sound != null)
            {
                AudioSource.PlayClipAtPoint(_sound, transform.position);
            }
        }
    }
}