﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ui {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.4.0.0")]
    internal sealed partial class UiSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static UiSettings defaultInstance = ((UiSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new UiSettings())));
        
        public static UiSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Your Copyright Message here")]
        public string Message {
            get {
                return ((string)(this["Message"]));
            }
            set {
                this["Message"] = value;
            }
        }
    }
}