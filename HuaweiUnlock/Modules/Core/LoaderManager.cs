using HuaweiUnlocker.Modules.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace HuaweiUnlocker.Modules.Core
{
    public static class LoaderManager
    {
        public static readonly string Root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules");
        public static readonly string LoadersFolder = Path.Combine(Root, "Loaders");
        public static readonly string ManifestPath = Path.Combine(Root, "loaders.json");

        private static LoaderManifest _manifest = new LoaderManifest();
        private static readonly JavaScriptSerializer _json = new JavaScriptSerializer();

        public static LoaderManifest Manifest => _manifest;

        public static void EnsureFolders()
        {
            if (!Directory.Exists(Root)) Directory.CreateDirectory(Root);
            if (!Directory.Exists(LoadersFolder)) Directory.CreateDirectory(LoadersFolder);
            if (!File.Exists(ManifestPath))
            {
                Save();
            }
        }

        public static void Load()
        {
            EnsureFolders();
            try
            {
                if (File.Exists(ManifestPath))
                {
                    var txt = File.ReadAllText(ManifestPath);
                    _manifest = _json.Deserialize<LoaderManifest>(txt) ?? new LoaderManifest();
                }
            }
            catch
            {
                _manifest = new LoaderManifest();
            }
        }

        public static void Save()
        {
            EnsureFolders();
            var txt = _json.Serialize(_manifest);
            File.WriteAllText(ManifestPath, txt);
        }

        public static void ImportInteractive(IWin32Window owner, string filePath)
        {
            // Ask metadata quickly
            var vendor = Prompt("Vendor/Marca (ex: Samsung, Xiaomi, Motorola, Huawei):", "Add Loader", "Generic");
            if (vendor == null) return;
            var model = Prompt("Modelo (ex: G50, SM-A015, etc):", "Add Loader", "Unknown");
            if (model == null) return;
            var chipsetStr = Prompt("Chipset [Qualcomm, MediaTek, HiSilicon]:", "Add Loader", "Qualcomm");
            if (chipsetStr == null) return;
            var chipset = ParseChipset(chipsetStr);

            // Optional rawprogram and patch XMLs
            var raw = Prompt("Caminho rawprogram0.xml (vazio se não houver):", "Add Loader", "");
            var patch = Prompt("Caminho patch0.xml (vazio se não houver):", "Add Loader", "");

            // Copy files into structured folder
            var vendorFolder = Path.Combine(LoadersFolder, chipset.ToString(), vendor);
            Directory.CreateDirectory(vendorFolder);
            var loaderDest = Path.Combine(vendorFolder, Path.GetFileName(filePath));
            File.Copy(filePath, loaderDest, true);

            string rawDest = string.IsNullOrWhiteSpace(raw) ? null : Path.Combine(vendorFolder, Path.GetFileName(raw));
            string patchDest = string.IsNullOrWhiteSpace(patch) ? null : Path.Combine(vendorFolder, Path.GetFileName(patch));
            if (rawDest != null && File.Exists(raw)) File.Copy(raw, rawDest, true);
            if (patchDest != null && File.Exists(patch)) File.Copy(patch, patchDest, true);

            var def = new LoaderDefinition
            {
                Vendor = vendor,
                Model = model,
                Chipset = chipset,
                LoaderPath = loaderDest,
                RawprogramXmlPath = rawDest,
                PatchXmlPath = patchDest,
                AllowRead = true,
                AllowWrite = true,
                AllowErase = true
            };
            _manifest.Loaders.Add(def);
            Save();
            MessageBox.Show(owner, "Loader importado.", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static List<LoaderDefinition> ListByVendor(string vendor = null)
        {
            if (string.IsNullOrWhiteSpace(vendor)) return _manifest.Loaders.ToList();
            return _manifest.Loaders.Where(l => string.Equals(l.Vendor, vendor, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public static ChipsetType ParseChipset(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return ChipsetType.Unknown;
            s = s.Trim().ToLowerInvariant();
            if (s.StartsWith("qual")) return ChipsetType.Qualcomm;
            if (s.StartsWith("med")) return ChipsetType.MediaTek;
            if (s.StartsWith("hi")) return ChipsetType.HiSilicon;
            return ChipsetType.Unknown;
        }

        private static string Prompt(string text, string caption, string def)
        {
            // Basic prompt using InputBox-like form
            using (var form = new Form())
            using (var txt = new TextBox())
            using (var ok = new Button())
            using (var cancel = new Button())
            using (var lbl = new Label())
            {
                form.Width = 520; form.Height = 160; form.Text = caption; form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.StartPosition = FormStartPosition.CenterParent; form.MaximizeBox = false; form.MinimizeBox = false;
                lbl.Text = text; lbl.Left = 10; lbl.Top = 10; lbl.Width = 480;
                txt.Left = 10; txt.Top = 40; txt.Width = 480; txt.Text = def ?? "";
                ok.Text = "OK"; ok.Left = 320; ok.Top = 80; ok.DialogResult = DialogResult.OK;
                cancel.Text = "Cancelar"; cancel.Left = 400; cancel.Top = 80; cancel.DialogResult = DialogResult.Cancel;
                form.Controls.AddRange(new Control[] { lbl, txt, ok, cancel });
                form.AcceptButton = ok; form.CancelButton = cancel;

                var result = form.ShowDialog();
                if (result == DialogResult.OK) return txt.Text;
                return null;
            }
        }
    }
}