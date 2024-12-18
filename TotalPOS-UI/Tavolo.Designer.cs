namespace TotalPOS_UI
{
    partial class Tavolo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listViewTavolos = new System.Windows.Forms.ListView();
            this.colQuantita = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colProduct = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPrezzo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTotal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSalvare = new System.Windows.Forms.Button();
            this.btnRimuovi = new System.Windows.Forms.Button();
            this.btnPagare = new System.Windows.Forms.Button();
            this.btnCocaCola = new System.Windows.Forms.Button();
            this.btnPizza = new System.Windows.Forms.Button();
            this.lbTitulo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // listViewTavolos
            // 
            this.listViewTavolos.CheckBoxes = true;
            this.listViewTavolos.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colQuantita,
            this.colProduct,
            this.colPrezzo,
            this.colTotal});
            this.listViewTavolos.FullRowSelect = true;
            this.listViewTavolos.GridLines = true;
            this.listViewTavolos.HideSelection = false;
            this.listViewTavolos.Location = new System.Drawing.Point(12, 54);
            this.listViewTavolos.Name = "listViewTavolos";
            this.listViewTavolos.Size = new System.Drawing.Size(604, 248);
            this.listViewTavolos.TabIndex = 0;
            this.listViewTavolos.UseCompatibleStateImageBehavior = false;
            this.listViewTavolos.View = System.Windows.Forms.View.Details;
            // 
            // colQuantita
            // 
            this.colQuantita.Text = "Quantità";
            this.colQuantita.Width = 100;
            // 
            // colProduct
            // 
            this.colProduct.Text = "Product";
            this.colProduct.Width = 200;
            // 
            // colPrezzo
            // 
            this.colPrezzo.Text = "Prezzo";
            this.colPrezzo.Width = 100;
            // 
            // colTotal
            // 
            this.colTotal.Text = "Total";
            this.colTotal.Width = 100;
            // 
            // btnSalvare
            // 
            this.btnSalvare.Location = new System.Drawing.Point(649, 54);
            this.btnSalvare.Name = "btnSalvare";
            this.btnSalvare.Size = new System.Drawing.Size(119, 46);
            this.btnSalvare.TabIndex = 1;
            this.btnSalvare.Text = "Salvare";
            this.btnSalvare.UseVisualStyleBackColor = true;
            this.btnSalvare.Click += new System.EventHandler(this.btnSalvare_Click);
            // 
            // btnRimuovi
            // 
            this.btnRimuovi.Location = new System.Drawing.Point(649, 162);
            this.btnRimuovi.Name = "btnRimuovi";
            this.btnRimuovi.Size = new System.Drawing.Size(119, 43);
            this.btnRimuovi.TabIndex = 2;
            this.btnRimuovi.Text = "Rimuovi prodotto";
            this.btnRimuovi.UseVisualStyleBackColor = true;
            this.btnRimuovi.Click += new System.EventHandler(this.btnRimuovi_Click);
            // 
            // btnPagare
            // 
            this.btnPagare.Location = new System.Drawing.Point(649, 259);
            this.btnPagare.Name = "btnPagare";
            this.btnPagare.Size = new System.Drawing.Size(119, 43);
            this.btnPagare.TabIndex = 3;
            this.btnPagare.Text = "Pagare";
            this.btnPagare.UseVisualStyleBackColor = true;
            this.btnPagare.Click += new System.EventHandler(this.btnPagare_Click);
            // 
            // btnCocaCola
            // 
            this.btnCocaCola.Location = new System.Drawing.Point(12, 348);
            this.btnCocaCola.Name = "btnCocaCola";
            this.btnCocaCola.Size = new System.Drawing.Size(119, 46);
            this.btnCocaCola.TabIndex = 4;
            this.btnCocaCola.Text = "COCA COLA";
            this.btnCocaCola.UseVisualStyleBackColor = true;
            this.btnCocaCola.Click += new System.EventHandler(this.btnCocaCola_Click);
            // 
            // btnPizza
            // 
            this.btnPizza.Location = new System.Drawing.Point(196, 348);
            this.btnPizza.Name = "btnPizza";
            this.btnPizza.Size = new System.Drawing.Size(119, 46);
            this.btnPizza.TabIndex = 5;
            this.btnPizza.Text = "PIZZA";
            this.btnPizza.UseVisualStyleBackColor = true;
            this.btnPizza.Click += new System.EventHandler(this.btnPizza_Click);
            // 
            // lbTitulo
            // 
            this.lbTitulo.AutoSize = true;
            this.lbTitulo.Location = new System.Drawing.Point(12, 19);
            this.lbTitulo.Name = "lbTitulo";
            this.lbTitulo.Size = new System.Drawing.Size(0, 13);
            this.lbTitulo.TabIndex = 6;
            // 
            // Tavolo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbTitulo);
            this.Controls.Add(this.btnPizza);
            this.Controls.Add(this.btnCocaCola);
            this.Controls.Add(this.btnPagare);
            this.Controls.Add(this.btnRimuovi);
            this.Controls.Add(this.btnSalvare);
            this.Controls.Add(this.listViewTavolos);
            this.Name = "Tavolo";
            this.Text = "Tavolo";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewTavolos;
        private System.Windows.Forms.Button btnSalvare;
        private System.Windows.Forms.Button btnRimuovi;
        private System.Windows.Forms.Button btnPagare;
        private System.Windows.Forms.Button btnCocaCola;
        private System.Windows.Forms.Button btnPizza;
        private System.Windows.Forms.Label lbTitulo;
        private System.Windows.Forms.ColumnHeader colQuantita;
        private System.Windows.Forms.ColumnHeader colProduct;
        private System.Windows.Forms.ColumnHeader colPrezzo;
        private System.Windows.Forms.ColumnHeader colTotal;
    }
}