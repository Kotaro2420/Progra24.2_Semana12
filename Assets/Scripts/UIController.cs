using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI coinsText;

    private void Update()
    {
        hpText.text = "HP: " + GameData.playerHp.ToString();
        coinsText.text = "Coins: " + GameData.playerScore.ToString();
    }
}
