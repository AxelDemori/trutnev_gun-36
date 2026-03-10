using UnityEngine;

namespace Sounds
{

    public class SoundHeal : MonoBehaviour
    {
        [SerializeField] private AudioClip _sound;
        private bool _hasPlayed = false;

        private void OnTriggerEnter(Collider other)
        {
            if (_hasPlayed) return;

            if (other.CompareTag("Player") && _sound != null)
            {
                AudioSource.PlayClipAtPoint(_sound, transform.position);
                _hasPlayed = true;
            }
        }
    }
}