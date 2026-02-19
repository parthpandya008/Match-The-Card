using UnityEngine;

namespace CardMatch.Score
{
    // Interface for score storage: JSON, Binary, Cloud saves, etc
    public interface IScoreStorage
    {
        void Save(GameSaveData data);
        GameSaveData Load();
        bool HasSaveFile();
        void Delete();
        string FilePath { get; }
    }
}
