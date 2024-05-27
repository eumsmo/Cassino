using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FalaType {
    public enum Tipo { Nada, Fala, Escolha, Acao, Conversor }
    public Tipo tipo;
    public bool autoNext = false;
}

[System.Serializable]
public class FalaNada: FalaType {
    public FalaNada() {
        tipo = Tipo.Nada;
    }
}

[System.Serializable]
public class Fala: FalaType {
    public string texto;

    public Fala() {
        tipo = Tipo.Fala;
    }
}

[System.Serializable]
public abstract class FalaAcao: FalaType {
    public enum Acao { Nada, SetVariable }
    public Acao acao;
    
    public abstract void Executar();
}

[System.Serializable]
public class FalaAcaoSetVariable: FalaAcao {
    public VariablesSystem.Type tipoVariavel;
    public string nomeVariavel;
    public bool valorBool;
    public int valorInt;
    public float valorFloat;
    public string valorString;

    public object GetValor() {
        switch (tipoVariavel) {
            case VariablesSystem.Type.INT:
                return valorInt;
            case VariablesSystem.Type.FLOAT:
                return valorFloat;
            case VariablesSystem.Type.STRING:
                return valorString;
            case VariablesSystem.Type.BOOL:
                return valorBool;
            default:
                return null;
        }
    }

    public FalaAcaoSetVariable() {
        tipo = Tipo.Acao;
        acao = Acao.SetVariable;
    }

    public override void Executar() {
        Debug.Log("Variavel " + nomeVariavel + " setada para " + GetValor());
        LevelController.instance.SetVariable(nomeVariavel, GetValor());
    }
}

[System.Serializable]
public class EditorFalas: FalaType {
    public Fala fala;
    public FalaAcaoSetVariable acao;

    public FalaType.Tipo tipoI;

    public EditorFalas(Fala fala) {
        this.fala = fala;
        this.acao = null;
        tipo = FalaType.Tipo.Conversor;
        tipoI = FalaType.Tipo.Fala;
    }

    public EditorFalas(FalaAcaoSetVariable acao) {
        this.acao = acao;
        this.fala = null;
        tipo = FalaType.Tipo.Conversor;
        tipoI = FalaType.Tipo.Acao;
    }

    public FalaType GetFala() {
        if (tipoI == FalaType.Tipo.Fala) {
            return fala;
        } else if (tipoI == FalaType.Tipo.Acao) {
            return acao;
        }
        return null;
    }
}

[System.Serializable]
public class Falas {
    public List<EditorFalas> falas;
}



public class Dialogavel : Interagivel {
    [HideInInspector]
    public List<Falas> falas;
    int index = 0;


    public override void Interact() {
        if (index >= falas.Count) index = falas.Count - 1;

        UIController.dialogo.StartDialogo(falas[index].falas.ToArray());
        index++;
    }
}
