using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangeScene : MonoBehaviour {
    public string scene;

    public string triggerName;

    void Start()  {
        StoredVariable porta_entrada = new StoredVariable(triggerName);
        porta_entrada.OnChange(Change);
    }

    public void Change(object value) {
        LevelController.instance.ChangeLevel(scene);
    }

    void OnDestroy() {
        StoredVariable porta_entrada = new StoredVariable(triggerName);
        porta_entrada.StopChange(Change);
    }
}
