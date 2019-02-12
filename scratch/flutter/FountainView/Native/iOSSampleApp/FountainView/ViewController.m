//
//  ViewController.m
//  FountainView
//
//  Created by Ben Buttigieg on 28/01/2019.
//  Copyright © 2019 Microsoft. All rights reserved.
//

#import "ViewController.h"
#include "FountainSharpWrapperIOS/FountainSharpWrapperIOS.h"

@interface ViewController ()

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    NSString *data = @"Title: Big Fish\nCredit: written by\nAuthor: John August\nSource: based on the novel by Daniel Wallace\nNotes:\t\n\tFINAL PRODUCTION DRAFT\n\tincludes post-production dialogue \n\tand omitted scenes\nCopyright: (c) 2003 Columbia Pictures\n\nThis is a Southern story, full of lies and fabrications, but truer for their inclusion.\n\n====\n\n**FADE IN:**\n\nA RIVER.\n\nWe're underwater, watching a fat catfish swim along.  \n\nThis is The Beast.\n\nEDWARD (V.O.)\nThere are some fish that cannot be caught.  It's not that they're faster or stronger than other fish.  They're just touched by something extra.  Call it luck.  Call it grace.  One such fish was The Beast.  \n\nThe Beast's journey takes it past a dangling fish hook, baited with worms.  Past a tempting lure, sparkling in the sun.  Past a swiping bear claw.  The Beast isn't worried.\n\nEDWARD (V.O.)(CONT'D)\nBy the time I was born, he was already a legend.  He'd taken more hundred-dollar lures than any fish in Alabama. Some said that fish was the ghost of Henry Walls, a thief who'd drowned in that river 60 years before.   Others claimed he was a lesser dinosaur, left over from the Cretaceous period.\n\nINT.  WILL'S BEDROOM - NIGHT (1973)\n\nWILL BLOOM, AGE 3, listens wide-eyed as his father EDWARD BLOOM, 40's and handsome, tells the story.  In every gesture, Edward is bigger than life, describing each detail with absolute conviction.\n\nEDWARD\nI didn't put any stock into such speculation or superstition.  All I knew was I'd been trying to catch that fish since I was a boy no bigger than you.  \n(closer)\nAnd on the day you were born, that was the day I finally caught him.\n\nEXT.  CAMPFIRE - NIGHT (1977)\n\nA few years later, and Will sits with the other INDIAN GUIDES as Edward continues telling the story to the tribe.  \n\nEDWARD\nNow, I'd tried everything on it:  worms, lures, peanut butter, peanut butter-and-cheese.  But on that day I had a revelation:  if that fish was the ghost of a thief, the usual bait wasn't going to work.  I would have to use something he truly desired. \n\nEdward points to his wedding band, glinting in the firelight.\n\nLITTLE BRAVE\n(confused)\nYour finger?\n\nEdward slips his ring off.\n\nEDWARD\nGold.";

    NSString *html= [FountainSharpWrapperIOS_FountainSharpWrapper convertToHtmlFountainText: data];
    
    NSLog(@"Greeting message: %@\n", html );
    
    [self.webView loadHTMLString:html baseURL:nil];
}


@end
