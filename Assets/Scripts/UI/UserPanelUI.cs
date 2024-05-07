using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserPanelUI : MonoBehaviour
{
    [SerializeField]
    private Image _userImage;
    [SerializeField]
    private TMP_Text _userNameText;

    public Image UserImage {  get { return _userImage; } }
    public TMP_Text UserNameText { get { return _userNameText; } }

    private void OnEnable()
    {
        if(Login.USER_DATA != null)
        {
            _userImage.sprite = Login.UserAvatar;
            _userNameText.text = Login.USER_DATA.nombre;
        } 
    }
}
