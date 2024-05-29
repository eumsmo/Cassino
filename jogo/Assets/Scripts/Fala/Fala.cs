using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Fala_Acao {
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

    public void Executar() {
        Debug.Log("Variavel " + nomeVariavel + " setada para " + GetValor());
        LevelController.instance.SetVariable(nomeVariavel, GetValor());
    }
}

[System.Serializable]
public class Fala {
    [TextArea(3, 10)]
    public string texto;
    public bool autoNext = false;

    public bool hasAcao = false;
    public Fala_Acao acao = null;
}
