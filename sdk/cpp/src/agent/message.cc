#include "message.hpp"

#include <string>
#include <string_view>
#include <utility>

struct MessageType {
  std::string messageType;
};

struct Token {
  std::string token;
};

struct Error {
  int errorCode;
  std::string message;
};

namespace thuai8_agent {

auto Message::ReadMessageType(std::string_view message) -> std::string {
  return ReadInfo<MessageType>(message).messageType;
}

auto Message::ReadToken(std::string_view message) -> std::string {
  return ReadInfo<Token>(message).token;
}

auto Message::ReadError(std::string_view message)
    -> std::pair<int, std::string> {
  Error value{ReadInfo<Error>(message)};
  return {value.errorCode, value.message};
}

}  // namespace thuai8_agent