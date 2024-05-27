using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fala {
    public enum Tipo { Fala, Escolha, Acao }

    public Tipo tipo;
    public string texto;
}

[System.Serializable]
public class Falas {
    public Fala[] falas;
}

public class Dialogavel : Interagivel {
    public Falas[] falas;
    public int index = 0;


    public override void Interact() {
        if (index >= falas.Length) index = falas.Length - 1;
        UIController.dialogo.StartDialogo(falas[index].falas);
        index++;
    }
}
