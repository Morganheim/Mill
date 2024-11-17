using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] private PlayerData _myPlayerData;
    [SerializeField] private PlayerData _opponentPlayerData;

    [SerializeField] private TMP_InputField _nameInputField;
    [SerializeField] private TMP_Dropdown _colorDropdown;
    [SerializeField] private Image _piecePreviewImage;

    private void OnEnable()
    {
        TryChangeColor(_colorDropdown.value);
    }

    public void TryChangeColor(int optionIndex)
    {
        //revert to default
        if (_opponentPlayerData.PieceColorIndex == optionIndex)
        {
            _colorDropdown.value = _myPlayerData.PieceColorIndex;
            return;
        }

        //apply color
        _myPlayerData.UpdatePlayerColor(_colorDropdown.options[optionIndex].color, optionIndex);
        _piecePreviewImage.color = _myPlayerData.PieceColor;
    }

    public void TryChangeName(string inputName)
    {
        //on end edit
        if(inputName.Trim() == _opponentPlayerData.PlayerName)
        {
            //can't change name, names are the same
            _nameInputField.text = _myPlayerData.PlayerName;
            return;
        }

        _myPlayerData.UpdatePlayerName(inputName.Trim());
    }
}
