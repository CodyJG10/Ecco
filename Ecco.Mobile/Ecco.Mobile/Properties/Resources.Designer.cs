﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Ecco.Mobile.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Ecco.Mobile.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Endpoint=sb://ecco-space-events.servicebus.windows.net/;SharedAccessKeyName=main;SharedAccessKey=9b1iT0P8sq2MFjbPiFiNf9WTpbIia4/MmLh5jwvTOaM=;EntityPath=ecco-events.
        /// </summary>
        internal static string EventsHubConnectionString {
            get {
                return ResourceManager.GetString("EventsHubConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ecco-events.
        /// </summary>
        internal static string EventsHubName {
            get {
                return ResourceManager.GetString("EventsHubName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DefaultEndpointsProtocol=https;AccountName=eccoeventstorage;AccountKey=eXZzxBqVgwqip23o+tEwp2efwPVQDaIizwoRES0aEUfccV9L+Y35YGdSsZj9J8YF+76FRJPuckeAuHB7f5RB1w==;EndpointSuffix=core.windows.net.
        /// </summary>
        internal static string EventsHubStorageConnectionString {
            get {
                return ResourceManager.GetString("EventsHubStorageConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to events.
        /// </summary>
        internal static string EventsHubStorageContainerName {
            get {
                return ResourceManager.GetString("EventsHubStorageContainerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DefaultEndpointsProtocol=https;AccountName=eccospacestorage;AccountKey=Nr6eERil/QqRitQ/XThQ9yElPlH844fwAqE0LDOX6ktyYae0S5xtvv5W/d0lrM3Y7uI8KP7qRgoQ/unHCmYnIw==;EndpointSuffix=core.windows.net.
        /// </summary>
        internal static string StorageConnectionString {
            get {
                return ResourceManager.GetString("StorageConnectionString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NDYzNjYyQDMxMzkyZTMxMmUzMFJacUpZSWlyM083MEs3aVFvU0NtMDlLN0RKQmRMVExiUXJUS29nQWtOakk9.
        /// </summary>
        internal static string SyncfusionLicense {
            get {
                return ResourceManager.GetString("SyncfusionLicense", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ABCDEFGHIJKLMNOPQRSTUVWXYZ.
        /// </summary>
        internal static string WebSecret {
            get {
                return ResourceManager.GetString("WebSecret", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://ecco-space.azurewebsites.net/.
        /// </summary>
        internal static string WebUrl {
            get {
                return ResourceManager.GetString("WebUrl", resourceCulture);
            }
        }
    }
}
