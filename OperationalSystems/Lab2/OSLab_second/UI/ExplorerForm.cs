using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using OSLab_second.Core;
using OSLab_second.Models;

namespace OSLab_second.UI
{
    public class ExplorerForm : Form
    {
        private TextBox tbPath;
        private Button btnGo;
        private ListView lvFiles;
        private ContextMenuStrip ctxMenu;
        private ImageList imgList;

        private string _clipboardSourcePath = null;
        private bool _clipboardIsCut = false;

        public ExplorerForm()
        {
            InitializeComponent();
            LoadDirectory(Environment.CurrentDirectory);
        }
        private void InitializeComponent()
        {
            this.Text = "Reinvented bycicle";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel panelTop = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(5) };

            tbPath = new TextBox { Dock = DockStyle.Fill, Text = Environment.CurrentDirectory };
            tbPath.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) LoadDirectory(tbPath.Text); };

            btnGo = new Button { Text = "Go", Dock = DockStyle.Right, Width = 80 };
            btnGo.Click += (s, e) => LoadDirectory(tbPath.Text);

            panelTop.Controls.Add(tbPath);
            panelTop.Controls.Add(btnGo);

            // Список файлов
            lvFiles = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            lvFiles.Columns.Add("Name", 200);
            lvFiles.Columns.Add("Date of change", 150);
            lvFiles.Columns.Add("Type", 100);
            lvFiles.Columns.Add("Size", 100);

            lvFiles.MouseDoubleClick += LvFiles_MouseDoubleClick;
            lvFiles.MouseDown += LvFiles_MouseDown;

            // Иконки
            imgList = new ImageList();
            imgList.Images.Add(SystemIcons.Application.ToBitmap());
            lvFiles.SmallImageList = imgList;
            lvFiles.LargeImageList = imgList;

            // Контекстное меню
            ctxMenu = new ContextMenuStrip();
            var miOpen = new ToolStripMenuItem("Open");
            var miRename = new ToolStripMenuItem("Rename");
            var miCopy = new ToolStripMenuItem("Copy");
            var miCut = new ToolStripMenuItem("Cut");
            var miPaste = new ToolStripMenuItem("Paste");
            var miProps = new ToolStripMenuItem("Properties");

            // Привязка событий меню (через Compatiblity wrapper или добавление в Items)
            // Для ContextMenuStrip используем ToolStripItems
            ctxMenu.Items.Add("Open", null, (s, e) => OpenSelectedItem());
            ctxMenu.Items.Add("Rename", null, (s, e) => RenameSelectedItem());
            ctxMenu.Items.Add("Copy", null, (s, e) => CopySelectedItem());
            ctxMenu.Items.Add("Cut", null, (s, e) => CutSelectedItem());
            ctxMenu.Items.Add("Paste", null, (s, e) => PasteFromClipboard());
            ctxMenu.Items.Add("Properties", null, (s, e) => ShowPropertiesSelectedItem());

            lvFiles.ContextMenuStrip = ctxMenu;
            this.ContextMenuStrip = ctxMenu; // Для клика по пустому месту

            this.Controls.Add(lvFiles);
            this.Controls.Add(panelTop);
        }

        private void LoadDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                MessageBox.Show("Directory not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tbPath.Text = path;
            lvFiles.Items.Clear();
            var items = FileManager.GetDirectoryContent(path);

            foreach (var item in items)
            {
                var lvi = new ListViewItem(item.Name);
                lvi.SubItems.Add(item.LastWriteTime.ToString());
                lvi.SubItems.Add(item.IsFolder ? "Folder" : "File");
                lvi.SubItems.Add(item.IsFolder ? "-" : item.Size.ToString());
                lvi.ImageIndex = item.IsFolder ? 0 : 1;
                lvi.Tag = item; // Сохраняем модель в теге
                lvFiles.Items.Add(lvi);
            }
        }

        private void LvFiles_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lvFiles.SelectedItems.Count > 0)
                OpenSelectedItem();
        }

        private void LvFiles_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hit = lvFiles.HitTest(e.Location);
                if (hit.Item == null)
                {
                    // Клик по пустому месту - отключаем пункты работы с файлом
                    ctxMenu.Items[0].Enabled = false; // Открыть
                    ctxMenu.Items[1].Enabled = false; // Переименовать
                    ctxMenu.Items[2].Enabled = false; // Копировать
                    ctxMenu.Items[3].Enabled = false; // Вырезать
                    ctxMenu.Items[4].Enabled = true;  // Вставить
                    ctxMenu.Items[5].Enabled = false; // Свойства
                }
                else
                {
                    lvFiles.SelectedItems.Clear();
                    hit.Item.Selected = true;
                    // Включаем пункты работы с файлом
                    for (int i = 0; i < 4; i++) ctxMenu.Items[i].Enabled = true;
                    ctxMenu.Items[5].Enabled = true;
                }
            }
        }

        private void OpenSelectedItem()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var item = (FileItem)lvFiles.SelectedItems[0].Tag;
            if (item.IsFolder)
            {
                LoadDirectory(item.FullPath);
            }
            else
            {
                try { System.Diagnostics.Process.Start(item.FullPath); }
                catch { MessageBox.Show("Couldn't open the file"); }
            }
        }

        private void RenameSelectedItem()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var item = (FileItem)lvFiles.SelectedItems[0].Tag;

            using (var inputForm = new Form())
            {
                inputForm.Text = "Rename";
                inputForm.Size = new Size(300, 150);
                var tb = new TextBox { Location = new Point(10, 10), Width = 260, Text = item.Name };
                var btn = new Button { Text = "OK", Location = new Point(10, 40), DialogResult = DialogResult.OK };
                inputForm.AcceptButton = btn;
                inputForm.Controls.Add(tb);
                inputForm.Controls.Add(btn);

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileManager.Rename(item.FullPath, tb.Text);
                        LoadDirectory(tbPath.Text);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void CopySelectedItem()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var item = (FileItem)lvFiles.SelectedItems[0].Tag;
            _clipboardSourcePath = item.FullPath;
            _clipboardIsCut = false;
        }

        private void CutSelectedItem()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var item = (FileItem)lvFiles.SelectedItems[0].Tag;
            _clipboardSourcePath = item.FullPath;
            _clipboardIsCut = true;
        }

        private void PasteFromClipboard()
        {
            if (_clipboardSourcePath == null) return;
            // Вставка в текущую директорию
            string destName = Path.GetFileName(_clipboardSourcePath);
            string destPath = Path.Combine(tbPath.Text, destName);

            try
            {
                if (_clipboardIsCut)
                {
                    FileManager.Move(_clipboardSourcePath, destPath);
                    _clipboardSourcePath = null;
                }
                else
                {
                    FileManager.Copy(_clipboardSourcePath, destPath);
                }
                LoadDirectory(tbPath.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowPropertiesSelectedItem()
        {
            if (lvFiles.SelectedItems.Count == 0) return;
            var item = (FileItem)lvFiles.SelectedItems[0].Tag;
            var propsForm = new PropertiesForm(item);
            propsForm.ShowDialog();
        }
    }
}
