using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject EnemyOriginal;
    [SerializeField] Canvas roundCounter;
    [SerializeField] TMP_Text label;
    [SerializeField] Animator animatorRound;
    [SerializeField] float intermissionTime = 30f;
    [SerializeField] Canvas timerCanvas;
    [SerializeField] TMP_Text timerTXT;

    GameObject[] Enemies;

    int round;
    int NumberOfEnemies;
    int EnemiesAlive;

    bool timerOn;
    float timeLeft;
   
    public void RoundBegin()
    {
        round += 1;
        NumberOfEnemies += 1;
        animatorRound.Play("roundCounter");
        label.text = "Round " + round.ToString();
        SpawnEnemies();
    }
    void SpawnEnemies()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            while (NumberOfEnemies > 0)
            {
                Vector3 SpawnPOS = new Vector3(0, 2, 0);
                int SpawnLocation = Random.Range(1, 5);
                if (SpawnLocation == 1)
                {
                    SpawnPOS.x = Random.Range(-45f, 45f);
                    SpawnPOS.z = Random.Range(-45f, -35f);
                }
                if (SpawnLocation == 2)
                {
                    SpawnPOS.x = Random.Range(-45f, -35f);
                    SpawnPOS.z = Random.Range(-45f, 45f);
                }
                if (SpawnLocation == 3)
                {
                    SpawnPOS.x = Random.Range(-45, 45);
                    SpawnPOS.z = Random.Range(35f, 45f);
                }
                if (SpawnLocation == 4)
                {
                    SpawnPOS.x = Random.Range(45f, 35f);
                    SpawnPOS.z = Random.Range(-45f, 45f);
                }
                GameObject EnemyClone = PhotonNetwork.Instantiate("Enemy", SpawnPOS, Quaternion.identity);
                NumberOfEnemies -= 1;
            }
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            EnemiesAlive = Enemies.Length;
        }
    }
    public void EnemyKilled()
    {
        NumberOfEnemies += 1;
        EnemiesAlive -= 1;
        if (EnemiesAlive == 0)
        {
            timerCanvas.enabled = true;
            timerOn = true;
            timeLeft = intermissionTime;
            StartCoroutine(intermission());
        }
    }
    IEnumerator intermission()
    {
        yield return new WaitForSeconds(intermissionTime);
        timerCanvas.enabled = false;
        RoundBegin();
    }
    private void Update()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0;
                timerOn = false;
            }

        }
    }
    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        timerTXT.text = string.Format("{0:00} : {1:00}", minutes, seconds);

    }
}
