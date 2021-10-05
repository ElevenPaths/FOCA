using System.Windows.Forms;

namespace FOCA
{
    /// <summary>
    ///     Options panel. In this panel user can set if he wants to use all available
    ///     DNS servers to resolve hosts and the max recursivity level he want to
    ///     reach
    /// </summary>
    public partial class PanelOptions : UserControl
    {
        public PanelOptions()
        {
            InitializeComponent();
        }

        public bool UseAllDNS
        {
            get { return chkUseAllDns.Checked; }
            set { chkUseAllDns.Checked = value; }
        }

        public decimal MaxRecursion
        {
            get { return updMaxRecursivity.Value; }
            set { updMaxRecursivity.Value = value; }
        }
    }
}