using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteragivel : MonoBehaviour {
    public static TileInteragivel ultimoTileInteragivel;
    public Direction direction;
    public System.Action onInteract;

    public bool isFP = true;

    public void Interact() {
        if (isFP) {
            FPPlayer player = BattleManager.instance.player.GetComponent<FPPlayer>();
            if (player.facingDirection != direction) {
                return;
            }
        }
        

        if (onInteract != null) {
            ultimoTileInteragivel = this;
            onInteract();
        }
    }
}
