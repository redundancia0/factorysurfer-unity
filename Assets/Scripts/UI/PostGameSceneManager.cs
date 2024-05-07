using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PostGameSceneManager : MonoBehaviour
{
    [SerializeField]
    private Button playAgainButton, backToMainButton;

    [SerializeField]
    private TMPro.TextMeshProUGUI distanceText, coinsCollectedText, pointsTotalText, titleText;

    [SerializeField]
    private string distancePhrase, coinsPhrase, pointsPhrase, newRecordPhrase;

    [SerializeField]
    private GameObject userNameAndImage;

    private void Awake()
    {
        if (Login.USER_DATA != null)
            userNameAndImage.gameObject.SetActive(true);

        SetUpButtons();
    }

    private void Start()
    {
        SetUpScore();
    }


    private void SetUpButtons()
    {
        playAgainButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.AddListener(
            delegate ()
            {
                GameManager.Instance.LoadScene(SCENE.GAME);
            });

        backToMainButton.onClick.RemoveAllListeners();
        backToMainButton.onClick.AddListener(
            delegate ()
            {
                GameManager.Instance.LoadScene(SCENE.MAIN);
            });
    }

    private void SetUpScore()
    {
        PlayerPoints pP = GameManager.Instance.GetPlayerPoints();
        distanceText.text = distancePhrase + pP.distance + "km";
        coinsCollectedText.text = coinsPhrase + pP.numCoins;
        pointsTotalText.text = pointsPhrase + pP.totalPoints;

        if (GameManager.Instance.ThereIsNewRecord())
        {
            titleText.text = newRecordPhrase;
        }

        StartCoroutine(Login.OnGameEnd(pP.totalPoints, pP.numCoins));
    }
}
