using GameJolt.API;
using GameJolt.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShowLogin : MonoBehaviour
{
    void Start()
    {
        GameJoltUI.Instance.ShowSignIn(OnSignIn);
    }

    private void OnSignIn(bool success)
    {
        if (success)
        {
            SceneManager.LoadScene("Lobby");
            Trophies.Unlock(251916);
            PlayerPrefs.SetInt("ClickCount", 0);
        }

        else
        {
            Debug.Log("No se pudo logear");
        }

    }
}