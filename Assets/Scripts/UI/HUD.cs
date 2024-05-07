using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class HUDActions
{
    public static Action AddCoin;
    public static Action<float> UpdateDistance;
}

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI numCoinsText;

    [SerializeField]
    private TMPro.TextMeshProUGUI distanceText;

    [SerializeField]
    private GameObject userNameAndImage;
    private Image userImage;
    private TMP_Text userNameText;

    private int coinNum = 0;
    private float distance = 0.0f;

    [SerializeField]
    private List<Sprite> bgs;

    [SerializeField]
    private SpriteRenderer currentBG;

    [SerializeField]
    private int distanceToChangeBG;

    private int currentBgCounter = 0;

    private bool blocker = false;

    private void OnEnable()
    {
        HUDActions.AddCoin += AddCoin;
        HUDActions.UpdateDistance += UpdateDistance;

        if(Login.USER_DATA != null)
        {
            userImage = userNameAndImage.GetComponentInChildren<Image>();
            userNameText = userNameAndImage.GetComponentInChildren<TMP_Text>();
            userNameAndImage.gameObject.SetActive(true);
            userImage.sprite = Login.UserAvatar;
            userNameText.text = Login.USER_DATA.nombre;
        }     
    }

    private void OnDisable()
    {
        HUDActions.AddCoin -= AddCoin;
    }

    private void AddCoin()
    {
        coinNum++;
        numCoinsText.text = "X " + coinNum;
    }

    private void UpdateDistance(float _distance)
    {
        distance = MathF.Floor(_distance);
        distanceText.text = distance.ToString() + "m";
        if (!blocker && distance > distanceToChangeBG && distance % distanceToChangeBG == 0)
        {
            NextBG();
        }
    }

    private void ActiveBlocker()
    {
        blocker = false;
    }

    private void OnDestroy()
    {
        GameManager.Instance.SetPlayerPoints(distance, coinNum);
    }

    private void NextBG()
    {
        blocker = true;
        currentBgCounter++;
        if (currentBgCounter >= bgs.Count)
        {
            currentBgCounter = 0;   
        }
        currentBG.sprite = bgs[currentBgCounter];
        Invoke(nameof(ActiveBlocker), 2);
    }
}
