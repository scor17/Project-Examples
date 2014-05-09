#ifndef TIMERVIEW_H
#define TIMERVIEW_H

#include "Observer.h"
#include "Subject.h"
#include "Timer.h"

class TimerView: public Observer {
public:
	TimerView(Timer* timer):timer_(timer) { timer->subscribe(this); }
		
	~TimerView() { timer_->unsubscribe(this); }	

	virtual void update(Subject *s) {
		if(s == timer_)
			display(std::cout);
	}

	virtual void display(std::ostream& os) const = 0;

protected:
	Timer *timer_;
};

#endif
