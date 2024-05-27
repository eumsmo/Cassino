using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Dialogavel))]
public class DialogavelEditor : Editor {
    Dialogavel dialogavel;
    int aux_index;
    int fala_a_remover = -1, dialogo_fala_a_remover = -1;

    void OnEnable() {
        dialogavel = (Dialogavel)target;
    }

    public override void OnInspectorGUI() {
        // Draw default inspector
        DrawDefaultInspector();

        Undo.RecordObject(dialogavel, "Dialogavel");
        EditorGUI.BeginChangeCheck();

        // Draw custom inspector
        EditorGUILayout.Space();

        if (dialogavel.falas == null || dialogavel.falas.Count == 0) {
            if (GUILayout.Button("Criar novo dialogo")) {
                Falas falas = new Falas();
                if (dialogavel.falas == null) {
                    dialogavel.falas = new List<Falas>() { falas };
                } else {
                    dialogavel.falas.Add(falas);
                }
            }
        } else {
            DrawFalas();
        }

        // Remove fala se clicar no botao de remover
        if (fala_a_remover > -1) {
            if (dialogavel.falas[dialogo_fala_a_remover].falas.Count == 1) {
                dialogavel.falas[dialogo_fala_a_remover] = null;
            } else {
                dialogavel.falas[dialogo_fala_a_remover].falas.RemoveAt(fala_a_remover);
            }

            fala_a_remover = -1;
            dialogo_fala_a_remover = -1;
        }

        if (EditorGUI.EndChangeCheck()){
            EditorUtility.SetDirty(dialogavel);
            EditorSceneManager.MarkSceneDirty(dialogavel.gameObject.scene);
        }
    }

    void DrawFalas() {
        EditorGUILayout.LabelField("Falas:", EditorStyles.boldLabel);
        GUILayout.BeginVertical("box");

        for (int i = 0; i < dialogavel.falas.Count; i++) {
            GUILayout.BeginVertical("HelpBox");
            EditorGUILayout.LabelField("Conjunto " + i, EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (dialogavel.falas[i] != null && dialogavel.falas[i].falas != null && dialogavel.falas[i].falas.Count > 0) {
                for (int j = 0; j < dialogavel.falas[i].falas.Count; j++) {
                    EditorFalas fala = dialogavel.falas[i].falas[j];
                    DrawFala(fala, j, i);
                }
            } else {
                EditorGUILayout.LabelField("Nenhuma fala", EditorStyles.boldLabel);
                EditorGUILayout.Space();
            }


            if (GUILayout.Button("Adicionar fala")) {
                aux_index = i;

                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Fala"), false, () => AddFala(aux_index, FalaType.Tipo.Fala));
                menu.AddItem(new GUIContent("Escolha"), false, () => AddFala(aux_index, FalaType.Tipo.Escolha));
                menu.AddItem(new GUIContent("Acao"), false, () => AddFala(aux_index, FalaType.Tipo.Acao));
                menu.ShowAsContext();
            }

            GUILayout.EndVertical();
        }

        GUILayout.EndVertical();
    }

    void DrawFala(EditorFalas falaE, int falaIndex = 0, int dialogoIndex = 0) {
        EditorGUILayout.BeginVertical("HelpBox");

        FalaType fala = falaE.GetFala();

        string titulo = "";

        switch (fala.tipo) {
            case FalaType.Tipo.Nada:
                break;
            case FalaType.Tipo.Fala:
                titulo = "Fala";
                break;
            case FalaType.Tipo.Escolha:
                titulo = "Escolha";
                break;
            case FalaType.Tipo.Acao:
                titulo = "Acao";
                break;
        }

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(titulo + " [id: " + falaIndex + "]", EditorStyles.boldLabel);
    
        if (GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20))) {
            fala_a_remover = falaIndex;
            dialogo_fala_a_remover = dialogoIndex;
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();


        fala.autoNext = EditorGUILayout.Toggle("Auto Next", fala.autoNext);

        switch (fala.tipo) {
            case FalaType.Tipo.Nada:
                break;
            case FalaType.Tipo.Fala:
                Fala f = falaE.fala;
                f.texto = EditorGUILayout.TextField("Texto", f.texto);
                break;
            case FalaType.Tipo.Escolha:
                break;
            case FalaType.Tipo.Acao:
                FalaAcao acao = falaE.acao;
                acao.acao = (FalaAcao.Acao)EditorGUILayout.EnumPopup("Acao", acao.acao);

                switch (acao.acao) {
                    case FalaAcao.Acao.SetVariable:
                        FalaAcaoSetVariable setVariable = (FalaAcaoSetVariable)acao;
                        setVariable.nomeVariavel = EditorGUILayout.TextField("Nome Variavel", setVariable.nomeVariavel);
                        setVariable.tipoVariavel = (VariablesSystem.Type)EditorGUILayout.EnumPopup("Tipo Variavel", setVariable.tipoVariavel);

                        switch (setVariable.tipoVariavel) {
                            case VariablesSystem.Type.INT:
                                setVariable.valorInt = EditorGUILayout.IntField("Valor", setVariable.valorInt);
                                break;
                            case VariablesSystem.Type.FLOAT:
                                setVariable.valorFloat = EditorGUILayout.FloatField("Valor", setVariable.valorFloat);
                                break;
                            case VariablesSystem.Type.STRING:
                                setVariable.valorString = EditorGUILayout.TextField("Valor", setVariable.valorString);
                                break;
                            case VariablesSystem.Type.BOOL:
                                setVariable.valorBool = EditorGUILayout.Toggle("Valor", setVariable.valorBool);
                                break;
                        }
                        break;
                }
                break;
        }

        EditorGUILayout.EndVertical();
    }

    void AddFala(int dialogoIndex, FalaType.Tipo tipo) {
        if (dialogavel.falas[dialogoIndex] == null) {
            dialogavel.falas[dialogoIndex] = new Falas();
        }

        if (dialogavel.falas[dialogoIndex].falas == null) {
            dialogavel.falas[dialogoIndex].falas = new List<EditorFalas>();
        }

        switch (tipo) {
            case FalaType.Tipo.Fala:
                EditorFalas fala = new EditorFalas(new Fala());
                dialogavel.falas[dialogoIndex].falas.Add(fala);
                break;
            case FalaType.Tipo.Escolha:
                break;
            case FalaType.Tipo.Acao:
                EditorFalas acao = new EditorFalas(new FalaAcaoSetVariable());
                dialogavel.falas[dialogoIndex].falas.Add(acao);
                break;
        }

    }
}
