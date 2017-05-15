using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librame.UnitTests
{
    [Serializable]
    public class SelectList
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public bool SelectedValue { get; set; }
    }
}
