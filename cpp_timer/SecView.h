#ifndef SECVIEW_H
#define SECVIEW_H

#include "Observer.h"
#include "Subject.h"
#include "Timer.h"

class SecView: public TimerView {
public:
	SecView(Timer* timer): TimerView(timer) {}

	~SecView() {timer_->unsubscribe(this);}

	virtual void display(std::ostream& os) const {
		os << timer_->get() << std::endl;
	}
};

#endif
