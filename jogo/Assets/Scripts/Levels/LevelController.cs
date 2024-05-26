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

        public Type GetType() {
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

    void SetVariable(string name, Type type, object value) {
        if (variables.ContainsKey(name)) {
            variables[name] = new Variable(name, type, value);
        } else {
            variables.Add(name, new Variable(name, type, value));
        }
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
}

public class LevelController : MonoBehaviour {
    public static LevelController instance;
    VariablesSystem variables = new VariablesSystem();

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

    public object GetVariable(string name) {
        return variables.GetVariable(name);
    }
}
