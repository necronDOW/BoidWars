using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Painter))]
public class Painter_Editor : Editor
{
    private Painter _target;
    private Vector3 _surfaceDimensions;
    private bool _scaleOptions = true;
    private float _uiMinClamp = 0.0f, _uiMaxClamp = 10.0f;
    private int _controlID;
    private int _lastControlID;
    private Color _brushColorDefault;
    private Color _brushColorPressed;
    private Color _brushHighlight;
    private Vector3 _lastClick;

    private void OnEnable()
    {
        _target = (Painter)target;
        _lastControlID = GUIUtility.hotControl;
        _controlID = GUIUtility.GetControlID(FocusType.Passive);

        _brushColorDefault = new Color(0, 1, 1, 0.1f);
        _brushColorPressed = new Color(0, 1, 1, 0.2f);
        _brushHighlight = _brushColorDefault;

        _surfaceDimensions = GenerateCollider.GetDimensions(_target.transform);
        _lastClick = Vector3.zero;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUILayout.Space(10);
        EditorGUILayout.ObjectField(serializedObject.FindProperty("prefab"));

        GUILayout.Space(5);
        _target.density = Mathf.Clamp(EditorGUILayout.IntField("Density:", _target.density), 1, 1000);
        _target.brushSize = EditorGUILayout.Slider("Brush Size:", _target.brushSize, 0.1f, 100.0f);
        _target.paintIntensity = EditorGUILayout.Slider("Intensity:", _target.paintIntensity, 0.01f, 100.0f);
        _target.layerMask = LayerMask.GetMask(EditorGUILayout.TextField("Layer Mask:", "Floor"));

        GUILayout.Space(5);
        _scaleOptions = EditorGUILayout.Foldout(_scaleOptions, "Scale Multiplier Range");
        if (_scaleOptions)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Min:");
            _target.minScale = Mathf.Clamp(EditorGUILayout.FloatField(_target.minScale), _uiMinClamp, _target.maxScale);
            GUILayout.Label("Max:");
            _target.maxScale = Mathf.Clamp(EditorGUILayout.FloatField(_target.maxScale), _target.minScale, _uiMaxClamp);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.MinMaxSlider(ref _target.minScale, ref _target.maxScale, _uiMinClamp, _uiMaxClamp);
        }

        GUILayout.Space(5);
        _scaleOptions = EditorGUILayout.Foldout(_scaleOptions, "Color Options");
        if (_scaleOptions)
        {
            _target.randomizeColor = EditorGUILayout.Toggle("Random?", _target.randomizeColor);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(_target.randomizeColor ? "Min:" : "Color:");
            _target.minColor = EditorGUILayout.ColorField(_target.minColor);
            if (_target.randomizeColor)
            {
                GUILayout.Label("Max:");
                _target.maxColor = EditorGUILayout.ColorField(_target.maxColor);
            }
            EditorGUILayout.EndHorizontal();

            if (_target.randomizeColor)
            {
                EditorGUILayout.HelpBox("Randomized colours are not optimized, performance may suffer.", MessageType.Warning);
            }
        }

        GUILayout.Space(5);
        if (GUILayout.Button("Undo"))
        {
            _target.Undo();
        }

        serializedObject.ApplyModifiedProperties();
    }
    
    void OnSceneGUI()
    {
        HandleInput();

        Vector3 brushPosition;
        Vector3 distance = _target.transform.position + _surfaceDimensions;

        if (ExtendedEditor.RaycastFromScreen(out brushPosition, distance, _target.layerMask))
        {
            Handles.color = _brushHighlight;
            Handles.DrawSolidDisc(brushPosition, Vector3.up, _target.brushSize);

            HandleUtility.Repaint();

            if (_brushHighlight == _brushColorPressed)
            {
                if (Vector3.Distance(_lastClick, brushPosition) > _target.paintIntensity)
                {
                    _target.Draw(brushPosition);
                    _lastClick = brushPosition;
                }
            }
        }
    }

    private void HandleInput()
    {
        switch (Event.current.type)
        {
            case EventType.mouseDown:
                if (Event.current.button == 0)
                {
                    _brushHighlight = _brushColorPressed;

                    GUIUtility.hotControl = _controlID;
                    Event.current.Use();
                }
                else
                {
                    GUIUtility.hotControl = _lastControlID;
                }
                break;

            case EventType.mouseUp:
                _brushHighlight = _brushColorDefault;
                break;
        }
    }
}
