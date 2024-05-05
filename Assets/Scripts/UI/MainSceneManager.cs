using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button playButton, exitButton, storeButton, logButton, backButton, enterLog, recordButton, recordsBackButton;

    [SerializeField]
    private GameObject _loadingImage;

    [SerializeField]
    private TMPro.TextMeshProUGUI logginText;

    [SerializeField]
    private GameObject logPanel, mainButtonsPanel, recordPanel, recordPrefab;

    [SerializeField]
    private TMPro.TMP_InputField userInputField, passwordInputField;

    [SerializeField]
    private string recordPhrase, errorLogPhrase;

    [SerializeField]
    private Transform poolRecord;

    public UnityEvent OnLoginSucces, OnLoginDenied;

    private void Awake()
    {
        passwordInputField.contentType = TMPro.TMP_InputField.ContentType.Password;
        logPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(
            delegate ()
            {
                GameManager.Instance.LoadScene(SCENE.GAME);
            });

        storeButton.onClick.RemoveAllListeners();
        storeButton.onClick.AddListener(
            delegate ()
            {
                GameManager.Instance.LoadScene(SCENE.STORE);
            });

        logButton.onClick.RemoveAllListeners();
        logButton.onClick.AddListener(
            delegate ()
            {
                logButton.gameObject.SetActive(false);
                mainButtonsPanel.SetActive(false);
                exitButton.gameObject.SetActive(false);
                logPanel.SetActive(true);
            });

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(
            delegate ()
            {
                mainButtonsPanel.SetActive(true);
                logPanel.SetActive(false);
            });

        enterLog.onClick.RemoveAllListeners();
        enterLog.onClick.AddListener(
            delegate ()
            {
                if (ThisUserExist(userInputField.text) && CorrectPassword(userInputField.text, passwordInputField.text)) 
                {
                    LogUser(userInputField.text, passwordInputField.text);
                    _loadingImage.SetActive(true);
                }
                else
                {
                    logginText.text = errorLogPhrase;
                }
            });

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(
            delegate ()
            {
                GameManager.Instance.CloseApp();
            });

        recordButton.onClick.RemoveAllListeners();
        recordButton.onClick.AddListener(
            delegate ()
            {
                mainButtonsPanel.SetActive(false);
                recordPanel.SetActive(true);
                recordsBackButton.onClick.RemoveAllListeners();
                recordsBackButton.onClick.AddListener(
                    delegate()
                    {
                        mainButtonsPanel.SetActive(true);
                        recordPanel.SetActive(false);
                        foreach (Transform child in transform)
                        {
                            Destroy(child);
                        }
                    });

            });
        OnLoginSucces.AddListener(() => {
            playButton.gameObject.SetActive(true);
            storeButton.gameObject.SetActive(true);
            recordButton.gameObject.SetActive(true);
            logButton.gameObject.SetActive(false);
            exitButton.gameObject.SetActive(true);
            _loadingImage.SetActive(false);
        });

        OnLoginDenied.AddListener(() => {
            Debug.LogWarning("Acceso denegado al loguearse");
            _loadingImage.SetActive(false);
            logButton.gameObject.SetActive(true);
            exitButton.gameObject.SetActive(true);
        });

        _loadingImage.SetActive(false);
    }

    private void OnEnable()
    {
        if(Login.USER_DATA != null)
        {
            playButton.gameObject.SetActive(true);
            storeButton.gameObject.SetActive(true);
            recordButton.gameObject.SetActive(true);
            logButton.gameObject.SetActive(false);
        }
    }

    private bool ThisUserExist(string _userName)
    {
        if (_userName != "")
        {
            print(_userName);
            return true;
        }
        else
        {
            return false;
        }

    }

    // log del usuario
    private void LogUser(string _user, string _password)
    {
        logPanel.SetActive(false);
        mainButtonsPanel.SetActive(true);
        StartCoroutine(LoginButton(_user, _password));
    }

    private bool CorrectPassword(string _userName, string _password)
    {
        return true;
    }


    private IEnumerator LoginButton(string user, string password)
    {
        yield return StartCoroutine(Login.SendLoginRequest(user, password));
        if (Login.USER_DATA == null)
            OnLoginDenied?.Invoke();
        else
            OnLoginSucces?.Invoke();
    }
}
