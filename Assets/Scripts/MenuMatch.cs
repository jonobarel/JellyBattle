using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

    /// <summary>
    /// Menu class for starting a match - color choice and game start
    /// </summary>
public class MenuMatch : MenuBase
{    

    
    /// <summary>
    /// used to set the color for the player based on the button clicked
    /// </summary>
    /// <param name="thisButton">Color to assign to the player</param>
    public void SetColor(GameObject thisButton) {
        players = game.players;
        Button button = thisButton.GetComponent<Button>();
        Color color = button.colors.normalColor;
        
        string playerName = thisButton.GetComponentInParent<Canvas>().name;

        players.ForEach(delegate (Player p) {
            if (playerName == p.name) {
                p.playerColor = color;
            }
        });
    }
    public void StartMatch() {
        UnityEngine.Random.InitState((int)(Time.time*1000));
        players = game.players;
        players.ForEach(delegate (Player p) {
            p.GetComponent<PlayerInput>().ActivateInput();
        });
    }
}
