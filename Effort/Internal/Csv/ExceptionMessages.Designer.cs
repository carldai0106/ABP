﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.0
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Effort.Internal.Csv {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Effort.Internal.Csv.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 Buffer size must be 1 or more. 的本地化字符串。
        /// </summary>
        internal static string BufferSizeTooSmall {
            get {
                return ResourceManager.GetString("BufferSizeTooSmall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot move to a previous record in forward-only mode. 的本地化字符串。
        /// </summary>
        internal static string CannotMovePreviousRecordInForwardOnly {
            get {
                return ResourceManager.GetString("CannotMovePreviousRecordInForwardOnly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot read record at index &apos;{0}&apos;. 的本地化字符串。
        /// </summary>
        internal static string CannotReadRecordAtIndex {
            get {
                return ResourceManager.GetString("CannotReadRecordAtIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Enumeration has either not started or has already finished. 的本地化字符串。
        /// </summary>
        internal static string EnumerationFinishedOrNotStarted {
            get {
                return ResourceManager.GetString("EnumerationFinishedOrNotStarted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Collection was modified; enumeration operation may not execute. 的本地化字符串。
        /// </summary>
        internal static string EnumerationVersionCheckFailed {
            get {
                return ResourceManager.GetString("EnumerationVersionCheckFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &apos;{0}&apos; field header not found. 的本地化字符串。
        /// </summary>
        internal static string FieldHeaderNotFound {
            get {
                return ResourceManager.GetString("FieldHeaderNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Field index must be included in [0, FieldCount[. Specified field index was : &apos;{0}&apos;. 的本地化字符串。
        /// </summary>
        internal static string FieldIndexOutOfRange {
            get {
                return ResourceManager.GetString("FieldIndexOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The CSV appears to be corrupt near record &apos;{0}&apos; field &apos;{1} at position &apos;{2}&apos;. Current raw data : &apos;{3}&apos;. 的本地化字符串。
        /// </summary>
        internal static string MalformedCsvException {
            get {
                return ResourceManager.GetString("MalformedCsvException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &apos;{0}&apos; is not a supported missing field action. 的本地化字符串。
        /// </summary>
        internal static string MissingFieldActionNotSupported {
            get {
                return ResourceManager.GetString("MissingFieldActionNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 No current record. 的本地化字符串。
        /// </summary>
        internal static string NoCurrentRecord {
            get {
                return ResourceManager.GetString("NoCurrentRecord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The CSV does not have headers (CsvReader.HasHeaders property is false). 的本地化字符串。
        /// </summary>
        internal static string NoHeaders {
            get {
                return ResourceManager.GetString("NoHeaders", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 The number of fields in the record is greater than the available space from index to the end of the destination array. 的本地化字符串。
        /// </summary>
        internal static string NotEnoughSpaceInArray {
            get {
                return ResourceManager.GetString("NotEnoughSpaceInArray", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &apos;{0}&apos; is not a valid ParseErrorAction while inside a ParseError event. 的本地化字符串。
        /// </summary>
        internal static string ParseErrorActionInvalidInsideParseErrorEvent {
            get {
                return ResourceManager.GetString("ParseErrorActionInvalidInsideParseErrorEvent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 &apos;{0}&apos; is not a supported ParseErrorAction. 的本地化字符串。
        /// </summary>
        internal static string ParseErrorActionNotSupported {
            get {
                return ResourceManager.GetString("ParseErrorActionNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 This operation is invalid when the reader is closed. 的本地化字符串。
        /// </summary>
        internal static string ReaderClosed {
            get {
                return ResourceManager.GetString("ReaderClosed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Record index must be 0 or more. 的本地化字符串。
        /// </summary>
        internal static string RecordIndexLessThanZero {
            get {
                return ResourceManager.GetString("RecordIndexLessThanZero", resourceCulture);
            }
        }
    }
}