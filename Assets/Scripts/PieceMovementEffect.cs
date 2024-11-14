using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovementEffect : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private readonly Gradient _gradient = new();
    private PlayerData _player;
    private Vector3 _targetPosition;

    public event System.Action OnMoveComplete;

    public void ExecuteMove(PlayerData player, Vector2 startPosition, Vector2 endPosition)
    {
        SetupEffect(player);

        _lineRenderer.enabled = true;

        ResetPositions(startPosition);

        SetGradient();

        _targetPosition = endPosition;

        StartCoroutine(MovePiece());
    }

    private void SetGradient()
    {
        Color.RGBToHSV(_player.PieceColor, out float h, out float s, out float v);

        Color color1 = Color.HSVToRGB(h, s * 0.25f, v);
        Color color2 = Color.HSVToRGB(h, s * 0.6f, v);
        Color color3 = _player.PieceColor;

        GradientColorKey[] colorKeys = new GradientColorKey[3]
        {
            new(color1, 0),
            new(color2, 0.33f),
            new(color3, 1),
        };

        _gradient.SetKeys(colorKeys, _lineRenderer.colorGradient.alphaKeys);
        _lineRenderer.colorGradient = _gradient;
    }

    private void ResetPositions(Vector2 startPosition)
    {
        _lineRenderer.SetPosition(0, startPosition);
        _lineRenderer.SetPosition(1, startPosition);
    }

    private void SetupEffect(PlayerData playerData)
    {
        _player = playerData;
    }

    private IEnumerator MovePiece()
    {
        while (_lineRenderer.GetPosition(1) != _targetPosition)
        {
            yield return null;
            _lineRenderer.SetPosition(1, Vector2.MoveTowards(_lineRenderer.GetPosition(1), _targetPosition, _player.MovementEffectSpeed * Time.deltaTime));
        }
        while (_lineRenderer.GetPosition(0) != _targetPosition)
        {
            yield return null;
            _lineRenderer.SetPosition(0, Vector2.MoveTowards(_lineRenderer.GetPosition(0), _targetPosition, _player.MovementEffectSpeed * Time.deltaTime));
        }

        OnMoveComplete?.Invoke();

        _lineRenderer.enabled = false;
    }
}
