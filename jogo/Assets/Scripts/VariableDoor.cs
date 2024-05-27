using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDoor : MonoBehaviour {
    public string variableName;
    public bool activateOnTrue = true;
    public bool startActive = false;


    void Start() {
        bool initialStatus = startActive;
        if (!activateOnTrue) initialStatus = !initialStatus;


        StoredVariable porta_entrada = new StoredVariable(variableName, initialStatus);
        porta_entrada.OnChange((object value) => {
            Debug.Log("Porta " + variableName + " mudou para " + value);
            StatusChanged((bool)value);
        });
    }

    public void StatusChanged(bool status) {
        bool active = status == activateOnTrue;
        gameObject.SetActive(!active);
    }
}
