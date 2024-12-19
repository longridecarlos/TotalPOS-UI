namespace TotalPOS_UI
{
    partial class Main
    {
        private void InitializeComponent()
        {
            this.listViewTavolo = new System.Windows.Forms.ListView();
            this.colId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCreatedAt = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTotal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTotale = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOpen = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnApri = new System.Windows.Forms.Button();
            this.btnNuovo = new System.Windows.Forms.Button();
            this.btnCancellare = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnInsieme = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewTavolo
            // 
            this.listViewTavolo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colId,
            this.colName,
            this.colCreatedAt,
            this.colTotal,
            this.colTotale,
            this.colOpen});
            this.listViewTavolo.FullRowSelect = true;
            this.listViewTavolo.GridLines = true;
            this.listViewTavolo.HideSelection = false;
            this.listViewTavolo.Location = new System.Drawing.Point(12, 54);
            this.listViewTavolo.Name = "listViewTavolo";
            this.listViewTavolo.Size = new System.Drawing.Size(604, 248);
            this.listViewTavolo.TabIndex = 0;
            this.listViewTavolo.UseCompatibleStateImageBehavior = false;
            this.listViewTavolo.View = System.Windows.Forms.View.Details;
            // 
            // colId
            // 
            this.colId.Text = "ID";
            this.colId.Width = 50;
            // 
            // colName
            // 
            this.colName.Text = "Nome";
            this.colName.Width = 150;
            // 
            // colCreatedAt
            // 
            this.colCreatedAt.Text = "Data di creazione";
            this.colCreatedAt.Width = 150;
            // 
            // colTotal
            // 
            this.colTotal.Text = "Total";
            this.colTotal.Width = 100;
            // 
            // colTotale
            // 
            this.colTotale.Text = "Totale product";
            this.colTotale.Width = 100;
            // 
            // colOpen
            // 
            this.colOpen.Text = "Open";
            this.colOpen.Width = 50;
            // 
            // btnApri
            // 
            this.btnApri.Location = new System.Drawing.Point(649, 106);
            this.btnApri.Name = "btnApri";
            this.btnApri.Size = new System.Drawing.Size(119, 46);
            this.btnApri.TabIndex = 1;
            this.btnApri.Text = "Apri";
            this.btnApri.UseVisualStyleBackColor = true;
            this.btnApri.Click += new System.EventHandler(this.btnApri_Click);
            // 
            // btnNuovo
            // 
            this.btnNuovo.Location = new System.Drawing.Point(649, 200);
            this.btnNuovo.Name = "btnNuovo";
            this.btnNuovo.Size = new System.Drawing.Size(119, 43);
            this.btnNuovo.TabIndex = 2;
            this.btnNuovo.Text = "Nuovo";
            this.btnNuovo.UseVisualStyleBackColor = true;
            this.btnNuovo.Click += new System.EventHandler(this.btnNuovo_Click);
            // 
            // btnCancellare
            // 
            this.btnCancellare.Location = new System.Drawing.Point(649, 259);
            this.btnCancellare.Name = "btnCancellare";
            this.btnCancellare.Size = new System.Drawing.Size(119, 43);
            this.btnCancellare.TabIndex = 3;
            this.btnCancellare.Text = "Cancellare";
            this.btnCancellare.UseVisualStyleBackColor = true;
            this.btnCancellare.Click += new System.EventHandler(this.btnCancellare_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ristorante";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(649, 174);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(119, 20);
            this.txtCode.TabIndex = 5;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(191, 19);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(299, 20);
            this.txtSearch.TabIndex = 6;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // btnInsieme
            // 
            this.btnInsieme.Location = new System.Drawing.Point(649, 54);
            this.btnInsieme.Name = "btnInsieme";
            this.btnInsieme.Size = new System.Drawing.Size(119, 46);
            this.btnInsieme.TabIndex = 7;
            this.btnInsieme.Text = "Mettere insieme";
            this.btnInsieme.UseVisualStyleBackColor = true;
            this.btnInsieme.Click += new System.EventHandler(this.btnInsieme_Click);
            // 
            // Main
            // 
            this.ClientSize = new System.Drawing.Size(799, 343);
            this.Controls.Add(this.btnInsieme);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancellare);
            this.Controls.Add(this.btnNuovo);
            this.Controls.Add(this.btnApri);
            this.Controls.Add(this.listViewTavolo);
            this.Name = "Main";
            this.Text = "TotalPOS-UI";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ListView listViewTavolo;
        private System.Windows.Forms.ColumnHeader colId;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colCreatedAt;
        private System.Windows.Forms.ColumnHeader colTotal;
        private System.Windows.Forms.Button btnApri;
        private System.Windows.Forms.Button btnNuovo;
        private System.Windows.Forms.Button btnCancellare;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader colTotale;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnInsieme;
        private System.Windows.Forms.ColumnHeader colOpen;
    }
}
