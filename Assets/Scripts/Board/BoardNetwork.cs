using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BoardNetwork : Board
{
    readonly SyncList<int[]> boardList = new SyncList<int[]>();
    public override IList<int[]> BoardList 
    {
        get
        {
            return boardList;
        }
    }
    public override event Action<Vector3> OnPieceCaptured;
    public override void OnStartServer()
    {
        FillBoardList(boardList);
        PieceMovementHandlerNetwork.OnPieceReachedBackline += TryPromotePieceOnBoard;
    }
    public override void OnStopServer()
    {
        PieceMovementHandlerNetwork.OnPieceReachedBackline -= TryPromotePieceOnBoard;
    }
    [Server]
    public override void MoveOnBoard( Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        MoveOnBoard(boardList, oldPosition, newPosition);
        RPCMoveOnBoard(oldPosition, newPosition, nextTurn);
    }
    [Server]
    public override void CaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList, piecePosition);
        RPCCaptureOnBoard(piecePosition);
        OnPieceCaptured?.Invoke(new Vector3(piecePosition.x, 0, piecePosition.y));
    }
    [ClientRpc]
    void RPCMoveOnBoard(Vector2Int oldPosition, Vector2Int newPosition, bool nextTurn)
    {
        if(NetworkServer.active)
        {
            return;
        }
        MoveOnBoard(boardList, oldPosition, newPosition);
        if(nextTurn) 
        {
            NetworkClient.connection.identity.GetComponent<PlayerNetwork>().CMDNextTurn();
        }
    }
    [ClientRpc]
    void RPCCaptureOnBoard(Vector2Int piecePosition)
    {
        Capture(boardList, piecePosition);
    }
    [Server]
    bool TryPromotePieceOnBoard(PiecePromotionHandler promotedPiece, int x, int z)
    {
        PromotePieceOnBoard(boardList, x, z);
        RPCPromotePieceOnBoard(x,z);
        return true;
    }
    [ClientRpc]
    void RPCPromotePieceOnBoard(int x, int z)
    {
        if(NetworkServer.active) 
        {
            return;
        }
        PromotePieceOnBoard(boardList, x, z);
    }
}
