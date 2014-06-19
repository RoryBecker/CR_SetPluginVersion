using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.CodeRush.Core;
using DevExpress.CodeRush.PlugInCore;
using DevExpress.CodeRush.StructuralParser;
using SP = DevExpress.CodeRush.StructuralParser;

namespace CR_SetPluginVersion
{
    public partial class PlugIn1 : StandardPlugIn
    {
        // DXCore-generated code...
        #region InitializePlugIn
        public override void InitializePlugIn()
        {
            base.InitializePlugIn();
            registerSetPluginVersion();
        }
        #endregion
        #region FinalizePlugIn
        public override void FinalizePlugIn()
        {
            //
            // TODO: Add your finalization code here.
            //

            base.FinalizePlugIn();
        }
        #endregion
        public void registerSetPluginVersion()
        {
            DevExpress.CodeRush.Core.Action SetPluginVersion = new DevExpress.CodeRush.Core.Action(components);
            ((System.ComponentModel.ISupportInitialize)(SetPluginVersion)).BeginInit();
            SetPluginVersion.ActionName = "SetPluginVersion";
            SetPluginVersion.ButtonText = "Set Plugin Version"; // Used if button is placed on a menu.
            SetPluginVersion.RegisterInCR = true;
            SetPluginVersion.CheckAvailability += SetPluginVersion_CheckAvailability;
            SetPluginVersion.Execute += SetPluginVersion_Execute;
            SetPluginVersion.CommonMenu = DevExpress.CodeRush.Menus.VsCommonBar.ProjectContext;
            ((System.ComponentModel.ISupportInitialize)(SetPluginVersion)).EndInit();
        }
        private void SetPluginVersion_CheckAvailability(CheckActionAvailabilityEventArgs ea)
        {
            // Project references DevExpress.CodeRush.PluginCore
            var References = (from AssemblyReference reference in CodeRush.Source.ActiveProject.AssemblyReferences
                              where reference.Name == "DevExpress.CodeRush.PluginCore.dll"
                              select (AssemblyReference)reference);
            if (!References.Any())
                return;

            // Project contains a class which inherits from StandardPlugin
            var PluginClasses = (from Class item in CodeRush.Source.ActiveProject.AllTypes.OfType<Class>()
                                 where item.PrimaryAncestorType.Name == "StandardPlugin"
                                 select item);
            if (!PluginClasses.Any())
                return;

        }
        private void SetPluginVersion_Execute(ExecuteEventArgs ea)
        {

            var AttributeValue = GetVersion.GetNewVersion("1.0.0.0");
            // Find AssemblyVersion and change version to captured value.
            SetAttributeValue("AssemblyVersion", AttributeValue);
            // Find AssemblyFileVersion and change value to captured value.
            SetAttributeValue("AssemblyFileVersion", AttributeValue);

            SetVSIXVersion(AttributeValue);

        }
        private static void SetAttributeValue(string AttributeName, string AttributeValue)
        {
            var Attributes = CodeRush.Source.ActiveProject.GetAssemblyAttributes().ToList();
            var AttributeVersion = (from IAttributeElement item in Attributes
                                    where item.Name == AttributeName
                                    select item).First().ToLanguageElement();
            var ChildPrimitive = ((PrimitiveExpression)AttributeVersion.DetailNodes[0]);

            CodeRush.File.ChangeFile(AttributeVersion.FileNode,
                new SourceRange[] { ChildPrimitive.NameRange.Contract(1, 1) },
                AttributeValue);
        }
        private void SetVSIXVersion(string Version)
        {
            var VSIXDoc = (from TextDocument item in CodeRush.Documents.AllTextDocuments 
                           where item.Name.EndsWith("source.extension.vsixmanifest")
                           select item).First().FileNode;
            var VersionNode = VSIXDoc.GetNode("Vsix").GetNode("Identifier").GetNode("Version");
            CodeRush.File.ChangeFile(VSIXDoc.GetSourceFile(),
                                     new SourceRange[] { VersionNode.InnerRange},
                                     Version);

        }

    }
    public static class SourceRangeExt
    {
        public static SourceRange Expand(this SourceRange source, int left, int right)
        {
            return new SourceRange(source.Start.OffsetPoint(0, -left), source.End.OffsetPoint(0, right));
        }
        public static SourceRange Contract(this SourceRange source, int left, int right)
        {
            return new SourceRange(source.Start.OffsetPoint(0, left), source.End.OffsetPoint(0, -right));
        }
    }
    public static class LanguageElementExt
    {
        public static SP.HtmlElement GetNode(this LanguageElement Parent, string NodeName)
        {
            if (Parent == null)
                return null;
            foreach (LanguageElement item in Parent.Nodes)
            {
                if (item.Name == NodeName)
                    return (SP.HtmlElement)item;
            }
            return (from SP.HtmlElement item in Parent.Nodes
                    where item.Name == NodeName
                    select item).First();
        }
    }
}