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

    public GameObject swordFollow;

    private int numberOfLives = 3;

    private string level;

    private float startTime;

    private void Start () {
        ShowMaxScore ();
        UpdateScore(0);
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)){
            swordFollow.SetActive(true);
        } else if (Input.GetMouseButtonUp(0)){
            swordFollow.SetActive(false);
        }
    }

    private void FixedUpdate() {
        /*float totalTime = startTime-Time.time;
        if (gameState == GameState.inGame){
            if (totalTime<=)
            else if (totalTime <= 6)
            {
                level = "level 2";
                levelText.text = "level2";
                Time.timeScale = 1.1f;
            }
            else if (Time.time >= 12)
            {
                level = "level 3";
                levelText.text = "level3";
                Time.timeScale = 1.3f;
            }
            if (Time.time == 18)
            {
                level = "level 4";
                levelText.text = "level4";
                Time.timeScale = 1.5f;
            }
            if (Time.time == 24)
            {
                level = "level 5";
                levelText.text = "level5";
                Time.timeScale = 1.6f;
            }
            if (Time.time == 30)
            {
                level = "level 6";
                levelText.text = "level6";
                Time.timeScale = 1.8f;
            }
            if (Time.time == 36)
            {
                level = "level 7";
                levelText.text = "level7";
                Time.timeScale = 1.9f;
            }
            if (Time.time == 42)
            {
                level = "level 8";
                levelText.text = "level8";
                Time.timeScale = 2f;
            }
            if (Time.time == 48)
            {
                level = "level 9";
                levelText.text = "level9";
                Time.timeScale = 2.2f;
            }
        }
        */
        //TODO: Implementar la dificultad correctamente
    }

    /// <summary>
    /// Método que inicia la partida cambiando el valor del estado del juego
    /// </summary>
    /// <param name="difficulty"> Número entero que indica el grado de dificultad del juego </param>
    public void StartGame (float difficulty) {
        levelText.gameObject.SetActive(true);
        levelText.text = "level1";
        startTime = Time.time;

        gameState = GameState.inGame;
        startMenu.SetActive (false);

        spawnRate /= difficulty;

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
        score += scoreToAdd;
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
    public void GameOver () {
        SetHiScore();
        gameState = GameState.gameOver;
        gameOverText.gameObject.SetActive (true);
        restartButton.gameObject.SetActive (true);
        Time.timeScale = 1f;
        Time.
    }

    /// <summary>
    /// Actualiza el High Score y lo muestra en pantalla
    /// </summary>
    private void SetHiScore(){
        int hiScore = PlayerPrefs.GetInt (HI_SCORE, 0);
        if (score > hiScore) {
            PlayerPrefs.SetInt (HI_SCORE, score);
            ShowMaxScore();
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