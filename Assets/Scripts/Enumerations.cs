using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PwndaGames.TapMonsters
{

    public enum EnemyState
    {
        Alive,
        Dying,
        Dead
    }

    public enum EnemyType
    {
        Normal,
        Boss
    }

    public enum GameControllerState
    {
        Start,
        Spawn

    }
}
