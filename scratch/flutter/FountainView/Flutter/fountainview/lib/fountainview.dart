import 'dart:async';

import 'package:flutter/cupertino.dart';
import 'package:flutter/services.dart';
import 'package:flutter_html/flutter_html.dart';

class FountainView extends StatefulWidget {
  const FountainView(this.fountainText);
  final String fountainText;
   @override
  _FountainViewState createState() => _FountainViewState(fountainText);
}

class _FountainViewState extends State<FountainView>
{
 _FountainViewState(this._fountainText)
 {
   convertFountainToHtml(_fountainText).then((val) => setState(() {
          _htmlText = val;
        }));
  }
 String _htmlText;
 String _fountainText;
 @override
  Widget build(BuildContext context) {
    return new FutureBuilder(
      future: convertFountainToHtml(_fountainText),
      initialData: "Loading text..",
      builder: (BuildContext context, AsyncSnapshot<String> text) {
        return new SingleChildScrollView(child: new SafeArea(
          child: new Html(
            data: _htmlText,
            padding: EdgeInsets.all(8.0),)));
      });
  }

  static const MethodChannel _channel = const MethodChannel('fountainview');

  static Future<String> convertFountainToHtml(String fountainText) async {
     return await _channel.invokeMethod('ConvertToHtml', "This is a test");
  }
}
