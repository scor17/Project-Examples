#include "KeyboardController.h"

#include <iostream>
#include <string>

void KeyboardController::start() {
	while(std::cin >> cmd_) {
		notify();
	}
}

std::string KeyboardController::getCommand() const {
	return cmd_;
}
