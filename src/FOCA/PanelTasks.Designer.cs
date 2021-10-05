namespace FOCA
{
    partial class PanelTasks
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelTasks));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.pendientes = new FOCA.Search.ListViewEx();
            this.task = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ejecucion = new FOCA.Search.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.realizadas = new FOCA.Search.ListViewEx();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAutoScroll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(18, 22);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.realizadas);
            this.splitContainer1.Size = new System.Drawing.Size(1203, 477);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 12;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.pendientes);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ejecucion);
            this.splitContainer2.Size = new System.Drawing.Size(1203, 300);
            this.splitContainer2.SplitterDistance = 838;
            this.splitContainer2.SplitterWidth = 6;
            this.splitContainer2.TabIndex = 0;
            // 
            // pendientes
            // 
            this.pendientes.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.pendientes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.task});
            this.pendientes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pendientes.FullRowSelect = true;
            this.pendientes.GridLines = true;
            this.pendientes.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.pendientes.LabelWrap = false;
            this.pendientes.Location = new System.Drawing.Point(0, 0);
            this.pendientes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pendientes.Name = "pending";
            this.pendientes.Size = new System.Drawing.Size(838, 300);
            this.pendientes.TabIndex = 9;
            this.pendientes.UseCompatibleStateImageBehavior = false;
            this.pendientes.View = System.Windows.Forms.View.Details;
            // 
            // task
            // 
            this.task.Text = "Queued tasks";
            this.task.Width = 555;
            // 
            // ejecucion
            // 
            this.ejecucion.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ejecucion.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ejecucion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ejecucion.FullRowSelect = true;
            this.ejecucion.GridLines = true;
            this.ejecucion.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ejecucion.LabelWrap = false;
            this.ejecucion.Location = new System.Drawing.Point(0, 0);
            this.ejecucion.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ejecucion.Name = "execution";
            this.ejecucion.Size = new System.Drawing.Size(359, 300);
            this.ejecucion.TabIndex = 10;
            this.ejecucion.UseCompatibleStateImageBehavior = false;
            this.ejecucion.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Running tasks";
            this.columnHeader1.Width = 235;
            // 
            // realizadas
            // 
            this.realizadas.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.realizadas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2});
            this.realizadas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.realizadas.FullRowSelect = true;
            this.realizadas.GridLines = true;
            this.realizadas.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.realizadas.LabelWrap = false;
            this.realizadas.Location = new System.Drawing.Point(0, 0);
            this.realizadas.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.realizadas.Name = "done";
            this.realizadas.Size = new System.Drawing.Size(1203, 171);
            this.realizadas.TabIndex = 11;
            this.realizadas.UseCompatibleStateImageBehavior = false;
            this.realizadas.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Finished tasks";
            this.columnHeader2.Width = 798;
            // 
            // btnAutoScroll
            // 
            this.btnAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAutoScroll.Image = ((System.Drawing.Image)(resources.GetObject("btnAutoScroll.Image")));
            this.btnAutoScroll.Location = new System.Drawing.Point(18, 509);
            this.btnAutoScroll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAutoScroll.Name = "btnAutoScroll";
            this.btnAutoScroll.Size = new System.Drawing.Size(207, 38);
            this.btnAutoScroll.TabIndex = 15;
            this.btnAutoScroll.Text = "&Activate autoScroll";
            this.btnAutoScroll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAutoScroll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAutoScroll.UseVisualStyleBackColor = true;
            this.btnAutoScroll.Click += new System.EventHandler(this.buttonAutoScroll_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.Location = new System.Drawing.Point(246, 509);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(86, 38);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "&Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // PanelTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnAutoScroll);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PanelTasks";
            this.Size = new System.Drawing.Size(1245, 571);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private FOCA.Search.ListViewEx pendientes;
        private System.Windows.Forms.ColumnHeader task;
        private FOCA.Search.ListViewEx ejecucion;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private FOCA.Search.ListViewEx realizadas;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btnAutoScroll;
        private System.Windows.Forms.Button btnClear;

    }
}
