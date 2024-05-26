using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fala {
    public enum Tipo { Fala, Escolha, Acao }

    public Tipo tipo;
    public string texto;
}

public class Dialogavel : Interagivel {
    public Fala[] falas;


    public override void Interact() {
        UIController.dialogo.StartDialogo(falas);
    }
}
