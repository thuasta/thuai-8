#include "format.hpp"

#include <magic_enum.hpp>
#include <string>

#include "agent.hpp"
#include "player_info.hpp"

namespace thuai8_agent {

auto format_as(const Agent& object) -> std::string {
  return fmt::format("Agent[Token: {}]", object.token());
}

auto format_as(SkillKind object) -> std::string {
  return std::string(magic_enum::enum_name(object));
}

auto format_as(BuffKind object) -> std::string {
  return std::string(magic_enum::enum_name(object));
}

}  // namespace thuai8_agent