/*!
 * Sitecore Content as a Service - Single Page Application
 * https://github.com/HedgehogDevelopment/sitecore-content-service
 *
 * Copyright © 2012 Hedgehog Development, LLC (www.hhogdev.com)
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 */

/// <reference path="/js/sitecore.content.js" />
/// <reference path="/js/lib/jquery-1.7.2-vsdoc.js" />
/// <reference path="/js/lib/knockout-2.1.0.debug.js" />

/*****

Setup our sitecore client api

*****/
var sitecore = new Sitecore.Client();
sitecore.serviceHost = 'http://sitecore.local/';
sitecore.servicePrefix = 'api';


var initialMoment = moment(new Date());

var SPA_PATH                = '/sitecore/content/Scaas-Demo/Single-Page-App';
var ARTICLE_REPO_PATH       = '/sitecore/content/Scaas-Demo/Content/Articles';
var PROMO_REPO_PATH         = '/sitecore/content/Scaas-Demo/Content/Promotions';
var APPLICATION_ITEM_TYPE   = 'HedgehogDevelopment.Scaas.Models.ApplicationItem, HedgehogDevelopment.Scaas.Models';
var CONTENT_ITEM_TYPE       = 'HedgehogDevelopment.Scaas.Models.ContentItem, HedgehogDevelopment.Scaas.Models';


//ViewModel
function SampleApplicationViewModel() {
    //Data
    var self = this;
    self.Navigation = ko.observable();
    self.Promotion = ko.observable();
    self.Content = ko.observable();
    self.Articles = ko.observable();

    self.intervalID = -1;
    self.lastArticleCount = -1;

    //Behaviors
    self.reloadArticles = function () {
        // Get all articles from Sitecore
        sitecore.getChildren(ARTICLE_REPO_PATH, function (data) {
            //Only update if the article count has changed
            if (self.lastArticleCount != data.length) {
                self.lastArticleCount = data.length;
                self.Articles(data);
                hookupAccordion();
            }
        });
    };

    self.loadArticles = function () {
        self.Content(null);
        self.lastArticleCount = -1;
        self.reloadArticles();

        if (self.intervalID == -1) {
            self.intervalID = window.setInterval(function () { self.reloadArticles() }, 5000);
        }
    };

    self.loadContent = function (content) {
        if (self.intervalID != -1) {
            window.clearInterval(self.intervalID);
            self.intervalID = -1;
        }

        self.Articles(null);

        sitecore.getItem(content, function (data) {
            self.Content(data);
        });
    };

    self.reloadNavigation = function () {
        var navElements = new Array();

        //Get the home element
        sitecore.getItem(SPA_PATH, function (data) {
            //Get the subnav items
            sitecore.getChildren(SPA_PATH, function (data) {
                var navElement;

                for (navElement in data) {
                    navElements.push(data[navElement]);
                }

                //Update the navigation
                self.Navigation(navElements);
            });
        });
    };

    self.reloadPromotions = function () {
        sitecore.getChildren(PROMO_REPO_PATH, function (data) {
            self.Promotion(data);
        });
    };

    self.itemSelected = function (item) {
        if (item.$type == APPLICATION_ITEM_TYPE) {
            self.loadArticles();
        }
        else if (item.$type == CONTENT_ITEM_TYPE) {
            self.loadContent(item.Key);
        }

        self.reloadPromotions();
    };

    self.resourceUrl = function (pathToresource) {
        return sitecore.serviceHost + pathToresource;
    }
}

function hookupAccordion() {
    //Accordion
    $(".accordion .collapsible-title").click(function () {
        var accordionElement = $(this).parent(".accordion-element");

        if (accordionElement.hasClass("collapsed")) {
            expandAccordion(accordionElement)
        }
        else {
            collapseAccordion(accordionElement);
        }
    });

    $(".accordion .accordion-element").not(":first").each(function () { collapseAccordion($(this)) });

    // lightbox the article images 
    $('#articles a').lightBox({
        imageLoading: '/img/lightbox-ico-loading.gif',
        imageBtnClose: '/img/lightbox-btn-close.gif',
        imageBtnPrev: '/img/lightbox-btn-prev.gif',
        imageBtnNext: '/img/lightbox-btn-next.gif',
        imageBlank: '/img/imageBlank.gif'
    });
}

function expandAccordion(accordionElement) {
    var icon = accordionElement.find(".icon");

    accordionElement.removeClass("collapsed");
    icon.removeClass("icon-chevron-down");
    icon.addClass("icon-chevron-up");
}

function collapseAccordion(accordionElement) {
    var icon = accordionElement.find(".icon");

    accordionElement.addClass("collapsed");
    icon.removeClass("icon-chevron-up");
    icon.addClass("icon-chevron-down");
}

//Enable cross site scripting in jQuery. This is for testing.
jQuery.support.cors = true;

app = new SampleApplicationViewModel();
ko.applyBindings(app);
app.reloadNavigation();
app.loadArticles();
app.reloadPromotions();

//Show the app container
$("#application_window").show();

