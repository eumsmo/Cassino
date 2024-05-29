using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInteragivel : MonoBehaviour {
    public Direction direction;
    public System.Action onInteract;

    public void Interact() {
        FPPlayer player = BattleManager.instance.player.GetComponent<FPPlayer>();
        if (player.facingDirection != direction) {
            return;
        }

        if (onInteract != null) {
            onInteract();
        }
    }
}
