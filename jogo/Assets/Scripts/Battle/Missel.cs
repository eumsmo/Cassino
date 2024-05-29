using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missel : MonoBehaviour {
    public int dano;
    public float speed;

    public Transform target;


    // Update is called once per frame
    void Update(){
        if (target == null) {
            Destroy(gameObject);
            return;
        }

        float time = Time.deltaTime * speed;

        // Smoothly rotate towards the target point.
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), time);

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        // check if same target
        if (other.transform == target) {
            // apply damage
            BattleUI.instance.BossTakeDamage(dano);
            Destroy(gameObject);
        }
    }
}
