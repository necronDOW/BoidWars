using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManager_Editor : Editor
{
    private GameManager _target;
    private GameObject _newTeamLeader;
    private Color _newTeamColor;
    private float _newTeamEmissionMultiplier;
    private bool _useCustomColor;

    private void OnEnable()
    {
        _target = (GameManager)target;
        _newTeamLeader = null;
        _newTeamColor = Color.white;
        _newTeamEmissionMultiplier = 0.0f;
        _useCustomColor = false;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GameObject[] selectedObjects = Selection.gameObjects;

        GUILayout.Space(10);
        #region New Team Editor
        EditorGUILayout.LabelField("Create New Team");
        _newTeamLeader = (GameObject)EditorGUILayout.ObjectField("Leader:", _newTeamLeader, typeof(GameObject), true);
        
        _useCustomColor = EditorGUILayout.Toggle("Use Custom Color?", _useCustomColor);
        if (_useCustomColor)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Color");
            _newTeamColor = EditorGUILayout.ColorField(_newTeamColor);
            GUILayout.Label("Emission Multiplier");
            _newTeamEmissionMultiplier = EditorGUILayout.FloatField(_newTeamEmissionMultiplier);
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Team"))
        {
            AddTeam();
        }
        #endregion

        #region Team Editors
        for (int i = 0; i < _target.TeamCount(); i++)
        {
            GUILayout.Space(5);
            _target.teamFoldouts[i] = EditorGUILayout.Foldout(_target.teamFoldouts[i], "Team " + (i + 1) + " (Led by " + _target.GetTeam(i).GetOwner().name + ")");
            if (_target.teamFoldouts[i])
            {
                if (GUILayout.Button("Add Selected to Team"))
                {
                    for (int j = 0; j < selectedObjects.Length; j++)
                    {
                        _target.AddToTeam(selectedObjects[j], i);
                    }
                }

                if (GUILayout.Button("Remove Selected from Team"))
                {
                    for (int j = 0; j < selectedObjects.Length; j++)
                    {
                        _target.GetTeam(i).Unsubscribe(selectedObjects[j]);
                    }
                }

                if (GUILayout.Button("Remove Team"))
                {
                    RemoveTeam(i);
                }
            }
        }
        #endregion

        serializedObject.ApplyModifiedProperties();
    }

    private void AddTeam()
    {
        if (_newTeamLeader)
        {
            if (_useCustomColor)
            {
                _target.AddTeam(_newTeamLeader, _newTeamColor, _newTeamEmissionMultiplier);
            }
            else
            {
                _target.AddTeam(_newTeamLeader);
            }

            _target.teamFoldouts.Add(true);

            _newTeamLeader = null;
            _newTeamColor = Color.white;
            _newTeamEmissionMultiplier = 0.0f;
            _useCustomColor = false;
        }
    }

    private void RemoveTeam(int index)
    {
        if (_target.RemoveTeam(index))
        {
            _target.teamFoldouts.RemoveAt(index);
        }
    }
}
