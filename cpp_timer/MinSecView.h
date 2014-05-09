#ifndef MINSECVIEW_H
#define MINSECVIEW_H

#include "Observer.h"
#include "Subject.h"
#include "Timer.h"

#include <iomanip>

class MinSecView: public TimerView {
public:
	MinSecView(Timer *timer): TimerView(timer) {}

	~MinSecView() {timer_->unsubscribe(this);}

	virtual void display(std::ostream& os) const {
		os << (timer_->get() / 60) << ':' << std::setfill('0') << std::setw(2) << (timer_->get() % 60) <<  std::endl;
	}
};

#endif
