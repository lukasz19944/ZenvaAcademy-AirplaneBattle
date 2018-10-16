using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public PlayerController player;
    public GameObject enemyPrefab;
    public GameObject fuelPrefab;
    public GameObject gameCamera;

    public Text scoreText;
    public Text fuelText;

    public float enemySpawnInterval = 1f;
    public float horizontalLimit = 2.8f;
    public float fuelDecreaseSpeed = 5f;
    public float fuelSpawnInterval = 9f;

    private float enemySpawnTimer;
    private float fuelSpawnTimer;
    private int score = 0;
    private float fuel = 100f;
    

	// Use this for initialization
	void Start () {
        enemySpawnTimer = enemySpawnInterval;

        player.OnFuel += OnFuel;

        fuelSpawnTimer = Random.Range(0f, fuelSpawnInterval);
	}
	
	// Update is called once per frame
	void Update () {
        if (player != null) {
            enemySpawnTimer -= Time.deltaTime;

            if (enemySpawnTimer <= 0) {
                enemySpawnTimer = enemySpawnInterval;

                GameObject enemyInstance = Instantiate(enemyPrefab, transform);
                enemyInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    player.transform.position.y + Screen.height / 100f
                );

                enemyInstance.GetComponent<EnemyController>().OnKill += OnEnemyKill;
            }

            fuelSpawnTimer -= Time.deltaTime;
            if (fuelSpawnTimer <= 0) {
                fuelSpawnTimer = fuelSpawnInterval;

                GameObject fuelInstance = Instantiate(fuelPrefab, transform);
                fuelInstance.transform.position = new Vector2(
                    Random.Range(-horizontalLimit, horizontalLimit),
                    player.transform.position.y + Screen.height / 100f
                );
            }

            fuel -= Time.deltaTime * fuelDecreaseSpeed;
            fuelText.text = "Fuel: " + (int)fuel;

            if (fuel <= 0) {
                Destroy(player.gameObject);
            }
        }

        foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>()) {
            if (gameCamera.transform.position.y - enemy.transform.position.y > Screen.height / 100f) {
                Destroy(enemy.gameObject);
            }
        }
    }

    void OnEnemyKill() {
        score += 25;
        scoreText.text = "Score: " + score;
    }

    void OnFuel() {
        fuel = 100f;
    }
}
