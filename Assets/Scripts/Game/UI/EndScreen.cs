using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public Text endText;

    public void OnClickReturnToLobby()
    {
        SceneManager.LoadScene(0);
    }
}
