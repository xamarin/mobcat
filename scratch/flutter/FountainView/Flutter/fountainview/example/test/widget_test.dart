// This is a basic Flutter widget test.
//
// To perform an interaction with a widget in your test, use the WidgetTester
// utility that Flutter provides. For example, you can send tap and scroll
// gestures. You can also use WidgetTester to find child widgets in the widget
// tree, read text, and verify that the values of widget properties are correct.

import 'package:flutter/material.dart';
import 'package:flutter_test/flutter_test.dart';
import 'package:fountainview/fountainview.dart';

import 'package:fountainview_example/main.dart';

void main() {
  testWidgets('Verify FountainView', (WidgetTester tester) async {
    // Build our app and trigger a frame.
    await tester.pumpWidget(MyApp());
    debugPrint("Ben testing");
    tester.allWidgets.forEach((f) => debugPrint(f.toString()));

    // Verify that platform version is retrieved.
    expect(
      find.byElementType(FountainView),
      findsOneWidget,
    );
  });
}
