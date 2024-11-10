using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardLine : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;

    private BoardNode[] _boardNodes = new BoardNode[2];

    public void SetupBoardLine(BoardNode firstNode, BoardNode secondNode)
    {
        _boardNodes[0] = firstNode;
        _boardNodes[1] = secondNode;

        _lineRenderer.SetPosition(0, _boardNodes[0].transform.position);
        _lineRenderer.SetPosition(1, _boardNodes[1].transform.position);
    }
}
