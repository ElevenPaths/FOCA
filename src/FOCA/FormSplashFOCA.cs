using System.Windows.Forms;

namespace FOCA
{
    public partial class FormSplashFOCA : Form
    {
        private FormSplashFOCA()
        {
            InitializeComponent();
        }

        public FormSplashFOCA(string version) : this()
        {
            lblVersionValue.Text = version;
        }
    }
}