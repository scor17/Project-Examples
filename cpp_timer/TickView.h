#ifndef TICKVIEW_H
#define TICKVIEW_H

#include "Observer.h"
#include "Subject.h"
#include "Timer.h"

class TickView: public TimerView {
public:
	TickView(Timer *timer): TimerView(timer) {}

	~TickView() {timer_->unsubscribe(this);}

	virtual void display(std::ostream& os) const {
		os << '*' << std::endl;
	}
};

#endif
