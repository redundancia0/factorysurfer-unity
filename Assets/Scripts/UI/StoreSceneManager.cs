using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSceneManager : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI numCoinsText, armorPriceText, tankPriceText;

    [SerializeField]
    private TMPro.TextMeshProUGUI tankButtonText, armorButtonText;

    [SerializeField]
    private Button birdButton, tankButton, armorButton, backButton;

    [SerializeField]
    private int tankPrice, armorPrice;

    [SerializeField]
    private string buyText, equipText;

    [SerializeField]
    private UserPanelUI userNameAndImage;
    

    private void Awake()
    {
        if (Login.USER_DATA != null)
            userNameAndImage.gameObject.SetActive(true);

        SetUpButton();
    }

    void Start()
    {
        numCoinsText.text = GameManager.Instance.GetCurrentNumOfCoins().ToString();
        armorPriceText.text = armorPrice.ToString();
        tankPriceText.text = tankPrice.ToString();
    }

    private void SetUpButton()
    {
        birdButton.onClick.RemoveAllListeners();
        birdButton.onClick.AddListener(
            delegate()
            {
                GameManager.Instance.SetSkin(SKIN.BIRD);
            });

        /// TANK BUTTON
        if (GameManager.Instance.IsThisSkinUnlock(SKIN.TANK))
            tankButtonText.text = equipText;
        else
            tankButtonText.text = buyText;

        tankButton.onClick.RemoveAllListeners();
        tankButton.onClick.AddListener(
            delegate ()
            {
                if (GameManager.Instance.IsThisSkinUnlock(SKIN.TANK))
                {
                    GameManager.Instance.SetSkin(SKIN.TANK);
                }
                else if (GameManager.Instance.CanBuyIt(tankPrice))
                {
                    GameManager.Instance.UnlockSkin(SKIN.TANK, tankPrice, tankButton, numCoinsText);
                }
            });



        /// ARMOR BUTTON
        if (GameManager.Instance.IsThisSkinUnlock(SKIN.ARMOR))
            armorButtonText.text = equipText;
        else
            armorButtonText.text = buyText;


        armorButton.onClick.RemoveAllListeners();
        armorButton.onClick.AddListener(
            delegate ()
            {
                if (GameManager.Instance.IsThisSkinUnlock(SKIN.ARMOR))
                {
                    GameManager.Instance.SetSkin(SKIN.ARMOR);
                }
                else if (GameManager.Instance.CanBuyIt(armorPrice))
                {
                    GameManager.Instance.UnlockSkin(SKIN.ARMOR, armorPrice, armorButton, numCoinsText);
                }
            });

        backButton.onClick.RemoveAllListeners();
        backButton.onClick.AddListener(
            delegate()
            {
                GameManager.Instance.LoadScene(SCENE.MAIN);
            });
    }
}
