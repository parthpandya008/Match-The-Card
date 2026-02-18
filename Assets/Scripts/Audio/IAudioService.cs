using UnityEngine;

namespace CardMatch.Audio
{
    // Interface for audio service
    public interface IAudioService 
    {
        // Play methods        
        void Play(AudioData clipData);        

        // Utility
        void StopAll();
        void StopMusic();       
    }
}
