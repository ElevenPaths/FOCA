using System;
using System.Windows.Forms;

namespace FOCA
{
    /// <summary>
    ///     Using a web searcher like Google or Bing the program searchs links pointing
    ///     to the domain site to identify new subdomains.
    /// </summary>
    public partial class PanelWebSearcherInformation : UserControl
    {
        public enum Engine
        {
            GoogleWeb,
            GoogleAPI,
            BingWeb,
            BingAPI,
            DuckDuckGoWeb
        }

        private Engine _selectedEngine;

        public PanelWebSearcherInformation()
        {
            InitializeComponent();
            panelEngineBingAPIInformation.Visible = false;
            panelEngineBingWebInformation.Visible = false;
            panelEngineGoogleAPIInformation.Visible = false;
            panelEngineGoogleWebInformation.Visible = false;
        }

        public Engine SelectedEngine
        {
            set
            {
                _selectedEngine = value;
                cboEngine.Text = EngineToString(value);
            }
            get { return _selectedEngine; }
        }

        private void PanelWebSearcher_Load(object sender, EventArgs e)
        {
            FillComboboxEngine();
            comboBoxEngine_SelectedValueChanged(cboEngine, null);
        }

        private void FillComboboxEngine()
        {
            cboEngine.Items.Clear();
            foreach (Engine e in Enum.GetValues(typeof(Engine)))
                cboEngine.Items.Add(EngineToString(e));
        }

        private void comboBoxEngine_SelectedValueChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
                switch (comboBox.Text)
                {
                    case "GoogleWeb":
                        SelectedEngine = Engine.GoogleWeb;
                        break;
                    case "GoogleAPI":
                        SelectedEngine = Engine.GoogleAPI;
                        break;
                    case "BingWeb":
                        SelectedEngine = Engine.BingWeb;
                        break;
                    case "BingAPI":
                        SelectedEngine = Engine.BingAPI;
                        break;
                    default:
                        MessageBox.Show(@"Select a valid engine, please!", Application.ProductName, MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                        break;
                }
            panelEngineGoogleWebInformation.Visible = SelectedEngine == Engine.GoogleWeb;
            panelEngineGoogleAPIInformation.Visible = SelectedEngine == Engine.GoogleAPI;
            panelEngineBingWebInformation.Visible = SelectedEngine == Engine.BingWeb;
            panelEngineBingAPIInformation.Visible = SelectedEngine == Engine.BingAPI;
        }

        public static string EngineToString(Engine e)
        {
            return e.ToString();
        }
    }
}