﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CIMCollect.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>:\windows</string>
  <string>:\winnt</string>
  <string>LogiOptions</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection IgnoreFilesInFolders {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["IgnoreFilesInFolders"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>.exe</string>
  <string>.dll</string>
  <string>.js</string>
  <string>.aspx</string>
  <string>.asmx</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection AuditFilesOfType {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["AuditFilesOfType"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>.bin</string>
  <string>.sys</string>
  <string>.cpl</string>
  <string>.dmp</string>
  <string>.hdmp</string>
  <string>.wer</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection IgnoreFilesOfType {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["IgnoreFilesOfType"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfString xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <string>:\moc</string>
  <string>WebKit.resources</string>
  <string>iTunes.Resources</string>
  <string>Apple Application Support</string>
  <string>Mobile Device Support</string>
  <string>AuthKitWin.resources</string>
  <string>lcplugins</string>
  <string>SAP</string>
  <string>LogiOptions</string>
</ArrayOfString>")]
        public global::System.Collections.Specialized.StringCollection IgnoreFolders {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["IgnoreFolders"]));
            }
        }
    }
}
