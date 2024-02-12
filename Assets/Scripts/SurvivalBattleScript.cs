using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalBattleScript : MonoBehaviour
{
    [SerializeField] LivesSystem Player1Lives;
    [SerializeField] LivesSystem Player2Lives;

    bool _gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_gameOver)
        {
            if(Player1Lives.GetCurrentLives() <= 0)
            {
                Debug.Log("Player 2 Wins!");
                _gameOver = true;
            } else if (Player2Lives.GetCurrentLives() <= 0)
            {
                Debug.Log("Player 1 Wins!");
                _gameOver = true;
            }
        }
    }
}
