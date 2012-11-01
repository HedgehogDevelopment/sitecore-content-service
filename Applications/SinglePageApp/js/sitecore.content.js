/*!
 * Sitecore Content as a Service JavaScript Client
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

/********************************************************************
Description:
    Used to fetch Sitecore content from a Sitecore instance

Usage:
    var client = new Sitecore.Client();
    client.serviceHost = '/';
    client.servicePrefix = 'api';

    client.getItem('/sitecore/content/home', function (data, r, e) {
     console.log('Home Item: ' + data.Path);
    });

    client.getChildren('/sitecore/content/home', function (data, r, e) {
        console.log('children of home = ' + data.length);
        $.each(data, function (i, item) {
            console.log(item.Path);
        });
    });

    // get the descendants 
    client.getDescendants('/sitecore/content/home', function (data, r, e) {
        console.log('press releases = ' + data.length);
        $.each(data, function (i, item) {
            console.log(item.Path);
        });
    });

    client.getDescendants('/sitecore/content/home', function (data, r, e) {
        console.log('channels = ' + data.length);
        $.each(data, function (i, item) {
            console.log(item.Path);
        });
    });

    client.getAncestors('/sitecore/content/home', function (data, r, e) {
        console.log('Home\'s ancestors = ' + data.length);
        $.each(data, function (i, item) {
            console.log(item.Path);
        });
    });

*********************************************************************/

var Sitecore = window.Sitecore;

if (Sitecore === undefined) {
    Sitecore = {};
}

if (Sitecore.Client === undefined) {

    // create shortcut for jQuery
    if (window.$j === undefined) {
        $j = jQuery;
    }

    Sitecore.Client = function () {
        /// <summary>The Client provides a convenient wrapper for the ContentService 'REST' API</summary>
        /// <field name="asyncAjax" type="bool" mayBeNull="true">
        ///     Make an async AJAX call?
        /// </field>
        /// <field name="serviceHost" type="string" mayBeNull="false">
        ///     The protocol and host of the server.
        ///     '/' is acceptable for relative server calls
        /// </field>
        /// <field name="servicePrefix" type="string" mayBeNull="false">
        ///     The path to the content service. no leading or trailing slash.
        /// </field>
        /// <field name="apiVersion" type="string" mayBeNull="true">
        ///     The API Version
        /// </field>

        this.asyncAjax = true;
        this.serviceHost = '/';
        this.servicePrefix = 'api';
        this.apiVersion = "1.0";
    }

    Sitecore.Client.prototype.getItem = function (key, successCallback) {
        /// <summary>Gets the item from Sitecore</summary>
        /// <param name="key" optional="false" type="string">
        ///     The key of the item to get
        /// </param>
        /// <param name="successCallback" optional="false" type="delegate">
        ///     The method to call on successful fetching of the item
        /// </param>
        this.ajax('item', key, successCallback, null);
    }

    Sitecore.Client.prototype.getParent = function (key, successCallback) {
        /// <summary>Gets the parent of the item from Sitecore</summary>
        /// <param name="key" optional="false" type="string">
        ///     The key of the item to get the parent for
        /// </param>
        /// <param name="successCallback" optional="false" type="delegate">
        ///     The method to call on successful fetching of the item
        /// </param>
        this.ajax('parent', key, successCallback, null);
    }

    Sitecore.Client.prototype.getChildren = function (key, successCallback) {
        /// <summary>Gets the children of the specified item from Sitecore</summary>
        /// <param name="key" optional="false" type="string">
        ///     The key of the item to get children for
        /// </param>
        /// <param name="successCallback" optional="false" type="delegate">
        ///     The method to call on successful fetching of the item
        /// </param>
        this.ajax('children', key, successCallback, null);
    }

    Sitecore.Client.prototype.getDescendants = function (key, successCallback) {
        /// <summary>Gets the descendants of the specified item from Sitecore</summary>
        /// <param name="key" optional="false" type="string">
        ///     The key of the item to get descendants for
        /// </param>
        /// <param name="successCallback" optional="false" type="delegate">
        ///     The method to call on successful fetching of the item
        /// </param>
        this.ajax('descendants', key, successCallback, null);
    }

    Sitecore.Client.prototype.getAncestors = function (key, successCallback) {
        /// <summary>Gets the ancestors of the specified item from Sitecore</summary>
        /// <param name="key" optional="false" type="string">
        ///     The key of the item to get ancestors for
        /// </param>
        /// <param name="successCallback" optional="false" type="delegate">
        ///     The method to call on successful fetching of the item
        /// </param>
        this.ajax('ancestors', key, successCallback, null);
    }

    Sitecore.Client.prototype.getReferrers = function (key, successCallback) {
        /// <summary>Gets the referrers of the specified item from Sitecore</summary>
        /// <param name="key" optional="false" type="string">
        ///     The key of the item to get referrers for
        /// </param>
        /// <param name="successCallback" optional="false" type="delegate">
        ///     The method to call on successful fetching of the item
        /// </param>
        this.ajax('referrers', key, successCallback, null);
    }

    Sitecore.Client.prototype.getItems = function (key, successCallback) {
        /// <summary>Gets the specified items from Sitecore</summary>
        /// <param name="key" optional="false" type="string">
        ///     The comma separated list of keys 
        /// </param>
        /// <param name="successCallback" optional="false" type="delegate">
        ///     The method to call on successful fetching of the item
        /// </param>
        this.ajax('items', key, successCallback, null);
    }

    Sitecore.Client.prototype.ajax = function (method, key, callback, error) {
        /// <summary> Low level utility function to call the Salesforce endpoint.</summary>
        /// <param name="method" optional="false" type="string">
        ///     The service method to call
        /// </param>
        /// <param name="key" optional="true" type="string">
        ///     The key of the item to fetch
        /// </param>
        /// <param name="callback" optional="false" type="delegate">
        ///     The method to call on successful fetching 
        /// </param>
        /// <param name="error" optional="false" type="delegate">
        ///     The method to call on an error
        /// </param>
        var that = this;

        if (key[0] != '/') {
            key = '/' + key;
        }

        // setup a default logger to the console
        if (error == null) {
            error = function (obj, status, ex) {
                var msg = 'Error. "' + ex + '" making request to "' + url + '"';

                // if dev tools isn't open in IE, then there is no console object.
                // create a console obj if needed. If we want to alert the error 
                // if console isn't defined in IE, change next line to true.
                var alertFallback = false;
                if (typeof console === "undefined" || typeof console.log === "undefined") {
                    console = {};
                    if (alertFallback) {
                        console.log = function (msg) {
                            alert(msg);
                        };
                    } else {
                        console.log = function () { };
                    }
                }
                console.log(msg);
            };
        }

        var url = this.serviceHost + this.servicePrefix + '/' + method + key;

        $j.ajax({
            type: "GET",
            dataType: "json",
            async: this.asyncAjax,
            url: url,
            cache: false,
            success: callback,
            error: error           
        });
    }
}