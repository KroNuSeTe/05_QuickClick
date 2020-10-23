using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    private const string HI_SCORE = "HI_SCORE";

    public enum GameState {
        loading,
        inGame,
        gameOver
    }

    public GameState gameState;

    [Header ("Targets List")]
    public List<GameObject> targetPrefabs;

    private float spawnRate = 1.0f;

    [Header ("Text Variables")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiScoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI levelText;

    [Header ("")]
    public Button restartButton;

    //Variable Autocomputada
    private int _score;
    private int score {
        set {
            _score = Mathf.Clamp (value, 0, 99999);

        }
        get {
            return _score;
        }
    }

    public GameObject startMenu;

    [Header ("Hearts Panel")]
    public GameObject heartsPanel;
    public List<GameObject> hearts;
    public Sprite heartBrokenImage;
    public ParticleSystem heartBrokenParticle;
    private int numberOfHearts = 3;

    public GameObject swordFollow;
    private string level;
    private float startTime;

    private SoundEffectsManager soundEffectsManager;

    private int difficultyPlusScore;

    private void Start () {
        soundEffectsManager = FindObjectOfType<SoundEffectsManager> ();
        ShowMaxScore ();
        UpdateScore (0);
    }

    private void Update () {
        if (Input.GetMouseButtonDown (0)) {
            soundEffectsManager.PlayKatanaAudioClip ();
            swordFollow.SetActive (true);
        } else if (Input.GetMouseButtonUp (0)) {
            swordFollow.SetActive (false);
        }
    }

    private void FixedUpdate () {
        float totalTime = Time.time - startTime;
        if (gameState == GameState.inGame) {

            if (totalTime > 6 && totalTime < 12) {
                level = "level 2";
                levelText.text = "level2";
                Time.timeScale = 1.1f;
            } else if (totalTime > 12 && totalTime < 18) {
                level = "level 3";
                levelText.text = "level3";
                Time.timeScale = 1.3f;
            } else if (totalTime > 18 && totalTime < 24) {
                level = "level 4";
                levelText.text = "level4";
                Time.timeScale = 1.5f;
            } else if (totalTime > 24 && totalTime < 30) {
                level = "level 5";
                levelText.text = "level5";
                Time.timeScale = 1.6f;
            } else if (totalTime > 30 && totalTime < 36) {
                level = "level 6";
                levelText.text = "level6";
                Time.timeScale = 1.8f;
            } else if (totalTime > 36 && totalTime < 42) {
                level = "level 7";
                levelText.text = "level7";
                Time.timeScale = 1.9f;
            } else if (totalTime > 42 && totalTime < 48) {
                level = "level 8";
                levelText.text = "level8";
                Time.timeScale = 2f;
            } else if (totalTime > 48 && totalTime < 54) {
                level = "level 9";
                levelText.text = "level9";
                Time.timeScale = 2.2f;
            }
        }
    }

    /// <summary>
    /// Método que inicia la partida cambiando el valor del estado del juego
    /// </summary>
    /// <param name="difficulty"> Número entero que indica el grado de dificultad del juego </param>
    public void StartGame (float difficulty, string typeDifficulty) {
        levelText.gameObject.SetActive (true);
        levelText.text = "level1";
        startTime = Time.time;

        gameState = GameState.inGame;
        startMenu.SetActive (false);
        heartsPanel.SetActive (true);

        spawnRate /= difficulty;

        // Cambiar puntuación Targets dependiendo de la dificultad
        if (typeDifficulty == "easy") {
            difficultyPlusScore = 0;
        } else if (typeDifficulty == "normal") {
            difficultyPlusScore = 2;
        } else if (typeDifficulty == "hard") {
            difficultyPlusScore = 5;
        }

        StartCoroutine (SpawnTarget ());
        score = 0;
        UpdateScore (0);
    }

    IEnumerator SpawnTarget () {
        while (gameState == GameState.inGame) {
            yield return new WaitForSeconds (spawnRate);
            int index = Random.Range (0, targetPrefabs.Count);
            Instantiate (targetPrefabs[index]);
        }
    }

    /// <summary>
    /// Actualiza la puntación y lo muestra por pantalla
    /// </summary>
    /// <param name="scoreToAdd"> Número de puntos a añadir a la puntuación global</param>
    public void UpdateScore (int scoreToAdd) {
        score += scoreToAdd + difficultyPlusScore;
        scoreText.text = "Score: " + score;
    }

    /// <summary>
    /// Actualiza el High Score al inicio del juego, 
    /// si no está creada la variable en PlayerPrefs se inicializa a 0
    /// </summary>
    public void ShowMaxScore () {
        int hiScore = PlayerPrefs.GetInt (HI_SCORE, 0);
        hiScoreText.text = "Hi-Score: " + hiScore;
    }

    /// <summary>
    /// Muestra el mensaje de Game Over, muestra el botón de Restart 
    /// y Pausa el tiempo de juego
    /// </summary>
    public void GameOver (string itemType) {

        if (itemType == "Good") {
            numberOfHearts--;
            if (numberOfHearts >= 0) {
                HeartBrokenAndParticle();
            }

        } else {
            // itemType = "Bad"
            for (int i=numberOfHearts; i>0; i--){
                numberOfHearts--;
                HeartBrokenAndParticle();
            }
            numberOfHearts = 0;
        }

        if (numberOfHearts <= 0) {
            SetHiScore ();
            gameState = GameState.gameOver;
            gameOverText.gameObject.SetActive (true);
            restartButton.gameObject.SetActive (true);
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// Añade unas Particulas, cambia el sprite por el broken heart y baja la opacidad
    /// </summary>
    private void HeartBrokenAndParticle(){
                Image heartImage = hearts[numberOfHearts].GetComponent<Image> ();
                Vector3 posHeartBrokenParticle = Camera.main.ScreenToWorldPoint(heartImage.transform.position);
                Instantiate (heartBrokenParticle, posHeartBrokenParticle, heartBrokenParticle.transform.rotation);
                heartImage.sprite = heartBrokenImage;
                Color tempColor = heartImage.color;
                tempColor.a = 0.5f;
                heartImage.color = tempColor;
    }

    /// <summary>
    /// Actualiza el High Score y lo muestra en pantalla
    /// </summary>
    private void SetHiScore () {
        int hiScore = PlayerPrefs.GetInt (HI_SCORE, 0);
        if (score > hiScore) {
            PlayerPrefs.SetInt (HI_SCORE, score);
            ShowMaxScore ();
            //TODO: Si hay nueva puntuación máxima, animarla!!!
        }
    }

    /// <summary>
    /// Carga de nuevo la escena para empezar nueva partida
    /// </summary>
    public void RestartGame () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }

}