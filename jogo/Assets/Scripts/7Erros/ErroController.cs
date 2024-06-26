using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ErroInfo {
    public Transform spawnAt;
    public GameObject[] hideObjects;
    public GameObject prefab;
    public Tile cleanTile;
    public Direction cleanDirection;
}

public class ErroController : MonoBehaviour {
    
    public List<ErroInfo> erros;
    List<ErroInfo> errosUsados = new List<ErroInfo>();


    ErroInfo currentErro;
    bool cleared = false;
    GameObject erroInstance;

    List<ErroInfo> errosDisponiveis { get {
        List<ErroInfo> list = new List<ErroInfo>();
        foreach (ErroInfo erro in erros) {
            if (!errosUsados.Contains(erro)) list.Add(erro);
        }
        return list;
    }}

    public void GerarErro() {
        List<ErroInfo> disponiveis = errosDisponiveis;

        int randIndex = Random.Range(0, disponiveis.Count);
        currentErro = disponiveis[randIndex];
        cleared = false;

        SpawnErro(currentErro);
        errosUsados.Add(currentErro);
    }

    void SpawnErro(ErroInfo erro) {
        Debug.Log("Erro gerado: " + erro.prefab.name);
        erroInstance = Instantiate(erro.prefab, erro.spawnAt);
        erroInstance.transform.localPosition = Vector3.zero;
        erroInstance.transform.SetParent(erro.spawnAt.parent);

        if (erro.hideObjects != null && erro.hideObjects.Length > 0) {
            foreach (GameObject hide in erro.hideObjects) {
                hide.SetActive(false);
            }
        }
    }

    public void ClearErro() {
        if (erroInstance != null)
            Destroy(erroInstance);

        if (currentErro.hideObjects != null && currentErro.hideObjects.Length > 0) {
            foreach (GameObject hide in currentErro.hideObjects) {
                hide.SetActive(true);
            }
        }
    }

    public bool TryClearAt(Tile tile, Direction dir) {
        if (tile == currentErro.cleanTile && dir == currentErro.cleanDirection) {
            cleared = true;
            ClearErro();
        }

        return cleared;
    }

    public bool WasErroCleared() {
        return cleared;
    }
}
