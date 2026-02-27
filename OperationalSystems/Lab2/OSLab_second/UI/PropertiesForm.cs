using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OSLab_second.Models;

namespace OSLab_second.UI
{
    public class PropertiesForm : Form
    {
        private Label lblInfo;

        public PropertiesForm(FileItem item)
        {
            InitializeComponent(item);
        }

        private void InitializeComponent(FileItem item)
        {
            this.Text = "Properties: " + item.Name;
            this.Size = new Size(300, 250);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            lblInfo = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopLeft,
                Padding = new Padding(10),
                Text = GetInfoText(item)
            };

            this.Controls.Add(lblInfo);
        }
        private string GetInfoText(FileItem item)
        {
            return $"Name: {item.Name}\n" +
                   $"Full path: {item.FullPath}\n" +
                   $"Type: {(item.IsFolder ? "Folder" : "File")}\n" +
                   $"Size: {(item.Size >= 0 ? item.Size.ToString() : "unknown")} byte\n" +
                   $"Created: {item.CreationTime}\n" +
                   $"Edited: {item.LastWriteTime}";
        }
    }
}
