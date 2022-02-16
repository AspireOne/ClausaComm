using System.ComponentModel;

namespace ClausaComm.Components.Icons;

public partial class FileSelectorIcon : ImageIconBase
{
    public FileSelectorIcon() : base(Properties.Resources.file_icon) { }

    public FileSelectorIcon(IContainer container) : this() => container.Add(this);
}