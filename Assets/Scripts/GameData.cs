using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PwndaGames.TapMonsters
{   
    [Serializable]
    public class GameData
    {
        private Player player;
        private int fase;
        private int level;

        public Player Jogador { get { return player; } }
        public int Fase { get { return fase; } }
        public int Level { get { return level; } }


        public GameData(Player p)
        {
            fase = 1;
            level = 1;
            player = p;
        }

        public void levelUp() {
            level++;
            if(level >= 11)
            {
                level = 1;
                fase++;
            }
        }

        public void levelDown() { level = 1; }



    }
}
