using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackInfo {
    public string type;
    public string[] variables; // not used on normal attacks
    public string translate; // not used on normal attacks
    public PathDelayAttack[] attack;
}

[System.Serializable]
public class PathDelayAttack {
    public string path;
    public float delay;
}

[System.Serializable]
public class BattleInfo {
    public string version;
    public int[] paths;
    public int[] line;
    public AttackInfo[] attacks;
    
}

public class BattleReader : MonoBehaviour {
    public TextAsset battleFile;
    public BattleInfo battleInfo;
    public BossBattle bossBattle;

    AtaqueSequencial[] ataques;

    void Start() {
        battleInfo = JsonUtility.FromJson<BattleInfo>(battleFile.text);

        ataques = new AtaqueSequencial[battleInfo.line.Length];
        // Line é o que define o próximo ataque
        int i = 0;
        foreach (int index in battleInfo.line) {
            ataques[i] = GetAtaqueSequencial(battleInfo.attacks[index]);
            i++;
        }

        bossBattle.SetAtaqueGroup(ataques);
    }

    public AtaqueSequencial GetAtaqueSequencial(AttackInfo info) {
        if (info.type == "normal") {
            return GetAtaqueNormal(info);
        } else if (info.type == "custom") {
            return GetAtaqueCustom(info);
        }

        return null;
    }

    AtaqueSequencial GetAtaqueNormal(AttackInfo info) {
        int[] ids = new int[info.attack.Length];
        float[] delays = new float[info.attack.Length];

        for (int j = 0; j < info.attack.Length; j++) {
            ids[j] = int.Parse(info.attack[j].path);
            delays[j] = info.attack[j].delay;
        }

        return new AtaqueSequencial(ids, delays);
    }
    AtaqueSequencial GetAtaqueCustom(AttackInfo info) {
        int[] paths;
        Dictionary<string, int> dict = new Dictionary<string, int>();

        int[] ids = new int[info.attack.Length];
        float[] delays = new float[info.attack.Length];

        switch (info.translate) {
            case "random_order":
                int quant = battleInfo.paths.Length;
                quant = Mathf.Min(quant, info.variables.Length); 
                paths = new int[quant];

                for (int i = 0; i < quant; i++) {
                    int index;
                    string variable;
                    int path = battleInfo.paths[i];

                    do {
                        index = Random.Range(0, quant);
                        variable = info.variables[index];
                    } while (dict.ContainsKey(variable));

                    dict.Add(variable, path);
                }
                break;
        }

        for (int j = 0; j < info.attack.Length; j++) {
            ids[j] = dict[info.attack[j].path];
            delays[j] = info.attack[j].delay;
        }

        return new AtaqueSequencial(ids, delays);
    }
}
