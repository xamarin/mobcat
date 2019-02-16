# fountainview example

    import 'package:flutter/material.dart';
    import 'package:fountainview/fountainview.dart';

    void main()
    {
    runApp(MaterialApp(
        title: "FountainView Example",
        home: MyApp()
    ));
    }

    class MyApp extends StatelessWidget {
    @override
    Widget build(BuildContext context) {
        return Scaffold(
            appBar: AppBar(
            title: const Text('FountainView example app'),
            ),
            body: new FountainView("Title: Big Fish\nCredit: written by\nAuthor: John August\nSource: based on the novel by Daniel Wallace"),
        );
    }
    }
