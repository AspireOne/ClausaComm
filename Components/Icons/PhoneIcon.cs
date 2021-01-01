using ClausaComm.Components.Icons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClausaComm.Components.Icons
{
    public partial class PhoneIcon : ImageIconBase
    {
        public PhoneIcon() : base(Properties.Resources.phone_icon) { }
        public PhoneIcon(IContainer container) : base(container) { }
    }
}
