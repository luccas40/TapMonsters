using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwndaGames.TapMonsters
{
    [Serializable]
    public class Soldier
    {

        int id;
        double BaseGold;
        float BaseAtkSpeed;
        double BaseDamage;

        int level = 1;
        float atkSpeed = 2.5f;
        Pitolar damage = new Pitolar(10, 0);

        public int ID { get { return id; } }
        public int Level { get { return level; } }
        public float AttackSpeed { get { return atkSpeed; } }
        public Pitolar Damage { get { return damage; } }

        public Soldier(int id, double BaseGold, float BaseAtkSpeed, double BaseDamage)
        {
            this.id = id;
            this.BaseGold = BaseGold;
            this.BaseAtkSpeed = BaseAtkSpeed;
            this.BaseDamage = BaseDamage;
            this.level = 1;
            this.atkSpeed = BaseAtkSpeed;
            damageCalculation();
        }


        public void damageCalculation()
        {
            damage = new Pitolar(BaseDamage, 0) *level;
        }

        public void setLevel(int level)
        {
            this.level = level;
            damageCalculation();
        }
        public int getLevel() { return level; }

        public void levelUp()
        {
            level += 1;
            damageCalculation();
        }
    }
}
