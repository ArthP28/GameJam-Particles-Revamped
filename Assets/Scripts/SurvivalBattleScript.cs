using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalBattleScript : MonoBehaviour
{
    [SerializeField] LivesSystem Player1Lives;
    [SerializeField] LivesSystem Player2Lives;

    bool _gameOver = false;
    bool _player1Wins = false;
    bool _player2Wins = false;

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
                _player2Wins=true;
            } else if (Player2Lives.GetCurrentLives() <= 0)
            {
                Debug.Log("Player 1 Wins!");
                _gameOver = true;
                _player1Wins=true;
            }
        }
    }

    public bool Player1Won()
    {
        return _player1Wins;
    }

    public bool Player2Won()
    {
        return _player2Wins;
    }
}
