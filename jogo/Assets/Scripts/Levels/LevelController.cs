using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesSystem {
    public enum Type { INT, FLOAT, STRING, BOOL }

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
    }

    Dictionary<string, Variable> variables = new Dictionary<string, Variable>();
    Dictionary<string, System.Action<object>> watchers = new Dictionary<string, System.Action<object>>();

    void SetVariable(string name, Type type, object value) {
        if (variables.ContainsKey(name)) {
            variables[name] = new Variable(name, type, value);
            if (watchers.ContainsKey(name)) {
                watchers[name](value);
            }
        } else {
            variables.Add(name, new Variable(name, type, value));
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

public class LevelController : MonoBehaviour {
    public static LevelController instance;
    VariablesSystem variables = new VariablesSystem();

    public string levelName;

    void Awake() {
        instance = this;
    }

    public void SetVariable(string name, int value) {
        variables.SetInt(name, value);
    }

    public void SetVariable(string name, float value) {
        variables.SetFloat(name, value);
    }

    public void SetVariable(string name, string value) {
        variables.SetString(name, value);
    }

    public void SetVariable(string name, bool value) {
        variables.SetBool(name, value);
    }

    public void SetVariable(string name, object value) {
        variables.SetVariable(name, value);
    }

    public object GetVariable(string name) {
        return variables.GetVariable(name);
    }

    public bool ContainsVariable(string name) {
        return variables.Contains(name);
    }
    
    public void Watch(string name, System.Action<object> action) {
        variables.Watch(name, action);
    }

    public void Unwatch(string name, System.Action<object> action) {
        variables.Unwatch(name, action);
    }
}
