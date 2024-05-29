using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogo {
    public List<Fala> falas;
}

public class Dialogavel : Interagivel {
    public List<Dialogo> falas;
    int index = 0;


    public override void Interact() {
        if (index >= falas.Count) index = falas.Count - 1;

        UIController.dialogo.StartDialogo(falas[index].falas.ToArray());
        index++;
    }
}
