import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_html/flutter_html.dart';



void main() => runApp(MyApp());

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        // This is the theme of your application.
        //
        // Try running your application with "flutter run". You'll see the
        // application has a blue toolbar. Then, without quitting the app, try
        // changing the primarySwatch below to Colors.green and then invoke
        // "hot reload" (press "r" in the console where you ran "flutter run",
        // or simply save your changes to "hot reload" in a Flutter IDE).
        // Notice that the counter didn't reset back to zero; the application
        // is not restarted.
        primarySwatch: Colors.blue,
      ),
      home: MyHomePage(title: 'Flutter Demo Page'),
    );
  }
}

class MyMainPage extends StatelessWidget{
  @override
  Widget build(BuildContext context) {
    return Text("Hello world");
  }
//   const channel = BasicMessageChannel<String>('foo', StringCodec());

// // Send message to platform and receive reply.
// final String reply = await channel.send('Hello, world');
// print(reply);

}

class MyHomePage extends StatefulWidget {
  MyHomePage({Key key, this.title}) : super(key: key);

  final String title;

  @override
  _MyHomePageState createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  static const platform = const MethodChannel('fountain.mobcat/fountainlib');
  int _counter = 0;
  String _fountainText = "Init text";

  String _fountainDoc = 'Basic start';

  Future<void> _convertToHtml() async {
    
    try {
     _fountainDoc = await platform.invokeMethod('ConvertToHtml', "This is a text");
    } on PlatformException catch (e) {
      _fountainDoc = "Failed to convert fountain docl: '${e.message}'.";
    }

    setState(() {
      _fountainText = _fountainDoc;
    });
  }

  @override
  Widget build(BuildContext context) {
    // This method is rerun every time setState is called, for instance as done
    // by the _incrementCounter method above.
    //
    // The Flutter framework has been optimized to make rerunning build methods
    // fast, so that you can just rebuild anything that needs updating rather
    // than having to individually change instances of widgets.
    return Scaffold(
      appBar: AppBar(
        // Here we take the value from the MyHomePage object that was created by
        // the App.build method, and use it to set our appbar title.
        title: Text(widget.title),
      ),
      body: Center(
        // Center is a layout widget. It takes a single child and positions it
        // in the middle of the parent.
        child: 
            Html(data:'$_fountainText',
            ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _convertToHtml,
        tooltip: 'Increment',
        child: Icon(Icons.add),
      ), // This trailing comma makes auto-formatting nicer for build methods.
    );
  }
}
