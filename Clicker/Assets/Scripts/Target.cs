using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRB;
    private GameManager gameManager;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 4;
    private float ySpawnPos = -2;


    private int click = 0;
    public ParticleSystem particleExplosion;
    public int pointValue;


    CoinsManager coinsManager;
    [SerializeField] GameObject coinNumPrefab;



    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        targetRB = GetComponent<Rigidbody>();
        targetRB.AddForce(RandomForce(), ForceMode.Impulse);
        targetRB.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);
        transform.position = RandomSpawnPos();
        coinsManager = FindObjectOfType<CoinsManager>();
    }


    private void OnMouseDown()
    {
        int randomCoins = Random.Range(0, 2);
        if (gameManager.isGameActive)
        {
            click++;
            gameManager.LevelUp(click);
            Destroy(gameObject);
            gameManager.UpdateScore(pointValue);
            Instantiate(particleExplosion, transform.position, particleExplosion.transform.rotation);

            coinsManager.AddCoins(new Vector3(0,1,0), randomCoins);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (!gameObject.CompareTag("Bad"))
        {
            gameManager.GameOver();
        }
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
