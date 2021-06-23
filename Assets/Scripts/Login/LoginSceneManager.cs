using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginSceneManager : MonoBehaviour
{
    [SerializeField]
    private InputField userNameInput;

    public void OnClickLogin()
    {
        PlayerPrefs.SetString("Name", userNameInput.text);

        SceneManager.LoadScene(1);
    }

}
