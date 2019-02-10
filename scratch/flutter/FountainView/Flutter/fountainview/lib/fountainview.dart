import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_html/flutter_html.dart';

class FountainView extends StatelessWidget {
  const FountainView(this.fountainText);
  final String fountainText;

 @override
  Widget build(BuildContext context) {
    return new FutureBuilder(
      future: platformVersion(fountainText),
      initialData: "Loading text..",
      builder: (BuildContext context, AsyncSnapshot<String> text) {
        return new Html(data: text.data);
      });
  }

  static const MethodChannel _channel = const MethodChannel('fountainview');

  static const MethodChannel _newchannel = const MethodChannel('fountain.mobcat/fountainlib');

  static Future<String> get convertFountainToHtml async {
  
     final String fountainDoc = await _channel.invokeMethod('ConvertToHtml', "This is a test");

    return fountainDoc;
  }

  static Future<String> platformVersion(String text) async {
    final String version = await _channel.invokeMethod('getPlatformVersion');
    return version + text;
  }
}
