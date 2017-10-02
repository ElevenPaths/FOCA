using System;
using System.Windows.Forms;
using FOCA.TaskManager;

namespace FOCA
{
    public partial class PanelTasks : UserControl
    {
        private bool bAutoScroll;

        public PanelTasks()
        {
            InitializeComponent();
        }

        public void ResetGUI()
        {
            var result = BeginInvoke(new MethodInvoker(delegate
            {
                pendientes.Items.Clear();
                pendientes.Columns[0].Text = @"Queued tasks (" + pendientes.Items.Count + @")";
                ejecucion.Items.Clear();
                ejecucion.Columns[0].Text = @"Queued tasks (" + ejecucion.Items.Count + @")";
                realizadas.Items.Clear();
                realizadas.Columns[0].Text = @"Queued tasks (" + realizadas.Items.Count + @")";
            }));

            EndInvoke(result);
        }

        public void AddNewTask(TaskFOCA t)
        {
            Invoke(new MethodInvoker(delegate
            {
                pendientes.BeginUpdate();
                pendientes.Items.Add(t.id.ToString(), "[" + t.id + "] " + t, 75);
                pendientes.EndUpdate();

                pendientes.Columns[0].Text = @"Queued tasks (" + pendientes.Items.Count + @")";

                if (!bAutoScroll) return;
                if (pendientes.Items.Count > 0)
                    pendientes.EnsureVisible(pendientes.Items.Count - 1);
            }));
        }

        public void StartTask(TaskFOCA t)
        {
            Invoke(new MethodInvoker(delegate
            {
                ejecucion.BeginUpdate();
                ejecucion.Items.Add(t.id.ToString(), "[" + t.id + "] " + t, 75);
                ejecucion.EndUpdate();

                pendientes.BeginUpdate();
                pendientes.Items.RemoveByKey(t.id.ToString());
                pendientes.EndUpdate();

                ejecucion.Columns[0].Text = @"Running tasks (" + ejecucion.Items.Count + @")";
                pendientes.Columns[0].Text = @"Queued tasks (" + pendientes.Items.Count + @")";

                if (!bAutoScroll) return;
                if (ejecucion.Items.Count > 0)
                    ejecucion.EnsureVisible(ejecucion.Items.Count - 1);
            }));
        }

        public void EndTask(TaskFOCA t)
        {
            Invoke(new MethodInvoker(delegate
            {
                realizadas.BeginUpdate();
                realizadas.Items.Add(t.id.ToString(), "[" + t.id + "] " + t, 75);
                realizadas.EndUpdate();

                ejecucion.BeginUpdate();
                ejecucion.Items.RemoveByKey(t.id.ToString());
                ejecucion.EndUpdate();


                realizadas.Columns[0].Text = @"Finished tasks (" + realizadas.Items.Count + ")";
                ejecucion.Columns[0].Text = @"Running tasks (" + ejecucion.Items.Count + ")";

                if (!bAutoScroll) return;
                if (realizadas.Items.Count > 0)
                    realizadas.EnsureVisible(realizadas.Items.Count - 1);
            }));
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            realizadas.Items.Clear();
            pendientes.Items.Clear();
            ejecucion.Items.Clear();
        }

        private void buttonAutoScroll_Click(object sender, EventArgs e)
        {
            btnAutoScroll.Text = (btnAutoScroll.Text.Equals("Activate AutoScroll"))
                ? "Deactivate AutoScroll"
                : "Activate AutoScroll";
            bAutoScroll = !bAutoScroll;
        }
    }
}