﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BassUtils.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BassUtils.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The directory {0} specified in parameter {1} does not exist..
        /// </summary>
        internal static string ArgVal_DirectoryDoesNotExist {
            get {
                return ResourceManager.GetString("ArgVal_DirectoryDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file {0} specified in parameter {1} does not exist..
        /// </summary>
        internal static string ArgVal_FileDoesNotExist {
            get {
                return ResourceManager.GetString("ArgVal_FileDoesNotExist", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot be less than {0}..
        /// </summary>
        internal static string ArgVal_LessThan {
            get {
                return ResourceManager.GetString("ArgVal_LessThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot be less than or equal to {0}..
        /// </summary>
        internal static string ArgVal_LessThanOrEqualTo {
            get {
                return ResourceManager.GetString("ArgVal_LessThanOrEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot be more than {0}..
        /// </summary>
        internal static string ArgVal_MoreThan {
            get {
                return ResourceManager.GetString("ArgVal_MoreThan", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot be more than or equal to {0}..
        /// </summary>
        internal static string ArgVal_MoreThanOrEqualTo {
            get {
                return ResourceManager.GetString("ArgVal_MoreThanOrEqualTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type {0} is not an enumerated type..
        /// </summary>
        internal static string ArgVal_NotAnEnumeratedType {
            get {
                return ResourceManager.GetString("ArgVal_NotAnEnumeratedType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not a valid member of enumeration {0}..
        /// </summary>
        internal static string ArgVal_NotValidEnumeratedValue {
            get {
                return ResourceManager.GetString("ArgVal_NotValidEnumeratedValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path must be specified..
        /// </summary>
        internal static string ArgVal_PathMustBeSpecified {
            get {
                return ResourceManager.GetString("ArgVal_PathMustBeSpecified", resourceCulture);
            }
        }
    }
}
