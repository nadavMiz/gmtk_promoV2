using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class endGame : MonoBehaviour
{
    static public TextMeshPro m_text;

    static string winMessage = "You win!";
    static string loseMessage = "You Lose!";
    static public void endTheGame(bool isWin) 
    {
        m_text.SetText(isWin ? winMessage : loseMessage);
    }
}
