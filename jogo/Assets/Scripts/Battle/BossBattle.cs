using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {
    public int id;
    public float delay;
}

public interface IAttacker {
    Attack? GetNext();
}

public class AtaqueSequencial : IAttacker {
    public int[] ids;
    public float[] delays;
    protected int index = 0;

    public AtaqueSequencial(int[] ids, float[] delays) {
        this.ids = ids;
        this.delays = delays;
    }

    public AtaqueSequencial(string sequencia, float delay) {
        ids = new int[sequencia.Length];
        delays = new float[sequencia.Length];
        int i = 0;

        foreach (char caracter in sequencia) {
            ids[i] = int.Parse(caracter + "");
            delays[i] = delay;
            i++;
        }
    }

    public Attack? GetNext() {
        if (index >= ids.Length) return null;
        
        Attack ataque = new Attack();
        ataque.id = ids[index];
        ataque.delay = delays[index];
        index++;
        return ataque;
    }

    public void Reset() { 
        index = 0;
    }
}

public class BossBattle : MonoBehaviour {
    public AtaqueSequencial ataqueGroup;
    public PathPoint path;

    float timer = 0;
    Attack? currentAttack;

    void Start() {
        ataqueGroup = new AtaqueSequencial("12345", 0.5f);
    }

    void FixedUpdate() {
        if (ataqueGroup == null) return;
        if (currentAttack == null) {
            currentAttack = ataqueGroup.GetNext();
    
            if (currentAttack == null) {
                ataqueGroup = null;
                return;
            }

            timer = currentAttack.delay;
        }

        if (timer > 0) {
            timer -= Time.fixedDeltaTime;
        } else {
            timer = 0;
            Atacar();
        }
    }

    public void Atacar() {
        if (currentAttack == null) return;

        PathPoint point = path.GetPath(currentAttack.id - 1);
        Debug.Log(point);
        Debug.Log(currentAttack.id);

        currentAttack = null;
    }
}