using System.Runtime.InteropServices;

namespace Librame.Tests
{
    //[Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class SelectList
    {
        public string Text { get; set; }

        public string Value { get; set; }

        public bool SelectedValue { get; set; }
    }
}
