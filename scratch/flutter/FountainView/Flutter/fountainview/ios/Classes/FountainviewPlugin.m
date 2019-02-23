#import "FountainviewPlugin.h"
#import "FountainSharpWrapperIOS/FountainSharpWrapperIOS.h"

@implementation FountainviewPlugin
+ (void)registerWithRegistrar:(NSObject<FlutterPluginRegistrar>*)registrar {

  FlutterMethodChannel* channel = [FlutterMethodChannel
      methodChannelWithName:@"fountainview"
            binaryMessenger:[registrar messenger]];

  FountainviewPlugin* instance = [[FountainviewPlugin alloc] init];

  [registrar addMethodCallDelegate:instance channel:channel];
}

- (void)handleMethodCall:(FlutterMethodCall*)call result:(FlutterResult)result {
  if ([@"ConvertToHtml" isEqualToString:call.method]) {
    NSString *fountainText = call.arguments[@"fountainText"];
    NSString *html= [FountainSharpWrapperIOS_FountainSharpWrapper convertToHtmlFountainText: fountainText];
    result(html);
  } else {
    result(FlutterMethodNotImplemented);
  }
}

@end
