using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public delegate void FuelHandler();
    public event FuelHandler OnFuel;

    public float horizontalSpeed = 3f;
    public float verticalSpeed = 1f;
    public float horizontalLimit = 2.8f;
    public float bulletSpeed = 5f;

    public GameObject bulletPrefab;

    private bool fired = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxis("Horizontal") * horizontalSpeed, verticalSpeed);

        if (transform.position.x > horizontalLimit) {
            transform.position = new Vector2(horizontalLimit, transform.position.y);
        } else if (transform.position.x < -horizontalLimit) {
            transform.position = new Vector2(-horizontalLimit, transform.position.y);
        }

        if (Input.GetAxis("Fire1") == 1f) {
            if (fired == false) {
                GameObject bulletInstance = Instantiate(bulletPrefab, transform.parent);
                bulletInstance.transform.position = transform.position;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);

                Destroy(bulletInstance, 3f);

                fired = true;
            } 
        } else {
            fired = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("EnemyBullet") || collision.CompareTag("Enemy")) {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        } else if (collision.CompareTag("Fuel")) {
            Destroy(collision.gameObject);

            if (OnFuel != null) {
                OnFuel();
            }
        }
    }
}
