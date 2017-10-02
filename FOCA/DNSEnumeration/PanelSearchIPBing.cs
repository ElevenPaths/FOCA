using System;
using System.Windows.Forms;

namespace FOCA
{
    /// <summary>
    ///     Search IP Bing information panel and Web Search engine selection
    /// </summary>
    public partial class PanelSearchIPBing : UserControl
    {
        public enum Engine
        {
            BingWeb,
            BingAPI
        }

        private Engine _selectedEngine;

        public PanelSearchIPBing()
        {
            InitializeComponent();
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

        private void PanelSearchIPBing_Load(object sender, EventArgs e)
        {
            FillComboboxEngine();
            panelEngineBingWebInformation.Visible = true;
        }

        private void FillComboboxEngine()
        {
            cboEngine.Items.Clear();
            foreach (Engine e in Enum.GetValues(typeof (Engine)))
                cboEngine.Items.Add(EngineToString(e));
        }

        private void comboBoxEngine_SelectedValueChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox != null)
                switch (comboBox.Text)
                {
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
            panelEngineBingWebInformation.Visible = SelectedEngine == Engine.BingWeb;
            panelEngineBingAPIInformation.Visible = SelectedEngine == Engine.BingAPI;
        }

        public static string EngineToString(Engine e)
        {
            switch (e)
            {
                case Engine.BingAPI:
                    return "BingAPI";
                case Engine.BingWeb:
                    return "BingWeb";
                default:
                    return string.Empty;
            }
        }

        public static Engine StringToEngine(string s)
        {
            switch (s)
            {
                case "BingWeb":
                    return Engine.BingWeb;
                case "BingAPI":
                    return Engine.BingAPI;
                default:
                    return Engine.BingWeb;
            }
        }
    }
}