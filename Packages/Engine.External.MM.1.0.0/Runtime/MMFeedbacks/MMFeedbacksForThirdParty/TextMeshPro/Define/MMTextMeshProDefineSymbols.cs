﻿using System.Collections.Generic;
using System.Linq;
using UnityEditor;

#if UNITY_EDITOR || UNITY_STANDALONE
namespace MMFeedbacks {
    /// <summary>
    /// This class lets you specify (in code, by editing it) symbols that will be added to the build settings' define symbols list automatically
    /// </summary>
    [InitializeOnLoad]
    public class MMTextMeshProDefineSymbols
    {
        /// <summary>
        /// A list of all the symbols you want added to the build settings
        /// </summary>
        public static readonly string[] Symbols = new string[] 
        {
            "MOREMOUNTAINS_TEXTMESHPRO_INSTALLED"
        };

        /// <summary>
        /// As soon as this class has finished compiling, adds the specified define symbols to the build settings
        /// </summary>
        static MMTextMeshProDefineSymbols()
        {
            string scriptingDefinesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup); 
            List<string> scriptingDefinesStringList = scriptingDefinesString.Split(';').ToList();
            scriptingDefinesStringList.AddRange(Symbols.Except(scriptingDefinesStringList));
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", scriptingDefinesStringList.ToArray()));
        }
    }
}
#endif