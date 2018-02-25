using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MusicScoreMessageBroker
{
    [CustomEditor(typeof(BeatConductor))]
    public class BeatConductorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            var _beatConductor = target as BeatConductor;

            //BPMをグレーアウトにする。
            EditorGUI.BeginDisabledGroup(true);

            _beatConductor.BPM = EditorGUILayout.FloatField("BGM", _beatConductor.BPM);

            EditorGUI.EndDisabledGroup();

            //tickPerBarとMsPerTickをチェックして、値の変更があればBPMに反映する。
            EditorGUI.BeginChangeCheck();

            _beatConductor.TickPerBar = (TickPerBar)EditorGUILayout.EnumPopup("TickPerBar",_beatConductor.TickPerBar);
            _beatConductor.MsPerTick = EditorGUILayout.IntSlider("MsPerTick", _beatConductor.MsPerTick,1,200);

            if (EditorGUI.EndChangeCheck())
            {
                _beatConductor.BPM = ((int)_beatConductor.TickPerBar * _beatConductor.MsPerTick * (float)60) / 1000;
            }

        }

    }
}