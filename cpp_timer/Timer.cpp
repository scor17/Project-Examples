#include "Timer.h"
#include "Observer.h"
#include "Subject.h"
//#include "KeyboardController.h"

#include <thread>
#include <string>
#include <iostream>
#include <atomic>
#include <chrono>

void Timer::start() {
	ticking_ = true;
}

void Timer::stop() {
	ticking_= false;
}

void Timer::reset() {
	sec_ = 0;
}

unsigned long Timer::get() const {
	return sec_;
}

void Timer::update(Subject* s) {
	KeyboardController* k = (KeyboardController*)s;
	if(k->getCommand() == "start")
		start();
	else if(k->getCommand() == "stop")
		stop();
	else if(k->getCommand() == "reset")
		reset();
}

void Timer::run() {
	while(true) {
		std::this_thread::sleep_for(std::chrono::seconds(1));
		if(ticking_) {
			notify();
			sec_++;
		}
	}
}
