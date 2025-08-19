using HuaweiUnlocker.Modules.Core;
using HuaweiUnlocker.Modules.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HuaweiUnlocker.UI
{
    public class ModuleManagerForm : Form
    {
        private DataGridView grid;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnOpenFolder;

        public ModuleManagerForm()
        {
            Text = "Gerenciador de Loaders";
            Width = 820; Height = 420;
            StartPosition = FormStartPosition.CenterParent;

            grid = new DataGridView(){ Dock = DockStyle.Top, Height = 300, ReadOnly = true, AutoGenerateColumns=false, AllowUserToAddRows=false};
            grid.Columns.Add(new DataGridViewTextBoxColumn(){ HeaderText="Vendor", DataPropertyName="Vendor", Width=120});
            grid.Columns.Add(new DataGridViewTextBoxColumn(){ HeaderText="Model", DataPropertyName="Model", Width=120});
            grid.Columns.Add(new DataGridViewTextBoxColumn(){ HeaderText="Chipset", DataPropertyName="Chipset", Width=120});
            grid.Columns.Add(new DataGridViewTextBoxColumn(){ HeaderText="Loader", DataPropertyName="LoaderPath", Width=220});
            grid.Columns.Add(new DataGridViewTextBoxColumn(){ HeaderText="Rawprogram", DataPropertyName="RawprogramXmlPath", Width=180});

            btnAdd = new Button(){ Text="Importar Loader", Width=160, Left=10, Top=310 };
            btnRemove = new Button(){ Text="Remover", Width=120, Left=180, Top=310 };
            btnOpenFolder = new Button(){ Text="Abrir Pasta", Width=120, Left=310, Top=310 };

            btnAdd.Click += (s,e) => OnImport();
            btnRemove.Click += (s,e) => OnRemoveSelected();
            btnOpenFolder.Click += (s,e) => System.Diagnostics.Process.Start("explorer.exe", LoaderManager.LoadersFolder);

            Controls.AddRange(new Control[]{ grid, btnAdd, btnRemove, btnOpenFolder });

            AllowDrop = true;
            DragEnter += (s,e)=>{
                if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            };
            DragDrop += (s,e)=>{
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach(var f in files) LoaderManager.ImportInteractive(this, f);
                RefreshData();
            };

            LoaderManager.Load();
            RefreshData();
        }

        private void RefreshData()
        {
            grid.DataSource = null;
            grid.DataSource = LoaderManager.Manifest.Loaders.ToList();
        }

        private void OnImport()
        {
            using(var ofd = new OpenFileDialog())
            {
                ofd.Filter = "Loaders (*.mbn;*.elf;*.bin)|*.mbn;*.elf;*.bin|Todos (*.*)|*.*";
                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    LoaderManager.ImportInteractive(this, ofd.FileName);
                    RefreshData();
                }
            }
        }

        private void OnRemoveSelected()
        {
            if (grid.CurrentRow == null) return;
            var def = grid.CurrentRow.DataBoundItem as LoaderDefinition;
            if (def == null) return;
            var confirm = MessageBox.Show(this, "Remover loader selecionado?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;
            LoaderManager.Manifest.Loaders.RemoveAll(l => l.Id == def.Id);
            LoaderManager.Save();
            RefreshData();
        }
    }
}