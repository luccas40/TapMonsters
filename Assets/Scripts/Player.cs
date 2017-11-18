using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using PwndaGames.TapMonsters;
using System;
using System.Collections.Generic;

namespace PwndaGames.TapMonsters
{
    [Serializable]
    public class Player
    {
        


        private double gastoBase = 8;

        private int level;
        private Pitolar gold;
        private float criticalRate;
        private float criticalDamage;
        private Pitolar damage;
        private Dictionary<int, Soldier> soldiers;

        public int Level { get { return level; } set { level = value; damageCalculator(); } }
        public Pitolar Gold { get { return gold; } set { gold = value; } }
        public float CriticalRate { get { return criticalRate; }  }
        public float CriticalDamage { get { return criticalDamage; }}
        public Pitolar Damage { get { return damage; } }
        public Dictionary<int, Soldier> Soldados { get { return soldiers; } }


        public Player()
        {
            level = 1;
            gold = Pitolar.ZERO;
            criticalRate = 1f;
            criticalDamage = 1.5f;
            soldiers = new Dictionary<int, Soldier>();
            damageCalculator();
        }


        public Pitolar cost2LevelUp()
        {
            Pitolar cost = new Pitolar(gastoBase, 0) * (double)level / 5d;
            cost += Math.Pow(1.03d, (level - 1));
            cost *= Math.Pow(1.047d, level);
            return cost;
        }

        void damageCalculator()
        {
            damage = new Pitolar(level, 0);
            damage *= Math.Pow(1.03d, level);
            damage -= 1;
        }






    }
}