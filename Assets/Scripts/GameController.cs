﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Class used for managing all aspects of gameplay: spawning players, triggering menus on victory.
/// In future iterations this class would be extended to support additional game modes.
/// </summary>

public class GameController : MonoBehaviour
{
    public MultipleTargetCamera gameCamera;
    public List<Player> players;
    protected GameController gameController;
    public ArenaController arena;

    [Header("General Settings")]
    public GameConfig config;
    public GameObject MenuScreen;

    [SerializeField] protected Player playerPrefab;

    [Header("Player 1 Settings")]
    [SerializeField] protected Transform player1Spawn;
    //[SerializeField] protected Color player1Color;

    [Header("Player 2 Settings")]
    [SerializeField] protected Transform player2Spawn;
    //[SerializeField] protected Color player2Color;

    private Player player1;
    private Player player2;

    void Start() {
        gameController = GetComponent<GameController>();
        players = new List<Player>(2);
        Color player1Color = config.Player1DefaultColor;
        Color player2Color = config.Player2DefaultColor;

        // Spawn player 1
        AddPlayer("Player1", player1Spawn, player1Color, Player.FaceRight);

        // Spawn player 2
        AddPlayer("Player2", player2Spawn, player2Color, Player.FaceLeft);
    }

    /// <summary>
    /// Add a player to the game. Used to streamline player spawning and for future expansion
    /// </summary>
    /// <param name="name">name assigned to Player object</param>
    /// <param name="spawnPoint">Player spawn point</param>
    /// <param name="color">Color override for player skin</param>
    protected void AddPlayer(string name, Transform spawnPoint, Color color, int facingDirection) {
        Player p = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        p.game = gameController;
        p.name = name;
        p.SetFacing(facingDirection);
        gameCamera.Targets.Add(p.transform);
        p.playerColor = color;
        p.GetComponent<PlayerInput>().SwitchCurrentControlScheme(name);
        p.GetComponent<PlayerInput>().DeactivateInput();
        players.Add(p);

    }
    //public void StartMatch() {
    //    players.ForEach(delegate (Player p) {
    //        p.GetComponent<PlayerInput>().ActivateInput();
    //    });
    //}

    public void PowerUpTaken() {
        arena.PowerUpTaken();
    }
    public void PlayerHit(Player target, Player shooter) {

    }

    public void Victory(string name) {
        players.ForEach(delegate (Player p) {
            p.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            p.GetComponent<PlayerInput>().DeactivateInput();
        });

        CanvasGroup menuMain = MenuScreen.GetComponent<CanvasGroup>();
        menuMain.alpha = Mathf.Lerp(0f, 1f, 1f);
        menuMain.blocksRaycasts = true;

        CanvasGroup menuVictory = MenuScreen.transform.Find("MenuVictory").GetComponent<CanvasGroup>();
        menuVictory.alpha = Mathf.Lerp(0f, 1f, 1f);
        menuVictory.blocksRaycasts = true;

        Text victoryLabel = MenuScreen.transform.Find("MenuVictory").GetComponentInChildren<Text>();

        victoryLabel.text = name + " defeated!";
    }

}
