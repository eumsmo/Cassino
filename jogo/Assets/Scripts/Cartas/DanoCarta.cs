using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoCarta : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        // Destroi em 2 segundos
        Destroy(gameObject, 2);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            BattleUI.instance.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
