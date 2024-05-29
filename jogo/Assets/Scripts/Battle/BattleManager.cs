using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {
    public static BattleManager instance;
    public GameObject player;

    void Awake() {
        instance = this;
    }
}
