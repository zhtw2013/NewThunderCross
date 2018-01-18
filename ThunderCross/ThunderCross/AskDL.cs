﻿using System;
using System.Windows.Forms;
using ByteSizeLib;
using System.IO;
using System.Globalization;

namespace ThunderCross
{
	partial class AskDL : Form
	{
		public DLAgent RetAgent;
		public bool saveDownloadType = false;
		public bool saveForSiteOnly = false;
		public AskDL(DLRequest r)
		{
			InitializeComponent();
			if (CultureInfo.CurrentCulture.Equals(CultureInfo.GetCultureInfo("zh-cn")))
				this.Font = new System.Drawing.Font("微软雅黑", 10);
			label_fileurl.Text = label_fileurl.Text + new Uri(r.Url).Host;
			textBox1_filename.Text = r.Filename;
			label_filetypesize.Text = label_filetypesize.Text + string.Format("{0} ({1})",r.ContentType,ByteSize.Parse(r.ContentLength + "B").ToString());
			picture_icon.Image = Etier.IconHelper.IconReader.GetFileIcon(Path.GetExtension(r.Filename), Etier.IconHelper.IconReader.IconSize.Large, false).ToBitmap();
			if (r.DefaultDM == DLAgent.Customized.ToString())
			{
				comboBox_dm.Items.Add(string.Format("{0} ({1})",r.CustomizedDM[0].Name, DLAgent.Customized.ToString()));
			}
			else
			{
				comboBox_dm.Items.Add(Enum.Parse(typeof(DLAgent), r.DefaultDM));
			}
			comboBox_dm.SelectedIndex = 0;
			radioButton_external.Checked = true;
			foreach (var dm in DownloadManager.DMList)
			{
				if (dm!=r.DefaultDM && ((IDownloadManager)Activator.CreateInstance(Type.GetType("ThunderCross.DM"+dm),true)).Valid())
				{
					comboBox_dm.Items.Add(Enum.Parse(typeof(DLAgent), dm));
				}
			}
		}

		private void button_Cancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.RetAgent = DLAgent.Cancel;
			this.Close();
		}

		private void button_OK_Click(object sender, EventArgs e)
		{
			if (radioButton_external.Checked)
			{
				if (comboBox_dm.SelectedItem.ToString().EndsWith("(" + DLAgent.Customized.ToString() + ")"))
					RetAgent = DLAgent.Customized;
				else
					RetAgent = (DLAgent)comboBox_dm.SelectedItem;
			}
			else if (radioButton_default.Checked)
				RetAgent = DLAgent.Default;
			if(checkBox_saveOption.Checked)
			{
				if (checkBox_saveForSite.Checked)
					saveForSiteOnly = true;
				saveDownloadType = true;
			}
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void checkBox_saveOption_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox_saveOption.Checked)
				checkBox_saveForSite.Enabled = true;
			else
				checkBox_saveForSite.Enabled = false;
		}
	}
}
