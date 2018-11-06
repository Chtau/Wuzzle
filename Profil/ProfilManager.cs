using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuzzle.Profil
{
    public sealed class ProfilManager
    {
        #region Singleton

        static ProfilManager()
        {
        }

        private ProfilManager()
        {
        }

        public static ProfilManager Instance { get; } = new ProfilManager();
        #endregion

        private Models.Profil profil;

        public long PlayedSeconds => profil.PlayedSeconds;
        public int Score => profil.Score;

        public event EventHandler<int> ScoreChanged;
        public event EventHandler<long> SecondsChanged;

        public bool Save()
        {
            //TODO: Save Profil to Filesystem
            return false;
        }

        public bool Load()
        {
            //TODO: Load Profil from Filesystem
            profil = new Models.Profil();
            return true;
        }

        public void AddPlayedSeconds(long seconds)
        {
            profil.PlayedSeconds += seconds;
            SecondsChanged?.Invoke(profil, profil.PlayedSeconds);
        }

        public void AddScore(int score)
        {
            profil.Score += score;
            ScoreChanged?.Invoke(profil, profil.Score);
        }
    }
}
