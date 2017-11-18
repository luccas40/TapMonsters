using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System.Text;

namespace PwndaGames.TapMonsters
{
    public class GameEngine : MonoBehaviour
    {


        public GameObject[] Enemies;
        private GameObject spawnedEnemy;
        Text[] FaseTxt = new Text[2];

        private GameData gameData;

        private GameControllerState state;

        void OnApplicationQuit()
        {
            if(state == GameControllerState.Spawn)
                Util.getInstance().saveGame(gameData);
        }


        void OnApplicationPause()
        {
            if (state == GameControllerState.Spawn)
                Util.getInstance().saveGame(gameData);
        }



        void Start()
        {
            state = GameControllerState.Start;
            FaseTxt[0] = GameObject.FindGameObjectWithTag("HUD#Fase").GetComponent<Text>();
            FaseTxt[1] = GameObject.FindGameObjectWithTag("HUD#LevelFase").GetComponent<Text>();
            Util.getInstance ().loadGame (out gameData);
            if (gameData == null)
                gameData = new GameData(GameObject.Find("Player").GetComponent<PlayerController>().Jogador);
            else
                GameObject.Find("Player").GetComponent<PlayerController>().setPlayer(gameData.Jogador);
            state = GameControllerState.Spawn;
        }

        void Update()
        {
            switch (state)
            {
                case GameControllerState.Start: break;
                case GameControllerState.Spawn: enemySpawn(); break;
            }
        }


        public void enemySpawn()
        {
            if(spawnedEnemy == null)
            {
                int rand = Random.Range(0, Enemies.Length - 1);
                spawnedEnemy = Instantiate(Enemies[rand]);
                spawnedEnemy.GetComponent<Enemy>().setVida(Util.getInstance().getEnemyHP(gameData.Fase, gameData.Level));
                spawnedEnemy.GetComponent<Enemy>().setGold(Util.getInstance().getEnemyGold(gameData.Fase, gameData.Level));
                if (gameData.Level == 10) { spawnedEnemy.GetComponent<Enemy>().setBoss(30f); }
            }
            else
            {
                Enemy e = spawnedEnemy.GetComponent<Enemy>();
                if(e.Tipo == EnemyType.Boss)
                {
                    if(e.BossTimer <= 0 && e.State == EnemyState.Alive)
                    {
                        Destroy(spawnedEnemy.gameObject);
                        gameData.levelDown();
                    }
                    if(e.BossTimer > 0 && e.State == EnemyState.Dead)
                    {
                        Destroy(spawnedEnemy.gameObject);
                        gameData.levelUp();
                    }
                }
                else if (e.State == EnemyState.Dead)
                {
                    Destroy(spawnedEnemy.gameObject);
                    gameData.levelUp();
                }
            }
            updateHUD();
        }


        void updateHUD()
        {
            FaseTxt[0].text = "Fase " + gameData.Fase;
            FaseTxt[1].text = gameData.Level + "/10";
        }

    }
}   
