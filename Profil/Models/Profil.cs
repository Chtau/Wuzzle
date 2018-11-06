using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wuzzle.Profil.Models
{
    public class Profil
    {
        public int Score { get; set; }
        public long PlayedSeconds { get; set; }

        public Profil()
        {
            Score = 0;
            PlayedSeconds = 0;
        }
    }
}
