using UnityEngine;

public class UIController : MonoBehaviour {
    public static UIController instance;
    public static DialogoController dialogo {
        get {
            return instance.dialogoController;
        }
    }
    public DialogoController dialogoController;

    void Awake() {
        instance = this;
    }
}
