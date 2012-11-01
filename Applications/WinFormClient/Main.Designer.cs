namespace HedgehogDevelopment.Scaas.WinFormClient
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.Articles = new System.Windows.Forms.FlowLayoutPanel();
            this.LoadingArticles = new System.Windows.Forms.Label();
            this.ArticleReload = new System.Windows.Forms.Timer(this.components);
            this.ArticleFetch = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Articles
            // 
            this.Articles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Articles.AutoScroll = true;
            this.Articles.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.Articles.Location = new System.Drawing.Point(12, 12);
            this.Articles.Name = "Articles";
            this.Articles.Size = new System.Drawing.Size(669, 437);
            this.Articles.TabIndex = 3;
            this.Articles.Resize += new System.EventHandler(this.Articles_Resize);
            // 
            // LoadingArticles
            // 
            this.LoadingArticles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadingArticles.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadingArticles.Location = new System.Drawing.Point(0, 0);
            this.LoadingArticles.Name = "LoadingArticles";
            this.LoadingArticles.Size = new System.Drawing.Size(694, 449);
            this.LoadingArticles.TabIndex = 4;
            this.LoadingArticles.Text = "Loading Articles...";
            this.LoadingArticles.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ArticleReload
            // 
            this.ArticleReload.Enabled = true;
            this.ArticleReload.Tick += new System.EventHandler(this.ArticleReload_Tick);
            // 
            // ArticleFetch
            // 
            this.ArticleFetch.Enabled = true;
            this.ArticleFetch.Interval = 5000;
            this.ArticleFetch.Tick += new System.EventHandler(this.ArticleFetch_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 451);
            this.Controls.Add(this.LoadingArticles);
            this.Controls.Add(this.Articles);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.Text = "Sitecore Symposium Articles";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel Articles;
        private System.Windows.Forms.Label LoadingArticles;
        private System.Windows.Forms.Timer ArticleReload;
        private System.Windows.Forms.Timer ArticleFetch;

    }
}

