using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceNetwork : NetworkBehaviour
{
    [SyncVar(hook = nameof(HandleOwnerSet))]
    PlayerPiecesHandler owner;
    public override void OnStartServer()
    {
        owner = connectionToClient.identity.GetComponent<PlayerPiecesHandler>();
        Board.Instance.OnPieceCaptured += ServerHandlePieceCapture;
    }
    public override void OnStopServer()
    {
        Board.Instance.OnPieceCaptured -= ServerHandlePieceCapture;
    }
    void HandleOwnerSet(PlayerPiecesHandler oldowner, PlayerPiecesHandler newowner)
    {
        transform.parent = newowner.PiecesParent;
    }
    [Server]
    void ServerHandlePieceCapture(Vector3 capturedPiecePos)
    {
        if (capturedPiecePos != transform.position) return;
        NetworkServer.Destroy(gameObject);
    }
}
