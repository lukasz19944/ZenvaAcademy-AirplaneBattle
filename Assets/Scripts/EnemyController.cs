using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public delegate void KillHandler();
    public event KillHandler OnKill;

    public float speed = 1f;
    public float bulletSpeed = 4f;
    public float shootingInterval = 6f;

    public GameObject bulletPrefab;

    private float shootingTimer;

	// Use this for initialization
	void OnEnable () {
        shootingTimer = Random.Range(0f, shootingInterval);

        GetComponent<Rigidbody2D>().velocity = new Vector2(0, speed);
	}
	
	// Update is called once per frame
	void Update () {
        shootingTimer -= Time.deltaTime;

        if (shootingTimer <= 0f) {
            shootingTimer = shootingInterval;

            GameObject bulletInstance = Instantiate(bulletPrefab, transform.parent);
            bulletInstance.transform.position = transform.position;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);

            Destroy(bulletInstance, 3f);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("PlayerBullet")) {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            Destroy(collision.gameObject);

            if (OnKill != null) {
                OnKill();
            }
        }
    }
}
