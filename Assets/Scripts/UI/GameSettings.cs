using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private GameData _gameData;

    [SerializeField] private SliderController _piecesSlider;

    public void UpdateRingsNumber(float value)
    {
        int number = Mathf.RoundToInt(value);

        _gameData.UpdateRingsAmount(number);

        int boardNodesCount = number * 8 + (_gameData.IsMiddlePositionEnabled ? 1 : 0);

        int piecesPerPlayer = (boardNodesCount / 2) - 1;
        int currentPiecesAmount = Mathf.Min(piecesPerPlayer, Mathf.RoundToInt(_piecesSlider.Slider.value));

        _piecesSlider.UpdateSliderValues(3, piecesPerPlayer, currentPiecesAmount);
    }

    public void UpdatePiecesNumber(float value)
    {
        int number = Mathf.RoundToInt(value);

        _gameData.UpdateInitialPiecesAmount(number);
    }

    public void UpdateEnableDiagonals(bool enable)
    {
        _gameData.EnableBoardDiagonalLines(enable);
    }

    public void UpdateEnableCenter(bool enable)
    {
        _gameData.EnableCenterNode(enable);

        int number = _gameData.RingsAmount;

        int boardNodesCount = number * 8 + (enable ? 1 : 0);

        int piecesPerPlayer = (boardNodesCount / 2) - 1;
        int currentPiecesAmount = Mathf.Min(piecesPerPlayer, Mathf.RoundToInt(_piecesSlider.Slider.value));

        _piecesSlider.UpdateSliderValues(3, piecesPerPlayer, currentPiecesAmount);
    }

    public void UpdateEnableFlying(bool enable)
    {
        _gameData.EnableFlyingPieces(enable);
    }
}
