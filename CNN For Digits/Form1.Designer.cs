namespace CNN_For_Digits
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
            pictureBox1 = new PictureBox();
            button1 = new Button();
            width = new Label();
            groupBox1 = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            height = new Label();
            menuStrip1 = new MenuStrip();
            файлыToolStripMenuItem = new ToolStripMenuItem();
            тестовыйНаборToolStripMenuItem = new ToolStripMenuItem();
            обучающийНаборToolStripMenuItem = new ToolStripMenuItem();
            нейросетьToolStripMenuItem = new ToolStripMenuItem();
            сохранитьМодельToolStripMenuItem = new ToolStripMenuItem();
            загрузитьМодельToolStripMenuItem = new ToolStripMenuItem();
            функцияАктивацииToolStripMenuItem = new ToolStripMenuItem();
            reluToolStripMenuItem = new ToolStripMenuItem();
            sigmoidToolStripMenuItem = new ToolStripMenuItem();
            groupBox2 = new GroupBox();
            label3 = new Label();
            button2 = new Button();
            textBox1 = new TextBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1.SuspendLayout();
            menuStrip1.SuspendLayout();
            groupBox2.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(39, 38);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(280, 280);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new Point(39, 324);
            button1.Name = "button1";
            button1.RightToLeft = RightToLeft.Yes;
            button1.Size = new Size(280, 23);
            button1.TabIndex = 1;
            button1.Text = "Достать Картинку";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Random_Img_button_Click;
            // 
            // width
            // 
            width.AutoSize = true;
            width.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            width.Location = new Point(161, 20);
            width.Name = "width";
            width.Size = new Size(41, 15);
            width.TabIndex = 2;
            width.Text = "width";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(height);
            groupBox1.Controls.Add(pictureBox1);
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(width);
            groupBox1.Location = new Point(12, 27);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(383, 527);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "MnistImages";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label2.Location = new Point(39, 461);
            label2.Name = "label2";
            label2.Size = new Size(119, 25);
            label2.TabIndex = 5;
            label2.Text = "info current";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 204);
            label1.Location = new Point(39, 382);
            label1.Name = "label1";
            label1.Size = new Size(105, 25);
            label1.TabIndex = 4;
            label1.Text = "info count";
            // 
            // height
            // 
            height.AutoSize = true;
            height.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            height.Location = new Point(325, 178);
            height.Name = "height";
            height.Size = new Size(45, 15);
            height.TabIndex = 3;
            height.Text = "height";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { файлыToolStripMenuItem, нейросетьToolStripMenuItem, функцияАктивацииToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1130, 24);
            menuStrip1.TabIndex = 5;
            menuStrip1.Text = "menuStrip1";
            // 
            // файлыToolStripMenuItem
            // 
            файлыToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { тестовыйНаборToolStripMenuItem, обучающийНаборToolStripMenuItem });
            файлыToolStripMenuItem.Name = "файлыToolStripMenuItem";
            файлыToolStripMenuItem.Size = new Size(57, 20);
            файлыToolStripMenuItem.Text = "Файлы";
            // 
            // тестовыйНаборToolStripMenuItem
            // 
            тестовыйНаборToolStripMenuItem.Name = "тестовыйНаборToolStripMenuItem";
            тестовыйНаборToolStripMenuItem.Size = new Size(181, 22);
            тестовыйНаборToolStripMenuItem.Text = "Тестовый набор";
            тестовыйНаборToolStripMenuItem.Click += тестовыйНаборToolStripMenuItem_Click;
            // 
            // обучающийНаборToolStripMenuItem
            // 
            обучающийНаборToolStripMenuItem.Name = "обучающийНаборToolStripMenuItem";
            обучающийНаборToolStripMenuItem.Size = new Size(181, 22);
            обучающийНаборToolStripMenuItem.Text = "Обучающий набор";
            обучающийНаборToolStripMenuItem.Click += обучающийНаборToolStripMenuItem_Click;
            // 
            // нейросетьToolStripMenuItem
            // 
            нейросетьToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { сохранитьМодельToolStripMenuItem, загрузитьМодельToolStripMenuItem });
            нейросетьToolStripMenuItem.Name = "нейросетьToolStripMenuItem";
            нейросетьToolStripMenuItem.Size = new Size(78, 20);
            нейросетьToolStripMenuItem.Text = "Нейросеть";
            // 
            // сохранитьМодельToolStripMenuItem
            // 
            сохранитьМодельToolStripMenuItem.Name = "сохранитьМодельToolStripMenuItem";
            сохранитьМодельToolStripMenuItem.Size = new Size(177, 22);
            сохранитьМодельToolStripMenuItem.Text = "Сохранить модель";
            сохранитьМодельToolStripMenuItem.Click += сохранитьМодельToolStripMenuItem_Click;
            // 
            // загрузитьМодельToolStripMenuItem
            // 
            загрузитьМодельToolStripMenuItem.Name = "загрузитьМодельToolStripMenuItem";
            загрузитьМодельToolStripMenuItem.Size = new Size(177, 22);
            загрузитьМодельToolStripMenuItem.Text = "Загрузить модель";
            загрузитьМодельToolStripMenuItem.Click += загрузитьМодельToolStripMenuItem_Click;
            // 
            // функцияАктивацииToolStripMenuItem
            // 
            функцияАктивацииToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { reluToolStripMenuItem, sigmoidToolStripMenuItem });
            функцияАктивацииToolStripMenuItem.Name = "функцияАктивацииToolStripMenuItem";
            функцияАктивацииToolStripMenuItem.Size = new Size(129, 20);
            функцияАктивацииToolStripMenuItem.Text = "Функция Активации";
            // 
            // reluToolStripMenuItem
            // 
            reluToolStripMenuItem.Name = "reluToolStripMenuItem";
            reluToolStripMenuItem.Size = new Size(180, 22);
            reluToolStripMenuItem.Text = "LeakyReLU";
            reluToolStripMenuItem.Click += reluToolStripMenuItem_Click;
            // 
            // sigmoidToolStripMenuItem
            // 
            sigmoidToolStripMenuItem.Name = "sigmoidToolStripMenuItem";
            sigmoidToolStripMenuItem.Size = new Size(180, 22);
            sigmoidToolStripMenuItem.Text = "Sigmoid";
            sigmoidToolStripMenuItem.Click += sigmoidToolStripMenuItem_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(button2);
            groupBox2.Controls.Add(textBox1);
            groupBox2.Location = new Point(401, 27);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(489, 527);
            groupBox2.TabIndex = 6;
            groupBox2.TabStop = false;
            groupBox2.Text = "Neural Neetwork";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(56, 245);
            label3.Name = "label3";
            label3.Size = new Size(38, 15);
            label3.TabIndex = 2;
            label3.Text = "label3";
            // 
            // button2
            // 
            button2.Location = new Point(56, 201);
            button2.Name = "button2";
            button2.Size = new Size(166, 23);
            button2.TabIndex = 1;
            button2.Text = "Предсказать";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Segoe UI", 72F, FontStyle.Regular, GraphicsUnit.Point, 204);
            textBox1.Location = new Point(56, 38);
            textBox1.MaxLength = 1;
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(166, 157);
            textBox1.TabIndex = 0;
            textBox1.Text = "1";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 544);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1130, 22);
            statusStrip1.TabIndex = 7;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 17);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1130, 566);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox1;
        private Button button1;
        private Label width;
        private GroupBox groupBox1;
        private Label height;
        private Label label1;
        private Label label2;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem файлыToolStripMenuItem;
        private ToolStripMenuItem тестовыйНаборToolStripMenuItem;
        private ToolStripMenuItem обучающийНаборToolStripMenuItem;
        private GroupBox groupBox2;
        private TextBox textBox1;
        private Button button2;
        private Label label3;
        private ToolStripMenuItem нейросетьToolStripMenuItem;
        private ToolStripMenuItem сохранитьМодельToolStripMenuItem;
        private ToolStripMenuItem загрузитьМодельToolStripMenuItem;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ToolStripMenuItem функцияАктивацииToolStripMenuItem;
        private ToolStripMenuItem reluToolStripMenuItem;
        private ToolStripMenuItem sigmoidToolStripMenuItem;
    }
}
