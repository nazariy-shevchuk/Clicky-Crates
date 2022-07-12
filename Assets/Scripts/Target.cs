using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targerRb;
    private GameManager gameManager;

    [SerializeField] private static float minSpeed = 12;
    [SerializeField] private static float maxSpeed = 16;
    [SerializeField] private static float maxTorque = 10;
    [SerializeField] private static float xRange = 4;
    [SerializeField] private const float ySpawnPos = -2;

    public int pointValue;
    public ParticleSystem explosionParticle;

    void Start()
    {
        targerRb = GetComponent<Rigidbody>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        targerRb.AddForce(RandomForce(), ForceMode.Impulse);    // Toss object
        targerRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);    // Spin object

        transform.position = RandomSpawnPos();    // Set default position
    }

    // When target is clicked, destroy it, update score, and generate explosion
    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
        }
        if (gameObject.CompareTag("Player"))
        {
            gameManager.UpdateLives(1);
        }
    }

    // If target that is NOT the bad object collides with sensor, trigger game over
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);

        if (!gameObject.CompareTag("Bad") && gameManager.isGameActive)
        {
            gameManager.UpdateLives(-1);
        }
    }

    // Generate a random force value
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    // Generate a random torque value
    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    // Generate a random spawn position
    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }
}
