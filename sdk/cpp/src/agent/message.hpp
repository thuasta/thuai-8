#pragma once
#ifndef _THUAI8_AGENT_MESSAGE_HPP_
#define _THUAI8_AGENT_MESSAGE_HPP_

#include <glaze/exceptions/core_exceptions.hpp>
#include <glaze/json/jmespath.hpp>
#include <glaze/json/write.hpp>
#include <string>
#include <string_view>
#include <utility>

#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "player.hpp"

namespace glz {

template <>
struct meta<thuai8_agent::ArmorKnifeState> {
  using enum thuai8_agent::ArmorKnifeState;
  static constexpr auto value =
      enumerate("NOT_OWNED", NotOwned, "AVAILABLE", Available, "ACTIVE", Active,
                "BROKEN", Broken);
};

template <>
struct meta<thuai8_agent::SkillKind> {
  using enum thuai8_agent::SkillKind;
  static constexpr auto value =
      enumerate("BLACK_OUT", BlackOut, "SPEED_UP", SpeedUp, "FLASH", Flash,
                "DESTROY", Destroy, "CONSTRUCT", Construct, "TRAP", Trap,
                "RECOVER", Recover, "KAMUI", Kamui);
};

template <>
struct meta<thuai8_agent::BuffKind> {
  using enum thuai8_agent::BuffKind;
  static constexpr auto value = enumerate(
      "BLACK_OUT", BlackOut, "SPEED_UP", SpeedUp, "FLASH", Flash, "DESTROY",
      Destroy, "CONSTRUCT", Construct, "TRAP", Trap, "RECOVER", Recover,
      "KAMUI", Kamui, "BULLET_COUNT", BulletCount, "BULLET_SPEED", BulletSpeed,
      "ATTACK_SPEED", AttackSpeed, "LASER", Laser, "DAMAGE", Damage,
      "ANTI_ARMOR", AntiArmor, "ARMOR", Armor, "REFLECT", Reflect, "DODGE",
      Dodge, "KNIFE", Knife, "GRAVITY", Gravity);
};

template <>
struct meta<thuai8_agent::Stage> {
  using enum thuai8_agent::Stage;
  static constexpr auto value =
      enumerate("REST", Rest, "BATTLE", Battle, "END", End);
};

template <>
struct meta<thuai8_agent::Wall> {
  static constexpr auto value{&thuai8_agent::Wall::position};
};

}  // namespace glz

namespace thuai8_agent {

class Message {
 public:
  Message() = delete;
  Message(const Message&) = delete;
  Message(Message&&) = delete;
  auto operator=(const Message&) -> Message& = delete;
  auto operator=(Message&&) -> Message& = delete;
  ~Message() = delete;

  template <glz::string_literal Path = "", bool Partial = false, class T>
  static void Read(T& value, std::string_view message) {
    if constexpr (Path.length == 0) {
      glz::ex::read<GetOpts(Partial)>(value, message);
    } else {
      if (glz::read_jmespath<Path, GetOpts(Partial)>(value, message))
          [[unlikely]] {
        throw std::runtime_error(std::string{message});
      }
    }
  }

  template <class T, bool Partial = false, glz::string_literal Path = "">
  [[nodiscard]] static auto Read(std::string_view message) -> T {
    T value{};
    Read<Path, Partial>(value, message);
    return value;
  }

  template <class T>
  [[nodiscard]] static auto Write(T&& value) -> std::string {
    return glz::write<writeopts>(std::forward<T>(value)).value();
  }

  [[nodiscard]] static auto ReadMessageType(std::string_view message)
      -> std::string_view {
    return Read<MessageType, true>(message).messageType;
  }

  [[nodiscard]] static auto ReadError(std::string_view message)
      -> std::pair<int, std::string_view> {
    Error value{Read<Error, true>(message)};
    return {value.errorCode, value.message};
  }

  [[nodiscard]] static auto GetPlayerInfo(std::string_view token)
      -> std::string {
    return Write(basic{.messageType = "GET_PLAYER_INFO", .token = token});
  }

  [[nodiscard]] static auto GetEnvironmentInfo(std::string_view token)
      -> std::string {
    return Write(basic{.messageType = "GET_ENVIRONMENT_INFO", .token = token});
  }

  [[nodiscard]] static auto GetGameStatistics(std::string_view token)
      -> std::string {
    return Write(basic{.messageType = "GET_GAME_STATISTICS", .token = token});
  }

  [[nodiscard]] static auto GetAvailableBuffs(std::string_view token)
      -> std::string {
    return Write(basic{.messageType = "GET_AVAILABLE_BUFFS", .token = token});
  }

  [[nodiscard]] static auto PerformMove(std::string_view token,
                                        std::string_view direction,
                                        float distance) -> std::string {
    return Write(addDistance{.messageType = "PERFORM_MOVE",
                             .token = token,
                             .direction = direction,
                             .distance = distance});
  }

  [[nodiscard]] static auto PerformTurn(std::string_view token,
                                        std::string_view direction, int angle)
      -> std::string {
    return Write(addAngle{.messageType = "PERFORM_TURN",
                          .token = token,
                          .direction = direction,
                          .angle = angle});
  }

  [[nodiscard]] static auto PerformAttack(std::string_view token)
      -> std::string {
    return Write(basic{.messageType = "PERFORM_ATTACK", .token = token});
  }

  [[nodiscard]] static auto PerformSkill(std::string_view token,
                                         SkillKind skill) -> std::string {
    return Write(addSkill{
        .messageType = "PERFORM_SKILL", .token = token, .skillName = skill});
  }

  [[nodiscard]] static auto PerformSelect(std::string_view token, BuffKind buff)
      -> std::string {
    return Write(addBuff{
        .messageType = "PERFORM_SELECT", .token = token, .buffName = buff});
  }

 private:
  static constexpr glz::opts readopts{
      .error_on_unknown_keys = false, .minified = true, .raw_string = true};
  static constexpr glz::opts partial_readopts{.error_on_unknown_keys = false,
                                              .minified = true,
                                              .raw_string = true,
                                              .partial_read = true};
  static constexpr glz::opts writeopts{.raw_string = true};

  static consteval auto GetOpts(bool is_partial) -> glz::opts {
    return is_partial ? partial_readopts : readopts;
  }

  struct MessageType {
    std::string_view messageType;
  };

  struct Error {
    int errorCode{};
    std::string_view message;
  };

  struct basic {
    std::string_view messageType;
    std::string_view token;
  };

  struct addDistance {
    std::string_view messageType;
    std::string_view token;
    std::string_view direction;
    float distance;
  };

  struct addAngle {
    std::string_view messageType;
    std::string_view token;
    std::string_view direction;
    int angle;
  };

  struct addSkill {
    std::string_view messageType;
    std::string_view token;
    SkillKind skillName;
  };

  struct addBuff {
    std::string_view messageType;
    std::string_view token;
    BuffKind buffName;
  };
};

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_MESSAGE_HPP_