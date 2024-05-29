using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDoor : MonoBehaviour {
    public string variableName;
    public bool activateOnTrue = true;


    void Start() {
        StoredVariable porta_entrada = new StoredVariable(variableName);
        porta_entrada.OnChange((object value) => {
            StatusChanged((bool)value);
        });

        if (porta_entrada.Get() == null)
            StatusChanged(false);
        else
            StatusChanged((bool)porta_entrada.Get());
    }

    public void StatusChanged(bool status) {
        bool active = status == activateOnTrue;
        gameObject.SetActive(!active);
    }
}
