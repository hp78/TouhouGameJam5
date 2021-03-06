﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace JSAM
{
    /// <summary>
    /// Thank god to brownboot67 for his advice
    /// https://forum.unity.com/threads/custom-editor-not-saving-changes.424675/
    /// </summary>
    [CustomEditor(typeof(AudioManager))]
    public class AudioManagerEditor : Editor
    {
        static bool showSoundLibrary;
        static bool showMusicLibrary;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            AudioManager myScript = (AudioManager)target;

            string editorMessage = myScript.GetEditorMessage();
            if (editorMessage != "")
            {
                EditorGUILayout.HelpBox(editorMessage, MessageType.Info);
            }

            DrawDefaultInspector();

            List<string> options = new List<string>();

            options.Add("None");
            foreach (string s in myScript.GetMusicDictionary().Keys)
            {
                options.Add(s);
            }

            GUIContent content;
            // Potentially Deprecated
            //GUIContent content = new GUIContent("Current Track", "Current music that's playing, will play on start if not \"None\"");
            //
            //string currentTrack = serializedObject.FindProperty("currentTrack").stringValue;
            //
            //if (currentTrack.Equals("") || !options.Contains(currentTrack)) // Default to "None"
            //{
            //    currentTrack = options[EditorGUILayout.Popup(content, 0, options.ToArray())];
            //}
            //else
            //{
            //    currentTrack = options[EditorGUILayout.Popup(content, options.IndexOf(currentTrack), options.ToArray())];
            //}
            //
            //serializedObject.FindProperty("currentTrack").stringValue = currentTrack;

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            if (GUILayout.Button("Re-Generate Audio Library"))
            {
                myScript.GenerateAudioDictionarys();
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add New Sound File"))
            {
                GameObject newSound = new GameObject("NEW AUDIO FILE (RENAME ME)", typeof(AudioFile));
                newSound.transform.parent = myScript.transform.GetChild(0);
                EditorGUIUtility.PingObject(newSound);
                Undo.RegisterCreatedObjectUndo(newSound, "Added new sound file");
            }

            if (GUILayout.Button("Add New Music File"))
            {
                GameObject newMusic = new GameObject("NEW AUDIO FILE (RENAME ME)", typeof(AudioFileMusic));
                newMusic.transform.parent = myScript.transform.GetChild(1);
                EditorGUIUtility.PingObject(newMusic);
                Undo.RegisterCreatedObjectUndo(newMusic, "Added new music file");
            }
            EditorGUILayout.EndHorizontal();

            content = new GUIContent("Sound Library", "Library of all sounds loaded into AudioManager's Sound Dictionary");

            showSoundLibrary = EditorGUILayout.Foldout(showSoundLibrary, content);
            if (showSoundLibrary)
            {
                if (myScript.GetSoundDictionary().Count > 0)
                {
                    string list = "";
                    foreach (string s in myScript.GetSoundDictionary().Keys)
                    {
                        if (list == "") list = s;
                        else list += "\n" + s;
                    }
                    EditorGUILayout.HelpBox(list, MessageType.None);
                }
            }

            content = new GUIContent("Music Library", "Library of all music loaded into AudioManager's Music Dictionary");

            showMusicLibrary = EditorGUILayout.Foldout(showMusicLibrary, content);
            if (showMusicLibrary)
            {
                if (myScript.GetMusicDictionary().Count > 0)
                {
                    string musiks = "";
                    foreach (string m in myScript.GetMusicDictionary().Keys)
                    {
                        if (musiks == "") musiks = m;
                        else musiks += "\n" + m;
                    }
                    EditorGUILayout.HelpBox(musiks, MessageType.None);
                }
            }

            if (myScript.GetMasterVolume() == 0) EditorGUILayout.HelpBox("Note: Master Volume is MUTED!", MessageType.Info);
            if (myScript.GetSoundVolume() == 0) EditorGUILayout.HelpBox("Note: Sound is MUTED!", MessageType.Info);
            if (myScript.GetMusicVolume() == 0) EditorGUILayout.HelpBox("Note: Music is MUTED!", MessageType.Info);
        }
    }
}