#include "Timer.h"
#include "TimerView.h"
#include "KeyboardController.h"
#include "TickView.h"
#include "SecView.h"
#include "MinSecView.h"

#include <string>
#include <iostream>

using namespace std;
int main() {
	Timer timer(0, false);
	
	KeyboardController kc;
	kc.subscribe(&timer);
	
	TickView tv(&timer);
	SecView sv(&timer);
	MinSecView msv(&timer);
	
	kc.start();
}
