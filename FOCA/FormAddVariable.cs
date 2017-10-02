using System;
using System.Drawing;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormAddVariable : Form
    {
        private readonly Color color;
        private readonly ParametersAddVariable currentParameters;

        /// <summary>
        ///     Create a new form aligned with its creator form
        /// </summary>
        /// <param name="frmParent">Creator form</param>
        /// <param name="strPattern">Pattern to be added</param>
        public FormAddVariable(Form frmParent, ParametersAddVariable currentParameters, Color color)
        {
            InitializeComponent();
            // center the new form
            Left = frmParent.Left + (frmParent.Width - Width)/2;
            Top = frmParent.Top + (frmParent.Height - Height)/2;
            this.currentParameters = currentParameters;
            rtfPattern.Text = currentParameters.text;
            this.color = color;
        }

        /// <summary>
        ///     Event to color the pattern where the cursor is located
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            var rtb = (RichTextBox) sender;
            var cursorPos = rtb.SelectionStart;

            if (PatternExists(rtb.Text, cursorPos) ||
                (currentParameters.PosPattern != -1 && cursorPos >= currentParameters.PosPattern &&
                 cursorPos <= currentParameters.PosPattern + currentParameters.StrPattern.Length)) return;
            // delete the pattern from the previous position
            if (currentParameters.PosPattern != -1)
            {
                rtb.SelectionChanged -= richTextBox1_SelectionChanged;
                rtb.Text = rtb.Text.Remove(currentParameters.PosPattern, currentParameters.StrPattern.Length);

                if (currentParameters.PosPattern == 0)
                {
                    rtb.Select(0, rtb.Text.Length);
                    rtb.SelectionColor = rtb.ForeColor;
                }
                rtb.SelectionChanged += richTextBox1_SelectionChanged;
                // fix cursorPos
                if (currentParameters.PosPattern < cursorPos)
                {
                    cursorPos -= currentParameters.StrPattern.Length;
                }
            }
            rtb.SelectionChanged -= richTextBox1_SelectionChanged;
            rtb.Text = rtb.Text.Insert(cursorPos, currentParameters.StrPattern);
            currentParameters.PosPattern = cursorPos;
            // color the pattern
            rtb.Select(cursorPos, currentParameters.StrPattern.Length);
            rtb.SelectionColor = color;

            rtb.SelectionLength = 0;

            rtb.SelectionChanged += richTextBox1_SelectionChanged;
        }

        /// <summary>
        ///     Check if a pattern exists in a given position of a string
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool PatternExists(string text, int position)
        {
            if (text.IndexOf('}', position) <= 0) return false;
            return text.IndexOf('{', position) <= 0 || text.IndexOf('{', position) >= text.IndexOf('}', position);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (currentParameters.PosPattern == -1)
            {
                MessageBox.Show(@"Select the position of the new variable, click in the text box.", @"Invalid value",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            char chrStartValue, chrEndValue;
            if (char.TryParse(txtStart.Text, out chrStartValue) &&
                char.TryParse(txtEnd.Text, out chrEndValue))
            {
                var bStart = (byte) chrStartValue;
                var bEnd = (byte) chrEndValue;
                if (bStart > bEnd)
                {
                    MessageBox.Show(@"The start value must be less than the final value", @"Invalid value",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                currentParameters.IsChar = true;
                currentParameters.ChrStartValue = chrStartValue;
                currentParameters.ChrEndValue = chrEndValue;
            }
            else
            {
                int intStartValue;
                if (!int.TryParse(txtStart.Text, out intStartValue))
                {
                    MessageBox.Show(@"Invalid start value", @"Invalid value", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                int intEndValue;
                if (!int.TryParse(txtEnd.Text, out intEndValue))
                {
                    MessageBox.Show(@"Invalid end value", @"Invalid value", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }
                if (intStartValue > intEndValue)
                {
                    MessageBox.Show(@"The start value must be less than the final value", @"Invalid value",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                currentParameters.IsChar = false;
                currentParameters.IntStartValue = intStartValue;
                currentParameters.IntEndValue = intEndValue;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        ///     Parameters needed to do a DNS Zone Transfer
        /// </summary>
        public class ParametersAddVariable
        {
            public char ChrEndValue;
            public char ChrStartValue;
            public int IntEndValue;
            public int IntStartValue;
            public int PosPattern = -1;
            public string StrPattern;
            public string text;
            public bool IsChar { get; set; }

            public int GetStartValue()
            {
                return IsChar ? ChrStartValue : IntStartValue;
            }

            public int GetEndValue()
            {
                return IsChar ? ChrEndValue : IntEndValue;
            }
        }
    }
}