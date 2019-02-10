import 'dart:async';

import 'package:flutter/services.dart';

class Fountainview {
  static const MethodChannel _channel =
      const MethodChannel('fountainview');

  static Future<String> get platformVersion async {
    final String version = await _channel.invokeMethod('getPlatformVersion');
    return version;
  }
}
