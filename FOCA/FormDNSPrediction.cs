using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormDNSPrediction : Form
    {
        private readonly List<string> lstVariants;

        public Color[] DefaultColors =
        {
            Color.SteelBlue, Color.DarkRed, Color.Goldenrod, Color.OliveDrab, Color.Orange,
            Color.MediumPurple, Color.OrangeRed
        };

        /// <summary>
        ///     Create a new instance of the form aligned with its creator form and fulfil the list with the possible variants
        /// </summary>
        /// <param name="frmParent"></param>
        /// <param name="lstResult"></param>
        /// <param name="strDomain"></param>
        public FormDNSPrediction(Form frmParent, List<string> lstResult, string strDomain)
        {
            Left = frmParent.Left + (frmParent.Width - Width)/2;
            Top = frmParent.Top + (frmParent.Height - Height)/2;
            lstVariants = lstResult;
            InitializeComponent();

            rtbPattern.Text = strDomain;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Open a new form where FOCA will receive the pattern's position and all possible values
            var pav = new FormAddVariable.ParametersAddVariable {text = rtbPattern.Text};
            var intPatron = FindFirstFreePattern();
            pav.StrPattern = string.Format("{{{0}}}", intPatron);
            // Pattern's color changes
            Form frm = new FormAddVariable(this, pav, DefaultColors[intPatron%DefaultColors.Length]);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                var lvi = listViewPatterns.Items.Add(pav.StrPattern);
                lvi.SubItems.Add(pav.IsChar
                    ? string.Format("{0} - {1}", pav.ChrStartValue, pav.ChrEndValue)
                    : string.Format("{0} - {1}", pav.IntStartValue, pav.IntEndValue));
                lvi.UseItemStyleForSubItems = false;
                lvi.ForeColor = DefaultColors[intPatron%DefaultColors.Length];
                lvi.Tag = pav;
                rtbPattern.Text = rtbPattern.Text.Insert(pav.PosPattern, pav.StrPattern);
            }
            var valuesCount = CalculateTotalValuesCount();
            lblVariants.Text = valuesCount == 0 ? "The value exceeds the limits" : valuesCount.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in listViewPatterns.SelectedItems)
            {
                // remove the pattern from the text box
                while (rtbPattern.Text.Contains(lvi.Text))
                {
                    var posPattern = rtbPattern.Text.IndexOf(lvi.Text, StringComparison.Ordinal);
                    var lenghtPattern = lvi.Text.Length;
                    rtbPattern.Text = rtbPattern.Text.Remove(posPattern, lenghtPattern);
                }
                // remove the pattern from the list view
                lvi.Remove();
            }
            var totalValues = CalculateTotalValuesCount();
            lblVariants.Text = totalValues == 0 ? "The value exceeds the limits" : totalValues.ToString();
        }

        /// <summary>
        ///     Find the first pattern position without values added
        /// </summary>
        /// <returns></returns>
        private int FindFirstFreePattern()
        {
            var i = 0;
            foreach (ListViewItem lvi in listViewPatterns.Items)
            {
                if (lvi.Text != string.Format("{{{0}}}", i))
                    return i;
                i++;
            }
            return i;
        }

        /// <summary>
        ///     Color patterns again whenever the text changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtbPattern_TextChanged(object sender, EventArgs e)
        {
            var oriSelectionStart = rtbPattern.SelectionStart;
            var oriSelectionLength = rtbPattern.SelectionLength;
            foreach (ListViewItem lvi in listViewPatterns.Items)
            {
                for (var startPos = 0;
                    rtbPattern.Text.IndexOf(lvi.Text, startPos) > -1;
                    startPos = rtbPattern.Text.IndexOf(lvi.Text, startPos) + lvi.Text.Length)
                {
                    rtbPattern.Select(rtbPattern.Text.IndexOf(lvi.Text, startPos), lvi.Text.Length);
                    rtbPattern.SelectionColor = lvi.ForeColor;
                }
            }
            rtbPattern.SelectionStart = oriSelectionStart;
            rtbPattern.SelectionLength = oriSelectionLength;
        }

        /// <summary>
        ///     Enable delete button whenever a pattern is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewPatterns_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = listViewPatterns.SelectedItems.Count != 0;
        }

        /// <summary>
        ///     Count number of possible total values
        /// </summary>
        /// <returns></returns>
        private uint CalculateTotalValuesCount()
        {
            uint total = 1;
            foreach (var pav in from ListViewItem lvi in listViewPatterns.Items select (FormAddVariable.ParametersAddVariable) lvi.Tag)
            {
                try
                {
                    checked
                    {
                        total *= (uint) (pav.GetEndValue() - pav.GetStartValue() + 1);
                    }
                }
                catch
                {
                    return 0;
                }
            }
            return total;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            uint totalValues = CalculateTotalValuesCount();
            if (totalValues <= 1)
            {
                MessageBox.Show(@"Add at least one variable", Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            if (totalValues > 10000)
            {
                MessageBox.Show(@"The number of possibilities exceeds the limit of 10000, narrow down the range.",
                    Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                GenerateVariations();
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        ///     Generate all possible variants
        /// </summary>
        private void GenerateVariations()
        {
            var nPatterns = listViewPatterns.Items.Count;
            // this array will store the current value of each pattern
            var currentValues = new int[nPatterns];
            // existing patterns array
            var pav = new FormAddVariable.ParametersAddVariable[nPatterns];
            // set initial value of each pattern
            for (var i = 0; i < nPatterns; i++)
            {
                pav[i] = (FormAddVariable.ParametersAddVariable) listViewPatterns.Items[i].Tag;
                currentValues[i] = pav[i].GetStartValue();
            }
            while (true)
            {
                do
                {
                    // add new variant
                    lstVariants.Add(VariantToString(rtbPattern.Text, currentValues, pav));
                } while (IncrementValue(ref currentValues, nPatterns - 1, pav[nPatterns - 1]));
                currentValues[nPatterns - 1] = pav[nPatterns - 1].GetStartValue();
                // if the current value is not the maximum, increment the previous value
                var indexIncrement = nPatterns - 2;
                while (indexIncrement >= 0 &&
                       !IncrementValue(ref currentValues, indexIncrement, pav[indexIncrement]))
                {
                    currentValues[indexIncrement] = pav[indexIncrement].GetStartValue();
                    indexIncrement--;
                }
                if (indexIncrement < 0)
                    break;
            }
        }

        /// <summary>
        ///     Increment the values array current value
        /// </summary>
        /// <param name="currentValues"></param>
        /// <param name="valueIndex"></param>
        /// <param name="pav"></param>
        /// <returns>False if the value can not be incremented</returns>
        private bool IncrementValue(ref int[] currentValues, int valueIndex, FormAddVariable.ParametersAddVariable pav)
        {
            if (currentValues[valueIndex] == pav.GetEndValue())
                return false;
            currentValues[valueIndex]++;
            return true;
        }

        /// <summary>
        ///     Given cureent values of values and patterns, replace them in the string
        /// </summary>
        /// <param name="currentValues"></param>
        /// <param name="pav"></param>
        /// <returns>The string with all patterns replaced by the values</returns>
        private string VariantToString(string strMain, int[] currentValues, FormAddVariable.ParametersAddVariable[] pav)
        {
            for (var i = 0; i < pav.Length; i++)
            {
                var strValue = pav[i].IsChar ? Convert.ToChar(currentValues[i]).ToString() : currentValues[i].ToString();
                strMain = strMain.Replace(pav[i].StrPattern, strValue);
            }
            return strMain;
        }
    }
}