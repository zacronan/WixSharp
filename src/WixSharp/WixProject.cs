using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace WixSharp
{
    public abstract partial class WixProject : WixEntity 
    {
        string sourceBaseDir = "";
        /// <summary>
        /// Base directory for the relative paths of the bootstrapper items (e.g. <see cref="MsiPackage"></see>).
        /// </summary>
        public string SourceBaseDir
        {
            get { return sourceBaseDir.ExpandEnvVars(); }
            set { sourceBaseDir = value; }
        }

        string outFileName = "setup";

        /// <summary>
        /// Name of the MSI/MSM file (without extension) to be build.
        /// </summary>
        public string OutFileName { get { return outFileName; } set { outFileName = value; } }

        string outDir;

        /// <summary>
        /// The output directory. The directory where all msi and temporary files should be assembled. The <c>CurrentDirectory</c> will be used if <see cref="OutDir"/> is left unassigned.
        /// </summary>
        public string OutDir
        {
            get
            {
                return outDir.IsEmpty() ? Environment.CurrentDirectory : outDir.ExpandEnvVars();
            }
            set
            {
                outDir = value;
            }
        }

        /// <summary>
        /// Collection of XML namespaces (e.g. <c>xmlns:iis="http://schemas.microsoft.com/wix/IIsExtension"</c>) to be declared in the XML (WiX project) root.
        /// </summary>
        public List<string> WixNamespaces = new List<string>();

        /// <summary>
        /// Collection of paths to the WiX extensions.
        /// </summary>
        public List<string> WixExtensions = new List<string>();

        /// <summary>
        /// Installation UI Language. If not specified <c>"en-US"</c> will be used.
        /// </summary>
        public string Language = "en-US";

        /// <summary>
        /// WiX linker <c>Light.exe</c> options.
        /// <para>The default value is "-sw1076 -sw1079" (disable warning 1076 and 1079).</para>
        /// </summary>
        public string LightOptions = "";

        /// <summary>
        /// WiX compiler <c>Candle.exe</c> options.
        /// <para>The default value is "-sw1076" (disable warning 1026).</para>
        /// </summary>
        public string CandleOptions = "";

        /// <summary>
        /// Occurs when WiX source code generated. Use this event if you need to modify generated XML (XDocument)
        /// before it is compiled into MSI.
        /// </summary>
        public event XDocumentGeneratedDlgt WixSourceGenerated;

        /// <summary>
        /// Occurs when WiX source file is saved. Use this event if you need to do any post-processing of the generated/saved file.
        /// </summary>
        public event XDocumentSavedDlgt WixSourceSaved;

        /// <summary>
        /// Occurs when WiX source file is formatted and ready to be saved. Use this event if you need to do any custom formatting of the XML content before
        /// it is saved by the compiler.
        /// </summary>
        public event XDocumentFormatedDlgt WixSourceFormated;

        /// <summary>
        /// Forces <see cref="Compiler"/> to preserve all temporary build files (e.g. *.wxs).
        /// <para>The default value is <c>false</c>: all temporary files are deleted at the end of the build/compilation.</para>
        /// <para>Note: if <see cref="Compiler"/> fails to build MSI the <c>PreserveTempFiles</c>
        /// value is ignored and all temporary files are preserved.</para>
        /// </summary>
        public bool PreserveTempFiles = false;

        internal void InvokeWixSourceGenerated(XDocument doc)
        {
            if (WixSourceGenerated != null)
                WixSourceGenerated(doc);
        }

        internal void InvokeWixSourceSaved(string fileName)
        {
            if (WixSourceSaved != null)
                WixSourceSaved(fileName);
        }

        internal void InvokeWixSourceFormated(ref string content)
        {
            if (WixSourceFormated != null)
                WixSourceFormated(ref content);
        }
    }
}