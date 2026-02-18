using UnityEngine;

namespace CardMatch.Audio
{
    public class AudioService : IAudioService
    {       
        // Audio sources
        //TODO: Consider using a pool of audio sources for SFX to allow multiple clips to play simultaneously.
        //For now, we'll use one for SFX and one for Music.
        private AudioSource sfxAudioSource;
        private AudioSource musicAudioSource;
        
        public AudioService()
        {            
            InitializeAudioSources();
        }

        private void InitializeAudioSources()
        {
            //Create an audio game object to hold AudioSource
            GameObject audioObject = new GameObject("AudioObject");

            sfxAudioSource = audioObject.AddComponent<AudioSource>();
            sfxAudioSource.playOnAwake = false;

            musicAudioSource = audioObject.AddComponent<AudioSource>();
            musicAudioSource.playOnAwake = false;
            musicAudioSource.loop = true;
        }

        #region Play Methods
       
        public void Play(AudioData clipData)
        {
            if (clipData == null || !clipData.IsValid())
            {
                Logger.LogWarning("Invalid AudioClipData provided!");
                return;
            }

            AudioSource source = GetAudioSourceForCategory(clipData.Category);
            ConfigureAndPlayAudio(source, clipData);
        }

        #endregion

        #region Audio Source Management
      
        // Gets the appropriate audio source based on category
        private AudioSource GetAudioSourceForCategory(AudioCategory category)
        {
            return category == AudioCategory.Music ? musicAudioSource : sfxAudioSource;
        }

        private void ConfigureAndPlayAudio(AudioSource source, AudioData clipData)
        {                        
            // Play audio
            if (clipData.Loop)
            {
                source.clip = clipData.Clip;
                source.volume = clipData.Volume;
                source.loop = true;
                source.Play();
            }
            else
            {                
                source.PlayOneShot(clipData.Clip, clipData.Volume);
            }
        }

        #endregion

        #region Utility Methods
        //Stop all audio
        public void StopAll()
        {
            sfxAudioSource?.Stop();
            musicAudioSource?.Stop();
        }

       
        // Stops music only        
        public void StopMusic()
        {
            musicAudioSource?.Stop();
        }
        #endregion
    }
}
