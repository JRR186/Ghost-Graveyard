using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static WaveSpawner;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private float countdown;
    [SerializeField] private GameObject spawnPoint;
    public int currentWave = 0;

    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI waveCountdownText;
    public Wave[] waves;
    private GameObject[] enemies;
    private bool readyToCountDown = true;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave >= waves.Length)
        {
            currentWaveText.SetText("All Waves Survived!");
            PlayerController playerController = FindFirstObjectByType<PlayerController>();
            playerController.StartCoroutine(playerController.LoadStart());
        }

        if (readyToCountDown == true)
        {
            currentWaveText.SetText("Wave " + (currentWave + 1).ToString());
            waveCountdownText.SetText(countdown.ToString("0.00") + " seconds");
            countdown -= Time.deltaTime;
        }
            if (countdown <= 0 )
        {
            readyToCountDown = false;
            currentWaveText.SetText("");
            waveCountdownText.SetText("");
            countdown = waves[currentWave].timeToNextWave;
            StartCoroutine(SpawnWave());
        }

        if (waves[currentWave].enemiesLeft == 0)
        {
            readyToCountDown = true;
            currentWave++;
        }
    }

    private IEnumerator SpawnWave()
    {
        if (currentWave < waves.Length)
        {
            Wave wave = waves[currentWave];
            enemies = new GameObject[wave.enemies.Length];
            wave.enemiesLeft = wave.enemies.Length;

            for (int i = 0; i < wave.enemies.Length; i++)
            {
                enemies[i] = Instantiate(wave.enemies[i].gameObject, spawnPoint.transform.position, spawnPoint.transform.rotation);
                enemies[i].SetActive(true);
                yield return new WaitForSeconds(wave.timeToNextEnemy);
            }
        }
    }

    [System.Serializable]
    public class Wave
    {
        public EnemyAi[] enemies;
        public float timeToNextEnemy;
        public float timeToNextWave;
        public int enemiesLeft;
    }
}