using UnityEngine;

namespace TheLegends.Base.UI
{
    public class PlayableSound : UIAnimationBase
    {
        [SerializeField]
        private AudioClip _audioClip;

        [SerializeField]
        private float _volume = 1f;

        [SerializeField]
        private float _pitch = 1f;

        [SerializeField]
        private bool _loop = false;

        private AudioSource _audioSource;

        public override void Play()
        {
            if (_audioSource == null)
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
                _audioSource.clip = _audioClip;
                _audioSource.volume = _volume;
                _audioSource.pitch = _pitch;
                _audioSource.loop = _loop;
            }
            _audioSource.Play();
        }

        public override void Stop()
        {
            if (_audioSource != null)
            {
                _audioSource.Stop();
            }
        }

        public override void Pause()
        {
            if (_audioSource != null && _audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
        }

        public override void Restart()
        {
            Stop();
            Play();
        }
    }
}
