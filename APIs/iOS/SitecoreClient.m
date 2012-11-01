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
//  SitecoreClient.m
//

#import "SitecoreClient.h"

@interface SitecoreClient()

- (ContentItem *) buildContentItem:(NSDictionary *)dictionary;
- (NSArray *) buildContentItemArray:(NSArray *)jsonArray;
- (NSDictionary *) getJsonResultDictionary :(NSString *)action: (NSString *) key;
- (NSArray *) getJsonResultArray :(NSString *)action : (NSString *) key;
+ (NSDictionary *) getJsonResultDictionary:(NSString *) url;
+ (NSArray *) getJsonResultArray:(NSString *) url;

@end

@implementation SitecoreClient

///
/// creates a new instance of the SitecoreClient class.
///
- (SitecoreClient *) initWithSettings:(NSString *) host
                                     :(NSString *) prefix
{
    self = [super init];
    if(self)
    {
        self.serviceHost = [host copy];
        self.servicePrefix = [prefix copy];
        self.apiVersion = @"1.0";
    }
    
    return self;
}

///
/// Gets an item by key.
///
- (ContentItem *) getItem:(NSString *) key
{
    return [self buildContentItem:[self getJsonResultDictionary:@"item" :key]];
}

///
/// Gets the parent item of an item.
///
- (ContentItem *) getParent:(NSString *) key
{
    return [self buildContentItem:[self getJsonResultDictionary:@"parent" :key]];
}

///
/// Gets a ContentItems array of the children items of an item.
///
- (NSArray *) getChildren:(NSString *) key
{
    return [self buildContentItemArray:[self getJsonResultArray:@"children" :key]];
}

///
/// Gets a ContentItems array of the descendant items of an item.
///
- (NSArray *) getDescendants:(NSString *) key
{
    return [self buildContentItemArray:[self getJsonResultArray:@"descendants" :key]];
}

///
/// Gets a ContentItems array of the ancestor items of an item.
///
- (NSArray *) getAncestors:(NSString *) key
{
    return [self buildContentItemArray:[self getJsonResultArray:@"ancestors" :key]];
}

///
/// Gets a ContentItems array of referrerer items for an item
///
- (NSArray *) getReferrers:(NSString *) key
{
    return [self buildContentItemArray:[self getJsonResultArray:@"referrers" :key]];
}

///
/// Gets a ContentItems array of ContentItems for the given comma seperated key string.
///
- (NSArray *) getItems:(NSString *) key
{
    return [self buildContentItemArray:[self getJsonResultArray:@"items" :key]];
}

//
// private method. Returns a ContentItem object from a dictionary.
//
- (ContentItem *) buildContentItem:(NSDictionary *) dictionary
{
    ContentItem *item = [[ContentItem alloc] init];
    
    for (NSString *key in dictionary)
    {
        id dicItem = [dictionary objectForKey:key];
        
        if ([key isEqualToString:@"Key"]){
            item.key = (NSString *)dicItem;
        }
        else if ([key isEqualToString:@"Name"]){
            item.name = (NSString *)dicItem;
        }
        else if ([key isEqualToString:@"Path"]){
            item.path = (NSString *) dicItem;
        }
        else if ([key isEqualToString:@"$type"]){
            item.type = (NSString *) dicItem;
        }
        else {
            [item.fields setObject:dicItem forKey:key];
        }
    }
    
    return item;
}

//
// private method. Builds an array of content items from a json array
//
- (NSArray *) buildContentItemArray:(NSArray *) jsonArray
{
    NSMutableArray *results = [[NSMutableArray alloc]init];
    
    for (int i = 0; i < jsonArray.count; i++)
    {
        [results addObject:[self buildContentItem:[jsonArray objectAtIndex:i]]];
    }
    
    return results;
}

//
// private method. gets json results as a dictionary
//
- (NSDictionary *) getJsonResultDictionary :(NSString *) action
                                           :(NSString *) key
{
    NSString *url = [NSString stringWithFormat:@"%@%@/%@%@", self.serviceHost, self.servicePrefix, action, key];
    return [SitecoreClient getJsonResultDictionary:url];
}

//
// private method. gets json results as an array
//
- (NSArray *) getJsonResultArray :(NSString *) action
                                 :(NSString *) key
{
    NSString *url = [NSString stringWithFormat:@"%@%@/%@%@", self.serviceHost, self.servicePrefix, action, key];
    return [SitecoreClient getJsonResultArray:url];
}

///
/// private static method. gets json results as a dictionary of objects
///
+ (NSDictionary *) getJsonResultDictionary:(NSString *) url
{
    // create requestUrl object
    NSURL *requestURL = [NSURL URLWithString:url];
    
    // create response object that we will use to read from later
    NSData *responseData = [NSData dataWithContentsOfURL:requestURL];
    
    // declare error object for error handling
    NSError *error = nil;
    
    // send request and recieve expected JSON response
    NSDictionary *json = [NSJSONSerialization JSONObjectWithData:responseData
                                                         options:0
                                                           error:&error];
    
    return json;
}

///
/// private static method. gets json results as an array of objects
///
+ (NSArray *) getJsonResultArray:(NSString *) url
{
    // create requestUrl object
    NSURL *requestURL = [NSURL URLWithString:url];
    
    // create response object that we will use to read from later
    NSData *responseData = [NSData dataWithContentsOfURL:requestURL];
    
    // declare error object for error handling
    NSError *error = nil;
    
    // send request and recieve expected JSON response
    NSArray *json = [NSJSONSerialization JSONObjectWithData:responseData
                                                    options:0
                                                      error:&error];
    
    return json;
}

@end