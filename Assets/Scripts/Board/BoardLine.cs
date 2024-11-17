using UnityEngine;

public class BoardLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private readonly BoardNode[] _boardNodes = new BoardNode[2];

    public void SetupBoardLine(BoardNode firstNode, BoardNode secondNode)
    {
        _boardNodes[0] = firstNode;
        _boardNodes[1] = secondNode;

        firstNode.BoardLines.Add(this);
        secondNode.BoardLines.Add(this);

        _lineRenderer.SetPosition(0, _boardNodes[0].transform.position);
        _lineRenderer.SetPosition(1, _boardNodes[1].transform.position);

        UpdateLineColor();
    }

    public void UpdateLineColor()
    {
        _lineRenderer.startColor = _boardNodes[0].SpriteRenderer.color;
        _lineRenderer.endColor = _boardNodes[1].SpriteRenderer.color;
    }
}
