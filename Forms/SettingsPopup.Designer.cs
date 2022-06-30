using System.ComponentModel;

namespace ClausaComm.Forms;

partial class SettingsPopup
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private IContainer components = null;

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
            this.RunAtStartupCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // RunAtStartupCheckbox
            // 
            this.RunAtStartupCheckbox.AutoSize = true;
            this.RunAtStartupCheckbox.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.RunAtStartupCheckbox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.RunAtStartupCheckbox.Location = new System.Drawing.Point(12, 94);
            this.RunAtStartupCheckbox.Name = "RunAtStartupCheckbox";
            this.RunAtStartupCheckbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RunAtStartupCheckbox.Size = new System.Drawing.Size(180, 25);
            this.RunAtStartupCheckbox.TabIndex = 1;
            this.RunAtStartupCheckbox.Text = "Run at system startup";
            this.RunAtStartupCheckbox.UseVisualStyleBackColor = true;
            // 
            // SettingsPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 450);
            //this.Controls.Add(this.RunAtStartupCheckbox);
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "SettingsPopup";
            this.Text = "SettingsPopup";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.CheckBox RunAtStartupCheckbox;
}