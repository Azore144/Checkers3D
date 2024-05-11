using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;

public class PieceMovementHandlerNetwork : PieceMovementHandler
{
    public static event Func<PiecePromotionHandler, int, int, bool> OnPieceReachedBackline;
    public override void OnStartAuthority()
    {
        TilesSelectionHandler.OnTileSelected += HandleTileSelected;
    }
    public override void OnStopClient()
    {
        TilesSelectionHandler.OnTileSelected -= HandleTileSelected;
    }
    protected override void Move(Vector3 position, bool nextTurn)
    {
        CMDMove(position, nextTurn);
    }
    protected override void Capture(Vector2Int piecePosition)
    {
        CMDCapture(piecePosition);
    }
    [Command]
    void CMDMove(Vector3 position, bool nextTurn)
    {
        base.Move(position, nextTurn);

    }
    [Command]
    void CMDCapture(Vector2Int piecePosition)
    {
        base.Capture(piecePosition);
    }
    [ClientRpc]
    void RPCPlayAudio()
    {
        base.PlayAudio();
    }
    protected override void PlayAudio()
    {
        RPCPlayAudio();
    }
    protected override void ReachedBackline(Vector2Int newPosition)
    {
        OnPieceReachedBackline?.Invoke(promotionHandler, newPosition.x, newPosition.y);
    }
}
