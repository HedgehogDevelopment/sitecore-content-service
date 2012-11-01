/*!
 * Sitecore Content as a Service iOS API
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

//
//  SitecoreClient.h
//  Provides methods for loading sitecore items from a JSON service end point.
//
//  Sample usage:
//
//  NSString *serviceHost = @"http://mysitecorewebsite";
//  NSString *servicePrefix = @"/api";
//  SitecoreClient *scClient = [[SitecoreClient alloc] initWithSettings:serviceHost :servicePrefix];
//
//  ContentItem *homeItem = [scClient getItem:@"/sitecore/content/home/"];
//  NSString *itemName = homeItem.name;
//  NSString *title = [homeItem.fields objectForKey:@"title"];

#import <Foundation/Foundation.h>
#import "ContentItem.h"

@interface SitecoreClient : NSObject

@property NSString *serviceHost;
@property NSString *servicePrefix;
@property NSString *apiVersion;

///
/// creates a new instance of the SitecoreClient class.
///
- (SitecoreClient *) initWithSettings:(NSString *) host
                         :(NSString *) prefix;

///
/// Gets an item by key
///
- (ContentItem *) getItem:(NSString *) key;

///
/// Gets the parent item of an item.
///
- (ContentItem *) getParent:(NSString *) key;

///
/// Gets a ContentItems array of the children items of an item.
///
- (NSArray *) getChildren:(NSString *) key;

///
/// Gets a ContentItems array of the descendant items of an item.
///
- (NSArray *) getDescendants:(NSString *) key;

///
/// Gets a ContentItems array of the ancestor items of an item.
///
- (NSArray *) getAncestors:(NSString *) key;

///
/// Gets a ContentItems array of referrerer items for an item
///
- (NSArray *) getReferrers:(NSString *) key;

///
/// Gets a ContentItems array of ContentItems for the given comma seperated key string.
///
- (NSArray *) getItems:(NSString *) key;

@end
