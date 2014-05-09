#include "Subject.h"

#include <list>

void Subject::subscribe(Observer *obs) {
	observers_.push_back(obs);
}

void Subject::unsubscribe(Observer *obs) {
	observers_.remove(obs);
}

void Subject::notify() {
	for(auto it : observers_) {
		it->update(this);
	}
}