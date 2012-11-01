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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using HedgehogDevelopment.Scaas.Content;
using HedgehogDevelopment.Scaas.Content.Remote;
using HedgehogDevelopment.Scaas.Models;

namespace HedgehogDevelopment.Scaas.WinFormClient
{
    public partial class Main : Form
    {
        private const string ARTICLE_REPO_PATH = "/sitecore/content/Scaas-Demo/Content/Articles";

        IContentNavigator _contentNavigator = new RemoteSitecoreContentService();
        IEnumerable<ArticleItem> _articles = new List<ArticleItem>();
        bool _fetchingArticles = false;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Fetch articles for the first time
            ArticleFetch_Tick(null, null);
        }

        /// <summary>
        /// Make sure the article controls stretch to fill the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Articles_Resize(object sender, EventArgs e)
        {
            foreach (Control ctrl in Articles.Controls)
            {
                ctrl.MinimumSize = new System.Drawing.Size(Articles.ClientSize.Width - 10, 0);
            }
        }

        /// <summary>
        /// Looks for a new article list by checking the count of articles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArticleReload_Tick(object sender, EventArgs e)
        {
            if (_articles.Count() != Articles.Controls.Count)
            {
                int width = Articles.ClientSize.Width;

                Articles.SuspendLayout();
                Articles.Controls.Clear();

                bool isFirst = true;

                //Create new articles
                foreach (ArticleItem article in _articles)
                {
                    Article newArticleControl = new Article();
                    Articles.Controls.Add(newArticleControl);
                    newArticleControl.BindContent(article);
                    newArticleControl.MinimumSize = new System.Drawing.Size(Articles.ClientSize.Width - 10, 0);

                    if (isFirst)
                    {
                        newArticleControl.Expand();

                        isFirst = false;
                    }
                }

                //Layout the page
                Articles.ResumeLayout(true);
                PerformLayout();

                LoadingArticles.Visible = false;
            }
        }

        /// <summary>
        /// Polls the server for articles
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ArticleFetch_Tick(object sender, EventArgs e)
        {
            //Make sure long running fetches don't queue up
            if (!_fetchingArticles)
            {
                _fetchingArticles = true;

                //Poll in the background
                ThreadPool.QueueUserWorkItem(delegate
                {
                    try
                    {
                        // Get the children of the article repo node...
                        // i.e. all articles
                        _articles = _contentNavigator.GetChildItems<ArticleItem>(ARTICLE_REPO_PATH);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(string.Format("Exception getting articles: {0}", ex.Message));
                    }
                    finally
                    {
                        _fetchingArticles = false;
                    }
                });
            }
        }
    }
}
