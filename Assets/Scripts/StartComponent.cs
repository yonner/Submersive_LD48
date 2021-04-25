using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartComponent : MonoBehaviour
{
    public GameObject PressStart;

    private bool showWarning = false;
    private bool showWarningBlink = true;
    private float showWarningTimer = .25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        showWarningTimer -= Time.deltaTime;

        if (showWarningBlink)
        {
            PressStart.SetActive(true);

            if (showWarningTimer < 0)
            {
                showWarningBlink = false;
                showWarningTimer = .25f;
            }
        }

        if (!showWarningBlink)
        {
            PressStart.SetActive(false);

            if (showWarningTimer < 0)
            {
                showWarningBlink = true;
                showWarningTimer = .25f;
            }
        }
    }

    public void OnStartGame(InputAction.CallbackContext context)
    {

        SceneManager.LoadScene("__MAIN__", LoadSceneMode.Single);
    }

    public void OnQuitGame(InputAction.CallbackContext context)
    {

        Application.Quit();
    }
}
