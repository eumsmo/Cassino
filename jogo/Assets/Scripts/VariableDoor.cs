using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDoor : MonoBehaviour {
    public string variableName;
    public bool activateOnTrue = true;


    void Start() {
        StoredVariable porta_entrada = new StoredVariable(variableName);
        porta_entrada.OnChange(StatusChange);

        if (porta_entrada.Get() == null)
            StatusChanged(false);
        else
            StatusChanged((bool)porta_entrada.Get());
    }

    public void StatusChange(object value) {
        StatusChanged((bool)value);
    }

    public void StatusChanged(bool status) {
        if (gameObject == null) return;

        bool active = status == activateOnTrue;
        gameObject.SetActive(!active);
    }

    void OnDestroy() {
        StoredVariable porta_entrada = new StoredVariable(variableName);
        porta_entrada.StopChange(StatusChange);
    }
}
