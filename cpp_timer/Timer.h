#ifndef TIMER_H
#define TIMER_H

#include "Observer.h"
#include "Subject.h"
#include "KeyboardController.h"

#include <iostream>
#include <string>
#include <atomic>
#include <thread>

class Timer : public Subject, public Observer {
public:
	Timer(long sec = 0, bool ticking = false) :sec_(sec), ticking_(ticking) {
		std::thread t(&Timer::run, this);
		t.detach();
	}
	void start();
	void stop();
	void reset();
	unsigned long get() const;

	virtual void update(Subject* s);

	Timer(const Timer&) = delete;
	Timer& operator = (const Timer&) = delete;

private:
	std::atomic<long> sec_;
	std::atomic<bool> ticking_;
	void run();
};

#endif
