using System.ComponentModel.Composition;
using DevExpress.CodeRush.Common;

namespace CR_SetPluginVersion
{
    [Export(typeof(IVsixPluginExtension))]
    public class CR_SetPluginVersionExtension : IVsixPluginExtension { }
}