
namespace BackupMap
{
    partial class Form1
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
            this.TitlePanal = new System.Windows.Forms.Panel();
            this.Closebtn = new System.Windows.Forms.PictureBox();
            this.Logo = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.OKbtn = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.OpenSource_label = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TickTime_input = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SavePath_input = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.GetPath = new System.Windows.Forms.Button();
            this.Label = new System.Windows.Forms.Label();
            this.Isleapfrog_check = new System.Windows.Forms.CheckBox();
            this.TitlePanal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Closebtn)).BeginInit();
            this.Logo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitlePanal
            // 
            this.TitlePanal.BackColor = System.Drawing.Color.Transparent;
            this.TitlePanal.Controls.Add(this.label1);
            this.TitlePanal.Controls.Add(this.pictureBox2);
            this.TitlePanal.Controls.Add(this.Closebtn);
            this.TitlePanal.Location = new System.Drawing.Point(0, 0);
            this.TitlePanal.Name = "TitlePanal";
            this.TitlePanal.Size = new System.Drawing.Size(700, 35);
            this.TitlePanal.TabIndex = 1;
            this.TitlePanal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TitlePanal_MouseDown);
            // 
            // Closebtn
            // 
            this.Closebtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.Closebtn.Image = global::BackupMap.Properties.Resources.exit_white;
            this.Closebtn.Location = new System.Drawing.Point(665, 0);
            this.Closebtn.Name = "Closebtn";
            this.Closebtn.Size = new System.Drawing.Size(35, 35);
            this.Closebtn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Closebtn.TabIndex = 0;
            this.Closebtn.TabStop = false;
            this.Closebtn.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // Logo
            // 
            this.Logo.BackColor = System.Drawing.Color.Transparent;
            this.Logo.BackgroundImage = global::BackupMap.Properties.Resources.Logobgimg;
            this.Logo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Logo.Controls.Add(this.pictureBox1);
            this.Logo.Location = new System.Drawing.Point(700, 0);
            this.Logo.Name = "Logo";
            this.Logo.Size = new System.Drawing.Size(700, 400);
            this.Logo.TabIndex = 0;
            this.Logo.Paint += new System.Windows.Forms.PaintEventHandler(this.Logo_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::BackupMap.Properties.Resources.BackupImage;
            this.pictureBox1.Location = new System.Drawing.Point(386, 70);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(242, 251);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // OKbtn
            // 
            this.OKbtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(80)))), ((int)(((byte)(107)))));
            this.OKbtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(199)))), ((int)(((byte)(255)))));
            this.OKbtn.FlatAppearance.BorderSize = 2;
            this.OKbtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(204)))), ((int)(((byte)(255)))));
            this.OKbtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(121)))), ((int)(((byte)(161)))));
            this.OKbtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.OKbtn.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OKbtn.ForeColor = System.Drawing.Color.White;
            this.OKbtn.Location = new System.Drawing.Point(565, 351);
            this.OKbtn.Name = "OKbtn";
            this.OKbtn.Size = new System.Drawing.Size(123, 37);
            this.OKbtn.TabIndex = 2;
            this.OKbtn.Text = "完  成";
            this.OKbtn.UseVisualStyleBackColor = false;
            this.OKbtn.Click += new System.EventHandler(this.OKbtn_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Image = global::BackupMap.Properties.Resources.BackupImage;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(35, 35);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(45, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = ">>服务器存档备份配置面板";
            // 
            // OpenSource_label
            // 
            this.OpenSource_label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.OpenSource_label.AutoSize = true;
            this.OpenSource_label.BackColor = System.Drawing.Color.Transparent;
            this.OpenSource_label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.OpenSource_label.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.OpenSource_label.Location = new System.Drawing.Point(12, 371);
            this.OpenSource_label.Name = "OpenSource_label";
            this.OpenSource_label.Size = new System.Drawing.Size(446, 17);
            this.OpenSource_label.TabIndex = 3;
            this.OpenSource_label.Text = "本插件依赖\"Tools.dll\",请下载插件时同时下载附带的\"Tools.dll\"于同目录-开源节流";
            this.OpenSource_label.Click += new System.EventHandler(this.OpenSource_label_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.Isleapfrog_check);
            this.panel1.Controls.Add(this.GetPath);
            this.panel1.Controls.Add(this.Label);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.SavePath_input);
            this.panel1.Controls.Add(this.TickTime_input);
            this.panel1.Location = new System.Drawing.Point(12, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(446, 320);
            this.panel1.TabIndex = 4;
            // 
            // TickTime_input
            // 
            this.TickTime_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TickTime_input.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TickTime_input.Location = new System.Drawing.Point(8, 39);
            this.TickTime_input.Name = "TickTime_input";
            this.TickTime_input.Size = new System.Drawing.Size(396, 22);
            this.TickTime_input.TabIndex = 0;
            this.TickTime_input.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(5, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(399, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "循环备份时间,每隔此时间插件备份一次存档[单位ms,时间不要小于一分钟]";
            // 
            // SavePath_input
            // 
            this.SavePath_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.SavePath_input.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SavePath_input.Location = new System.Drawing.Point(8, 100);
            this.SavePath_input.Name = "SavePath_input";
            this.SavePath_input.Size = new System.Drawing.Size(396, 22);
            this.SavePath_input.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(5, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "要保存存档的保存目录";
            // 
            // GetPath
            // 
            this.GetPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetPath.Location = new System.Drawing.Point(407, 100);
            this.GetPath.Name = "GetPath";
            this.GetPath.Size = new System.Drawing.Size(35, 23);
            this.GetPath.TabIndex = 2;
            this.GetPath.Text = "<<";
            this.GetPath.UseVisualStyleBackColor = true;
            this.GetPath.Click += new System.EventHandler(this.GetPath_Click);
            // 
            // Label
            // 
            this.Label.AutoSize = true;
            this.Label.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label.ForeColor = System.Drawing.Color.White;
            this.Label.Location = new System.Drawing.Point(5, 141);
            this.Label.Name = "Label";
            this.Label.Size = new System.Drawing.Size(260, 17);
            this.Label.TabIndex = 1;
            this.Label.Text = "当要保存存档的保存目录已存在的是否是否覆盖";
            // 
            // Isleapfrog_check
            // 
            this.Isleapfrog_check.AutoSize = true;
            this.Isleapfrog_check.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Isleapfrog_check.Location = new System.Drawing.Point(8, 162);
            this.Isleapfrog_check.Name = "Isleapfrog_check";
            this.Isleapfrog_check.Size = new System.Drawing.Size(56, 24);
            this.Isleapfrog_check.TabIndex = 3;
            this.Isleapfrog_check.Text = "跳过";
            this.Isleapfrog_check.UseVisualStyleBackColor = true;
            this.Isleapfrog_check.CheckedChanged += new System.EventHandler(this.Isleapfrog_check_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::BackupMap.Properties.Resources.formbgimg;
            this.ClientSize = new System.Drawing.Size(700, 400);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.OpenSource_label);
            this.Controls.Add(this.OKbtn);
            this.Controls.Add(this.Logo);
            this.Controls.Add(this.TitlePanal);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TitlePanal.ResumeLayout(false);
            this.TitlePanal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Closebtn)).EndInit();
            this.Logo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel Logo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel TitlePanal;
        private System.Windows.Forms.PictureBox Closebtn;
        private System.Windows.Forms.Button OKbtn;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label OpenSource_label;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox TickTime_input;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox SavePath_input;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button GetPath;
        private System.Windows.Forms.Label Label;
        private System.Windows.Forms.CheckBox Isleapfrog_check;
    }
}