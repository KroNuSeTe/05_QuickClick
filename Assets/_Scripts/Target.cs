using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class Target : MonoBehaviour {

    private Rigidbody _rigidbody;

    [Header ("Force Up Item Values")]
    [Range (10, 14)]
    public float minForce = 12;
    [Range (14, 18)]
    public float maxForce = 16;

    [Header ("Torque Rotation Item Values")]
    [Range (-10, -30)]
    public float minTorque = -10;
    [Range (10, 30)]
    public float maxTorque = 10;

    [Header ("Location where item appears")]
    [Range (-5, -3)]
    public float minStartLocationX = -4;
    [Range (3, 5)]
    public float maxStartLocationX = 4;
    [Range (-4, -7)]
    public float startLocationY = -6;

    private GameManager gameManager;

    [Header ("")]
    public int pointValue;

    public ParticleSystem explosionParticle;

    private SoundEffectsManager soundEffectsManager;

    // Start is called before the first frame update
    void Start () {
        _rigidbody = GetComponent<Rigidbody> ();

        _rigidbody.AddForce (RandomForce (), ForceMode.Impulse);
        _rigidbody.AddTorque (RandomTorque (), RandomTorque (), RandomTorque (), ForceMode.Impulse);
        transform.position = RandomSpawnPosition ();

        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager = FindObjectOfType<GameManager> ();

        soundEffectsManager = FindObjectOfType<SoundEffectsManager>();
    }

    /// <summary>
    /// Genera un Vector aleatorio en 3D
    /// </summary>
    /// <returns> Fuerza aleatoria para arriba </returns>
    private Vector3 RandomForce () {
        return Vector3.up * Random.Range (minForce, maxForce);
    }

    /// <summary>
    /// Genera un Número aleatorio
    /// </summary>
    /// <returns> Devuelve un valor aleatorio entre minTorque y maxTorque </returns>
    private float RandomTorque () {
        return Random.Range (minTorque, maxTorque);
    }

    /// <summary>
    /// Genera una posición aleatoria
    /// </summary>
    /// <returns> Posición aleatoria en 3D, con la coordenada z = 0 </returns>
    private Vector3 RandomSpawnPosition () {
        return new Vector3 (Random.Range (minStartLocationX, maxStartLocationX), startLocationY); // z = 0
    }

    private void OnMouseOver() {
        if (Input.GetMouseButton(0)){
            if (gameManager.gameState == GameManager.GameState.inGame) {
                Instantiate (explosionParticle, transform.position, explosionParticle.transform.rotation);
                if (gameObject.CompareTag ("ItemBad")) {
                    soundEffectsManager.PlayBombAudioClip();
                    gameManager.GameOver ("Bad");
                } 
                if (gameObject.CompareTag("ItemGood")){
                    soundEffectsManager.PlaySplashAudioClip();
                    gameManager.UpdateScore(pointValue);
                }
                Destroy (gameObject);      
            }
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (other.CompareTag ("KillZone")) {
            Destroy (gameObject);
            if (gameObject.CompareTag ("ItemGood")) {
                gameManager.GameOver ("Good");
            }
        }
    }
}