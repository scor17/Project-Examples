#ifndef KEYBOARDCONTROLLER_H
#define KEYBOARDCONTROLLER_H

#include "Subject.h"
#include <iostream>
#include <string>

class KeyboardController: public Subject {
public:
	void start();
	std::string getCommand() const;
private:
	std::string cmd_;
};

#endif
