#ifndef SUBJECT_H
#define SUBJECT_H

#include "Observer.h"

#include <list>
 
class Subject {
public:
	virtual ~Subject() {}

	virtual void subscribe(Observer *obs);
	virtual void unsubscribe(Observer *obs);
	virtual void notify();

protected:
	Subject() {}

private:
	std::list<Observer *> observers_;
};

#endif
