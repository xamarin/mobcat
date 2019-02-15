package com.benbtg.fountainview;

import io.flutter.plugin.common.MethodCall;
import io.flutter.plugin.common.MethodChannel;
import io.flutter.plugin.common.MethodChannel.MethodCallHandler;
import io.flutter.plugin.common.MethodChannel.Result;
import io.flutter.plugin.common.PluginRegistry.Registrar;

import fountainsharpwrapperdroid.fountainsharpwrapperdroid.FountainSharpWrapper;

/** FountainviewPlugin */
public class FountainviewPlugin implements MethodCallHandler {
  /** Plugin registration. */
  public static void registerWith(Registrar registrar) {
    final MethodChannel channel = new MethodChannel(registrar.messenger(), "fountainview");
    channel.setMethodCallHandler(new FountainviewPlugin());
  }

  @Override
  public void onMethodCall(MethodCall call, Result result) {
    if (call.method.equals("ConvertToHtml")) {
      String sampleText = call.argument("fountainText");
      String fountainText = FountainSharpWrapper.convertToHtml(sampleText);
      result.success(fountainText);
    } else {
      result.notImplemented();
    }
  }
}
