namespace HedgehogDevelopment.Scaas.WinFormClient
{
    partial class Article
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Article));
            this.ExpandContract = new System.Windows.Forms.Button();
            this.ButtonImages = new System.Windows.Forms.ImageList(this.components);
            this.PromoTitle = new System.Windows.Forms.Label();
            this.ArticleImage = new System.Windows.Forms.PictureBox();
            this.ArticleDate = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BottomBorder = new System.Windows.Forms.PictureBox();
            this.ArticleContent = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.ArticleImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomBorder)).BeginInit();
            this.SuspendLayout();
            // 
            // ExpandContract
            // 
            this.ExpandContract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ExpandContract.ImageIndex = 0;
            this.ExpandContract.ImageList = this.ButtonImages;
            this.ExpandContract.Location = new System.Drawing.Point(604, 3);
            this.ExpandContract.Name = "ExpandContract";
            this.ExpandContract.Size = new System.Drawing.Size(26, 26);
            this.ExpandContract.TabIndex = 0;
            this.ExpandContract.UseVisualStyleBackColor = true;
            this.ExpandContract.Click += new System.EventHandler(this.ExpandContract_Click);
            // 
            // ButtonImages
            // 
            this.ButtonImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ButtonImages.ImageStream")));
            this.ButtonImages.TransparentColor = System.Drawing.Color.Transparent;
            this.ButtonImages.Images.SetKeyName(0, "navigate_down2.png");
            this.ButtonImages.Images.SetKeyName(1, "navigate_up2.png");
            // 
            // PromoTitle
            // 
            this.PromoTitle.AutoSize = true;
            this.PromoTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PromoTitle.Location = new System.Drawing.Point(3, 3);
            this.PromoTitle.Name = "PromoTitle";
            this.PromoTitle.Size = new System.Drawing.Size(53, 20);
            this.PromoTitle.TabIndex = 1;
            this.PromoTitle.Text = "[Title]";
            // 
            // ArticleImage
            // 
            this.ArticleImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ArticleImage.Location = new System.Drawing.Point(0, 82);
            this.ArticleImage.Name = "ArticleImage";
            this.ArticleImage.Size = new System.Drawing.Size(167, 167);
            this.ArticleImage.TabIndex = 3;
            this.ArticleImage.TabStop = false;
            // 
            // ArticleDate
            // 
            this.ArticleDate.AutoSize = true;
            this.ArticleDate.Location = new System.Drawing.Point(43, 35);
            this.ArticleDate.Name = "ArticleDate";
            this.ArticleDate.Size = new System.Drawing.Size(36, 13);
            this.ArticleDate.TabIndex = 4;
            this.ArticleDate.Text = "[Date]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date:";
            // 
            // BottomBorder
            // 
            this.BottomBorder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BottomBorder.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BottomBorder.Location = new System.Drawing.Point(0, 32);
            this.BottomBorder.Name = "BottomBorder";
            this.BottomBorder.Size = new System.Drawing.Size(633, 1);
            this.BottomBorder.TabIndex = 7;
            this.BottomBorder.TabStop = false;
            // 
            // ArticleContent
            // 
            this.ArticleContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ArticleContent.Location = new System.Drawing.Point(173, 39);
            this.ArticleContent.MinimumSize = new System.Drawing.Size(20, 20);
            this.ArticleContent.Name = "ArticleContent";
            this.ArticleContent.Size = new System.Drawing.Size(457, 210);
            this.ArticleContent.TabIndex = 8;
            // 
            // Article
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ArticleContent);
            this.Controls.Add(this.BottomBorder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ArticleDate);
            this.Controls.Add(this.ArticleImage);
            this.Controls.Add(this.PromoTitle);
            this.Controls.Add(this.ExpandContract);
            this.Name = "Article";
            this.Size = new System.Drawing.Size(633, 252);
            this.Load += new System.EventHandler(this.Article_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ArticleImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomBorder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ExpandContract;
        private System.Windows.Forms.Label PromoTitle;
        private System.Windows.Forms.PictureBox ArticleImage;
        private System.Windows.Forms.ImageList ButtonImages;
        private System.Windows.Forms.Label ArticleDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox BottomBorder;
        private System.Windows.Forms.WebBrowser ArticleContent;
    }
}
