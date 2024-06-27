using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesSystem {
    public enum Type { INT, FLOAT, STRING, BOOL, TRIGGER }

    class Variable {
        public string name;
        public Type type;
        public object value;

        public Variable(string name, Type type, object value) {
            this.name = name;
            this.type = type;
            this.value = value;
        }

        public Type GetVariableType() {
            return type;
        }

        public object GetValue() {
            return value;
        }

        public int GetInt() {
            return (int)value;
        }

        public float GetFloat() {
            return (float)value;
        }

        public string GetString() {
            return (string)value;
        }

        public bool GetBool() {
            return (bool)value;
        }

        public bool GetTrigger() {
            return false;
        }
    }

    Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
    Dictionary<string, System.Action<object>> watchers = new Dictionary<string, System.Action<object>>();

    void SetVariable(string name, Type type, object value) {
        if (variables.ContainsKey(name)) {
            variables[name] = new Variable(name, type, value);
        } else {
            variables.Add(name, new Variable(name, type, value));
        }

        if (watchers.ContainsKey(name)) {
            if (watchers[name] == null || watchers[name].GetInvocationList().Length == 0) {
                watchers.Remove(name);
            } else
                watchers[name](value);
        }
    }

    public void SetVariable(string name, object value) {
        SetVariable(name, GetObjectType(value), value);
    }

    public void SetInt(string name, int value) {
        SetVariable(name, Type.INT, value);
    }

    public void SetFloat(string name, float value) {
        SetVariable(name, Type.FLOAT, value);
    }

    public void SetString(string name, string value) {
        SetVariable(name, Type.STRING, value);
    }

    public void SetBool(string name, bool value) {
        SetVariable(name, Type.BOOL, value);
    }

    public int GetInt(string name) {
        return variables[name].GetInt();
    }

    public float GetFloat(string name) {
        return variables[name].GetFloat();
    }

    public string GetString(string name) {
        return variables[name].GetString();
    }

    public bool GetBool(string name) {
        return variables[name].GetBool();
    }

    public object GetVariable(string name) {
        return variables[name].GetValue();
    }

    public bool Contains(string name) {
        return variables.ContainsKey(name);
    }

    public void Watch(string name, System.Action<object> action) {
        Debug.Log("Watching " + name);
        if (watchers.ContainsKey(name)) {
            watchers[name] += action;
        } else {
            watchers.Add(name, action);
        }
    }

    public void Unwatch(string name, System.Action<object> action) {
        if (watchers.ContainsKey(name)) {
            watchers[name] -= action;
        }
    }

    public void PrintAll() {
        foreach (KeyValuePair<string, Variable> variable in variables) {
            Debug.Log(variable.Key + ": " + variable.Value.GetValue());
        }
    }

    public static Type GetObjectType(object value) {
        if (value is int) {
            return Type.INT;
        } else if (value is float) {
            return Type.FLOAT;
        } else if (value is string) {
            return Type.STRING;
        } else if (value is bool) {
            return Type.BOOL;
        } else {
            return Type.INT;
        }
    }
}

public class StoredVariable {
    string name;

    public StoredVariable(string name, object value) {
        this.name = name;
        LevelController.instance.SetVariable(name, value);
    }

    public StoredVariable(string name) {
        this.name = name;
    }

    public string GetName() {
        return name;
    }

    public object Get() {
        if (!LevelController.instance.ContainsVariable(name)) return null;
        return LevelController.instance.GetVariable(name);
    }

    public void Set(object value) {
        LevelController.instance.SetVariable(name, value);
    }

    public void OnChange(System.Action<object> action) {
        LevelController.instance.Watch(name, action);
    }

    public void StopChange(System.Action<object> action) {
        LevelController.instance.Unwatch(name, action);
    }
}

public enum Contexto { Global, Level, LastLevel }

public class LevelController : MonoBehaviour {

    public static LevelController instance;
    VariablesSystem global = new VariablesSystem();
    VariablesSystem level = new VariablesSystem();
    VariablesSystem lastLevel = new VariablesSystem();

    void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeLevel(string scene) {
        Debug.Log("Changing level to " + scene);
        lastLevel = level;
        level = new VariablesSystem();
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    public void PrintVariables(Contexto contexto) {
        GetContext(contexto).PrintAll();
    }

    public void SetVariable(string name, object value, Contexto contexto = Contexto.Global) {
        GetContext(contexto).SetVariable(name, value);
    }

    public object GetVariable(string name, Contexto contexto = Contexto.Global) {
        return GetContext(contexto).GetVariable(name);
    }

    public bool ContainsVariable(string name, Contexto contexto = Contexto.Global) {
        return GetContext(contexto).Contains(name);
    }
    
    public void Watch(string name, System.Action<object> action, Contexto contexto = Contexto.Global) {
        GetContext(contexto).Watch(name, action);
    }

    public void Unwatch(string name, System.Action<object> action, Contexto contexto = Contexto.Global) {
        GetContext(contexto).Unwatch(name, action);
    }

    public VariablesSystem GetContext(Contexto contexto) {
        switch (contexto) {
            case Contexto.Global:
                return global;
            case Contexto.Level:
                return level;
            case Contexto.LastLevel:
                return lastLevel;
            default:
                return global;
        }
    }
}
