﻿namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            textBox1 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            textBox2 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(596, 267);
            button1.Name = "button1";
            button1.Size = new Size(140, 52);
            button1.TabIndex = 0;
            button1.Text = "Set";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.AllowDrop = true;
            textBox1.Location = new Point(162, 102);
            textBox1.Name = "textBox1";
            textBox1.Text = "D:\\小Data小\\Check_大大大大.ini";
            textBox1.Size = new Size(574, 26);
            textBox1.TabIndex = 1;
            textBox1.DragDrop += textBox1_DragDrop;
            textBox1.DragEnter += textBox1_DragEnter;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(72, 109);
            label1.Name = "label1";
            label1.Size = new Size(48, 19);
            label1.TabIndex = 2;
            label1.Text = "Ini File";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(72, 162);
            label2.Name = "label2";
            label2.Size = new Size(52, 19);
            label2.TabIndex = 2;
            label2.Text = "csv File";
            // 
            // textBox2
            // 
            textBox2.AllowDrop = true;
            textBox2.Location = new Point(162, 155);
            textBox2.Name = "textBox2";
            textBox2.Text = "D:\\小Data小\\Check_大大大大csv.csv";
            textBox2.Size = new Size(574, 26);
            textBox2.TabIndex = 1;
            textBox2.DragDrop += textBox1_DragDrop;
            textBox2.DragEnter += textBox1_DragEnter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox2;
    }
}
