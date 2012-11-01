#region License
// Copyright © 2012 Hedgehog Development, LLC (www.hhogdev.com)
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using HedgehogDevelopment.Scaas.Content.Remote.Configuration;
using HedgehogDevelopment.Scaas.Models;

namespace HedgehogDevelopment.Scaas.WinFormClient
{
    public partial class Article : UserControl
    {
        RemoteConfigurationSection _config = RemoteConfigurationSection.Create();

        public Article()
        {
            InitializeComponent();
        }

        private void Article_Load(object sender, EventArgs e)
        {
            Shrink();
        }

        public void ExpandContract_Click(object sender, EventArgs e)
        {
            Parent.SuspendLayout();

            if (ExpandContract.ImageIndex == 0)
            {
                Expand();
            }
            else
            {
                Shrink();
            }

            Parent.ResumeLayout(false);
            Parent.PerformLayout();
        }

        public void Expand()
        {
            ExpandContract.ImageIndex = 1;
            Height = 255;
            BottomBorder.Top = 254;

            foreach (Article ctrl in Parent.Controls)
            {
                if (ctrl != this)
                {
                    ctrl.Shrink();
                }
            }

        }

        protected void Shrink()
        {
            ExpandContract.ImageIndex = 0;
            Height = 33;
            BottomBorder.Top = 32;
        }

        public void BindContent(ArticleItem article)
        {
            Shrink();

            PromoTitle.Text = article.ArticleTitle;
            ArticleDate.Text = article.ArticleDate.ToShortDateString() + " " + article.ArticleDate.ToShortTimeString();

            if (!string.IsNullOrEmpty(article.ArticleCopy))
            {
                ArticleContent.DocumentText = article.ArticleCopy;
            }

            if (!string.IsNullOrEmpty(article.ArticleImage.Path))
            {
                using (HttpClient client = new HttpClient())
                {
                    string imageUrl = _config.BaseUri + "/" + article.ArticleImage.Path;

                    using (Bitmap bmp = new Bitmap(client.GetStreamAsync(imageUrl).Result))
                    {
                        float heightScale = (float)bmp.Height / (float)ArticleImage.Height;
                        float widthScale = (float)bmp.Width / (float)ArticleImage.Width;

                        float scale = Math.Max(heightScale, widthScale);

                        float newImageHeight = (float)bmp.Height / scale;
                        float newImageWidth = (float)bmp.Width / scale;

                        Bitmap scaledBitmap = new Bitmap(ArticleImage.Width, ArticleImage.Height);

                        using (Graphics g = Graphics.FromImage(scaledBitmap))
                        {
                            float top = 0;
                            float left = 0;
                            
                            //Center the image
                            if (newImageWidth < ArticleImage.Width)
                            {
                                left = (ArticleImage.Width - newImageWidth) / 2;
                            }

                            if (newImageHeight < ArticleImage.Height)
                            {
                                top = (ArticleImage.Height - newImageHeight) / 2;
                            }

                            g.DrawImage(bmp, left, top, newImageWidth, newImageHeight);
                        }

                        ArticleImage.Image = scaledBitmap;
                    }
                }
            }
        }
    }
}
