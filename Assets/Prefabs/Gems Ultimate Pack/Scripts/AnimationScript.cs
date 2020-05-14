using UnityEngine;
using System.Collections;

public class AnimationScript : MonoBehaviour {
    public Vector3 rotationAngle = new Vector3(0f, 0f, 1f);
    public float rotationSpeed = 10;

    public float floatSpeed = 0.005f;
    public float floatRate = 0.5;
    private bool goingUp = true;
    private float floatTimer;

    private GameController game;
    
	void Start () {
        game = GameObject.Find("GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!game.paused) {
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
}
