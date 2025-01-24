using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    // This script allows for customizeable player controls in one script
    public FrameInput FrameInput { get; private set; }
    public PlayerNumber playerNum; // The number that is set for this value determines the control schematics of a player object
    PlayerControls _playerActions; // The action map that the player controls derive from

    // Player inputs
    InputAction _jump;
    InputAction _move;
    InputAction _fire;

    // Start is called before the first frame update
    void Awake()
    {
        _playerActions = new PlayerControls();
        if(playerNum == PlayerNumber.Player1) // Controls are configured based on the player number set in the Unity Editor
        {
            _jump = _playerActions.Player1.Jump;
            _move = _playerActions.Player1.Move;
            _fire = _playerActions.Player1.Fire;
        } else if (playerNum == PlayerNumber.Player2)
        {
            _jump = _playerActions.Player2.Jump;
            _move = _playerActions.Player2.Move;
            _fire = _playerActions.Player2.Fire;
        } else
        {
            throw new Exception("The controls for one or more players have not been mapped properly!"); // This error catches any unexpected exception from the above player control conditions
        }
    }

    private void OnEnable()
    {
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        FrameInput = GatherInput();
    }

    FrameInput GatherInput()
    {
        return new FrameInput
        {
            Move = _move.ReadValue<Vector2>(), // Movement is based on horizontal input
            Jump = _jump.WasPressedThisFrame(), // Rather than any vertical input, the up button initiates a simple command upon pressing
            Fire = _fire.IsPressed() // The player shoots as long as the fire button is pressed
            // FUTURE: Have the option to configure the fire command between classic (Single Fire) and normal (Rapid Fire) from the pause menu
        };
    }

}

// The player's inputs will be gathered every frame from the player's main script. 
public struct FrameInput // This method is public to provide the main player script a way to get the input
{
    public Vector2 Move;
    public bool Jump;
    public bool Fire;
}
