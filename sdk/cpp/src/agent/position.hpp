#pragma once
#ifndef _THUAI8_AGENT_POSITION_HPP_
#define _THUAI8_AGENT_POSITION_HPP_

#include <spdlog/fmt/bundled/format.h>

#include <cstdlib>
#include <type_traits>

constexpr double epsilon{1e-6};

namespace thuai8_agent {

template <class T>
  requires std::is_arithmetic_v<T>
struct Position {
  T x{};
  T y{};
  double angle{};

  template <class U>
    requires std::is_arithmetic_v<U>
  constexpr auto operator==(const Position<U>& rhs) -> bool {
    if constexpr (std::is_floating_point_v<T> && std::is_floating_point_v<U>) {
      return std::abs(x - rhs.x) < epsilon && std::abs(y - rhs.y) < epsilon;
    } else {
      return x == rhs.x && y == rhs.y;
    }
  }
};

}  // namespace thuai8_agent

template <class T>
  requires std::is_arithmetic_v<T>
struct fmt::formatter<thuai8_agent::Position<T>> : fmt::formatter<std::string> {
  static auto format(const thuai8_agent::Position<T>& obj,
                     format_context& ctx) {
    return fmt::format_to(ctx.out(), "Position: {{x: {}, y: {}, angle: {}}}",
                          obj.x, obj.y, obj.angle);
  }
};

#endif  // _THUAI8_AGENT_POSITION_HPP_