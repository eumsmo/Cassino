using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogo {
    public List<Fala> falas;
}

[System.Serializable]
public class ConversaCondicional {
    public Fala_Acao condicoes;
    public List<Dialogo> falas;
    public int index = 0;

    public bool IsAtEnd() {
        return index >= falas.Count;
    }

    public bool PassCondicional() {
        if (!LevelController.instance.ContainsVariable(condicoes.nomeVariavel)) return false;
        object value = LevelController.instance.GetVariable(condicoes.nomeVariavel);
        
        return value.Equals(condicoes.GetValor());
    }
}

public struct ListaIndex {
    public List<Dialogo> falas;
    public int index;
    public bool isDefault;
}

public class Dialogavel : Interagivel {
    public List<ConversaCondicional> condicionais = new List<ConversaCondicional>();
    public List<Dialogo> falas;
    int index = 0;

    public override void Interact() {
        ListaIndex conjunto = GetDialogo();
        List<Dialogo> falas = conjunto.falas;
        int index = conjunto.index;

        if (index >= falas.Count) index = falas.Count - 1;

        UIController.dialogo.StartDialogo(falas[index].falas.ToArray());
        index++;

        if (conjunto.isDefault) {
            this.index = index;
        } else {
            SetIndexCondicional(falas, index);
        }
    }

    public ListaIndex GetDialogo() {
        ConversaCondicional escolhida = null;

        foreach (ConversaCondicional conversa in condicionais) {
            if (conversa.PassCondicional()) {
                if (escolhida == null) escolhida = conversa;
                else if (!conversa.IsAtEnd()) {
                    escolhida = conversa;
                    break;
                }
            }
        }

        ListaIndex listaIndex = new ListaIndex();
    

        if (escolhida != null) {
            listaIndex.falas = escolhida.falas;
            listaIndex.index = escolhida.index;
            listaIndex.isDefault = false;
        } else {
            listaIndex.falas = falas;
            listaIndex.index = index;
            listaIndex.isDefault = true;
        }

        return listaIndex;
    }

    void SetIndexCondicional(List<Dialogo> lista, int index) {
        foreach (ConversaCondicional conversa in condicionais) {
            List<Dialogo> falas = conversa.falas;
            if (falas == lista) conversa.index = index;
        }
    }
}
