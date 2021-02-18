﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuMatch : MenuBase
{
    public void SetColor(GameObject thisButton) {
        players = game.players;
        Button button = thisButton.GetComponent<Button>();
        Color color = button.colors.normalColor;
        
        string playerName = thisButton.GetComponentInParent<Canvas>().name;

        players.ForEach(delegate (Player p) {
            if (playerName == p.name) {
                p.SetPlayerColor(color);
            }
        });
    }
    public void StartMatch() {
        players = game.players;
        players.ForEach(delegate (Player p) {
            p.GetComponent<PlayerInput>().ActivateInput();
        });
    }
}
