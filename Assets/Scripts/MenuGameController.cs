using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /// <summary>
    /// derived class for a GameController to run during the main menu.
    /// </summary>
public class MenuGameController : GameController
{

    void Start() {
        gameController = GetComponent<GameController>();
        players = new List<Player>(2);
        Color player1Color = config.Player1DefaultColor;
        Color player2Color = config.Player2DefaultColor;

        // Spawn player 1
        AddPlayer("blank", player1Spawn, player1Color, Player.FaceRight);

        // Spawn player 2
        AddPlayer("blank", player2Spawn, player2Color, Player.FaceLeft);

    }
}
