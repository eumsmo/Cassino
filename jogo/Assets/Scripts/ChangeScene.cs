using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour {
    public string scene;

    public string triggerName;

    void Start()  {
        StoredVariable porta_entrada = new StoredVariable(triggerName);
        porta_entrada.OnChange((object value) => {
            LevelController.instance.ChangeLevel(scene);
        });
    }
}
