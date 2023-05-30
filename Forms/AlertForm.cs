using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace FSCmd.Forms;

public partial class AlertForm : Form
{
	string labelText;

	//=============================================================================
	/// <summary></summary>
	public AlertForm ()
	{
		InitializeComponent ();
	}

	//=============================================================================
	/// <summary></summary>
	public void setParameters (string text)
	{
		labelText = text;
		if (String.IsNullOrWhiteSpace (labelText)) 
		{
			labelText = "Alert set without message!";
		}
	}

	//=============================================================================
	/// <summary></summary>
	private void AlertForm_Shown (object sender, EventArgs e)
	{
		m_label.Text = labelText;

		// center text in label
		m_label.Left = (this.ClientSize.Width - m_label.Width) / 2;
	}

	//=============================================================================
	/// <summary></summary>
	private void m_close_Click (object sender, EventArgs e)
	{
		// close the dialog
		Close ();
	}
}
