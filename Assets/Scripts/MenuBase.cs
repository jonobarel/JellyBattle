using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
///  Base class for game menus
/// </summary>
public class MenuBase : MonoBehaviour
{
    [Header("General Settings")]
    public GameConfig config;
    public GameController game;

    [SerializeField] protected List<Player> players;

    public void HideMenu(Canvas screen) {
        CanvasGroup menu = screen.GetComponent<CanvasGroup>();
        menu.alpha = Mathf.Lerp(1f, 0f, 1f);
        menu.blocksRaycasts = false;
    }
    public void ShowMenu(Canvas screen) {
        CanvasGroup menu = screen.GetComponent<CanvasGroup>();
        menu.alpha = Mathf.Lerp(0f, 1f, 1f);
        menu.blocksRaycasts = true;
    }

}
