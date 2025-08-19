using HuaweiUnlocker.Modules.Core;
using HuaweiUnlocker.Modules.Models;
using HuaweiUnlocker.UI;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HuaweiUnlocker
{
    public partial class Window : Form
    {
        private ToolStrip _strip;
        private ToolStripDropDownButton _modulesDrop;

        private void InitModulesUI()
        {
            try
            {
                // Branding mínimo
                this.Text = "Shyyxn Unlock";
            }
            catch {}

            // Top ToolStrip
            _strip = new ToolStrip(){ Dock = DockStyle.Top };
            _modulesDrop = new ToolStripDropDownButton("Modules");
            var btnManager = new ToolStripMenuItem("Gerenciar Loaders", null, (s,e)=> OpenManager());
            var btnOpen = new ToolStripMenuItem("Abrir Pasta de Loaders", null, (s,e)=> System.Diagnostics.Process.Start("explorer.exe", LoaderManager.LoadersFolder));
            _modulesDrop.DropDownItems.Add(btnManager);
            _modulesDrop.DropDownItems.Add(btnOpen);

            _strip.Items.Add(_modulesDrop);
            this.Controls.Add(_strip);

            // Ensure storage
            LoaderManager.EnsureFolders();
            LoaderManager.Load();
        }

        private void OpenManager()
        {
            using (var f = new ModuleManagerForm())
            {
                f.ShowDialog(this);
            }
        }
    }
}