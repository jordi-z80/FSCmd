namespace FSCmd.Forms
{
	partial class AlertForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose (bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose ();
			}
			base.Dispose (disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent ()
		{
			m_label = new Label ();
			m_close = new Button ();
			SuspendLayout ();
			// 
			// m_label
			// 
			m_label.AutoSize = true;
			m_label.Location = new Point (12, 25);
			m_label.Name = "m_label";
			m_label.Size = new Size (32, 15);
			m_label.TabIndex = 0;
			m_label.Text = "label";
			// 
			// m_close
			// 
			m_close.Location = new Point (191, 113);
			m_close.Name = "m_close";
			m_close.Size = new Size (75, 23);
			m_close.TabIndex = 1;
			m_close.Text = "Close";
			m_close.UseVisualStyleBackColor = true;
			m_close.Click += m_close_Click;
			// 
			// AlertForm
			// 
			AutoScaleDimensions = new SizeF (7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size (460, 149);
			Controls.Add (m_close);
			Controls.Add (m_label);
			Name = "AlertForm";
			Text = "Alert!";
			Shown += AlertForm_Shown;
			ResumeLayout (false);
			PerformLayout ();
		}

		#endregion

		private Label m_label;
		private Button m_close;
	}
}