using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DificultyButton : MonoBehaviour
{

    private Button _button;
    private GameManager gameManager;

    [Range (1,3)]
    public float difficulty;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        _button = GetComponent<Button>(); 
        _button.onClick.AddListener(SetDifficulty); 

    }

    void SetDifficulty(){
            Debug.Log("The Button " + gameObject.name + " is pushed");
            gameManager.StartGame(difficulty);
    }
}
