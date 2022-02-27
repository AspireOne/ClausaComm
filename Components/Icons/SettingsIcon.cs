using System.ComponentModel;

namespace ClausaComm.Components.Icons;

public partial class SettingsIcon : ImageIconBase
{
    public SettingsIcon() : base(Properties.Resources.settings) { }

    public SettingsIcon(IContainer container) : this() => container.Add(this);
}