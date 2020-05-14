using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {
    public enum PowerUp { Speed, MaxAmmo };
    public PowerUp powerType;

    public Vector3 rotationAngle = new Vector3(0f, 0f, 1f);
    public float rotationSpeed = 10;

    public float floatSpeed = 0.005f;
    public float floatRate = 0.5f;
    private bool goingUp = true;
    private float floatTimer;

    private float lifeTime = 10f;
    private float currLife = 0f;

    private GameController game;
    private Renderer rendererObject;

	void Start () {
        game = GameObject.Find("GameController").GetComponent<GameController>();
        rendererObject = GetComponent<Renderer>();

        StartCoroutine(ManageBlink());
	}
	
	// Update is called once per frame
	void Update () {
        if (!game.paused) {
            currLife += Time.deltaTime;

            transform.Rotate(rotationAngle * rotationSpeed * Time.deltaTime);

            floatTimer += Time.deltaTime;
            Vector3 moveDir = new Vector3(0.0f, 0.0f, floatSpeed);
            transform.Translate(moveDir);

            if (goingUp && floatTimer >= floatRate) {
                goingUp = false;
                floatTimer = 0;
                floatSpeed = -floatSpeed;
            }

            else if(!goingUp && floatTimer >= floatRate) {
                goingUp = true;
                floatTimer = 0;
                floatSpeed = +floatSpeed;
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            if (powerType == PowerUp.Speed) {
                other.gameObject.GetComponent<PlayerController>().SuperSpeed();
            } else if (powerType == PowerUp.MaxAmmo) {
                other.gameObject.GetComponent<PlayerController>().MaxAmmo();
            }
            
            Destroy(gameObject);
        }
    }

    IEnumerator ManageBlink() {
        yield return new WaitForSeconds(3f);

        while (currLife < lifeTime / 2) {
            rendererObject.enabled = !rendererObject.enabled;
            yield return new WaitForSeconds(0.5f);
        }

        while (currLife < lifeTime * 3/4) {
            rendererObject.enabled = !rendererObject.enabled;
            yield return new WaitForSeconds(0.3f);
        }

        while (currLife < lifeTime) {
            rendererObject.enabled = !rendererObject.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
