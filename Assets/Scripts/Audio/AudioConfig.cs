using UnityEngine;

namespace  CardMatch.Audio
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Card Match/Audio/Card Audio Config")]
    public class AudioConfig : ScriptableObject
    {
        [Header("Audio Data")]
        [SerializeField]
        private AudioData cardClickData;
        [SerializeField]
        private AudioData matchWinData;
        [SerializeField]
        private AudioData uiButtonClickData;
        [SerializeField]
        private AudioData bgData;

        public AudioData CardClickData => cardClickData;
        public AudioData MatchWinData => matchWinData;
        public AudioData UIButtonClickData => uiButtonClickData;
        public AudioData GameBGData => bgData;
    }
}
