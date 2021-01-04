using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Components.Icons
{
    public partial class PinIcon : ImageIconBase
    {
        public PinIcon() : base(Properties.Resources.pin_icon) { }

        public PinIcon(IContainer container) : base(container) { }
    }
}
