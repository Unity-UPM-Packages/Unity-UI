using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLegends.Base.UI
{
    public class PlayableParticle : UIAnimationBase
    {
        [SerializeField]
        private ParticleSystem _particleSystem;

        [SerializeField]
        private bool _loop = false;

        private ParticleSystem _instance;

        public override void Play()
        {
            ParticleSystem.MainModule mainModule = _particleSystem.main;
            mainModule.loop = true;
            _particleSystem.Play();
        }

        public override void Stop()
        {
            if (_instance != null)
            {
                _instance.Stop();
            }
        }

        public override void Pause()
        {
            if (_instance != null && _instance.isPlaying)
            {
                _instance.Pause();
            }
        }

        public override void Restart()
        {
            Stop();
            Play();
        }
    }
    
}
