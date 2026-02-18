using UnityEngine;

namespace CardMatch.Audio
{
    [CreateAssetMenu(fileName = "Audio Data", menuName = "Card Match/Audio/Audio Data")]
    public class AudioData : ScriptableObject
    {
        [Header("Audio Clip")]
        [Tooltip("The audio clip to play")]
        [SerializeField]
        private AudioClip clip;

        [Header("Configuration")]
        [SerializeField]
        private AudioCategory category = AudioCategory.SFX;

        [Tooltip("Base volume (0-1)")]
        [Range(0f, 1f)]
        [SerializeField]
        private float volume = 1f;        

        [Tooltip("Should this audio loop?")]
        [SerializeField]
        private bool loop = false;

        public AudioClip Clip => clip;
        public AudioCategory Category => category;
        public float Volume => volume;
        public bool Loop => loop;

        // Validates the audio clip data        
        public bool IsValid()
        {
            return clip != null;
        }
    }
    public enum AudioCategory
    {
        SFX,
        Music,
        UI
    }
}
