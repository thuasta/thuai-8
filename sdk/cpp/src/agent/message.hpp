#pragma once
#ifndef _THUAI8_AGENT_MESSAGE_HPP_
#define _THUAI8_AGENT_MESSAGE_HPP_

#ifndef NDEBUG
#include <glaze/exceptions/core_exceptions.hpp>
#endif
#include <glaze/core/read.hpp>
#include <glaze/json/write.hpp>
#include <string>
#include <string_view>
#include <utility>

#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "game_statistics.hpp"
#include "player_info.hpp"

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
                "MISSILE", Missile, "KAMUI", Kamui);
};

template <>
struct meta<thuai8_agent::BuffKind> {
  using enum thuai8_agent::BuffKind;
  static constexpr auto value = enumerate(
      "BLACK_OUT", BlackOut, "SPEED_UP", SpeedUp, "FLASH", Flash, "DESTROY",
      Destroy, "CONSTRUCT", Construct, "TRAP", Trap, "MISSILE", Missile,
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

  template <class T>
  static void Read(T& value, std::string_view message) {
#ifdef NDEBUG
    std::ignore = glz::read<readopts>(value, message);
#else
    glz::ex::read<readopts>(value, message);
#endif
  }

  template <class T>
  [[nodiscard]] static auto Read(std::string_view message) -> T {
    T value{};
    Read(value, message);
    return value;
  }

  template <class T>
  [[nodiscard]] static auto Write(T&& value) -> std::string {
    return glz::write<writeopts>(std::forward<T>(value)).value();
  }

  // NOLINTBEGIN
  template <class T, class... Args>
  [[nodiscard]] static auto Write(Args&&... args) -> std::string {
    return glz::write<writeopts>(T{std::forward<Args>(args)...}).value();
  }
  // NOLINTEND

  [[nodiscard]] static auto ReadMessageType(std::string_view message)
      -> std::string {
    return Read<MessageType>(message).messageType;
  }

  [[nodiscard]] static auto ReadToken(std::string_view message) -> std::string {
    return Read<Token>(message).token;
  }

  [[nodiscard]] static auto ReadError(std::string_view message)
      -> std::pair<int, std::string> {
    Error value{Read<Error>(message)};
    return {value.errorCode, value.message};
  }

  [[nodiscard]] static auto GetPlayerInfo(std::string_view token,
                                          std::string_view request)
      -> std::string {
    return Write<addRequest>("GET_PLAYER_INFO", token, request);
  }

  [[nodiscard]] static auto GetEnvironmentInfo(std::string_view token)
      -> std::string {
    return Write<basic>("GET_ENVIRONMENT_INFO", token);
  }

  [[nodiscard]] static auto GetGameStatistics(std::string_view token)
      -> std::string {
    return Write<basic>("GET_GAME_STATISTICS", token);
  }

  [[nodiscard]] static auto GetAvailableBuffs(std::string_view token)
      -> std::string {
    return Write<basic>("GET_AVAILABLE_BUFFS", token);
  }

  [[nodiscard]] static auto PerformMove(std::string_view token,
                                        std::string_view direction)
      -> std::string {
    return Write<addDirection>("PERFORM_MOVE", token, direction);
  }

  [[nodiscard]] static auto PerformTurn(std::string_view token,
                                        std::string_view direction)
      -> std::string {
    return Write<addDirection>("PERFORM_TURN", token, direction);
  }

  [[nodiscard]] static auto PerformAttack(std::string_view token)
      -> std::string {
    return Write<basic>("PERFORM_ATTACK", token);
  }

  [[nodiscard]] static auto PerformSkill(std::string_view token,
                                         SkillKind skill) -> std::string {
    return Write<addSkill>("PERFORM_SKILL", token, skill);
  }

  [[nodiscard]] static auto PerformSelect(std::string_view token, BuffKind buff)
      -> std::string {
    return Write<addBuff>("PERFORM_SELECT", token, buff);
  }

 private:
  // NOLINTBEGIN(readability-implicit-bool-conversion)
  static constexpr glz::opts readopts{
      .error_on_unknown_keys = false, .raw_string = true, .partial_read = true};
  static constexpr glz::opts writeopts{.raw_string = true};
  // NOLINTEND(readability-implicit-bool-conversion)

  struct MessageType {
    std::string messageType;
  };

  struct Token {
    std::string token;
  };

  struct Error {
    int errorCode{};
    std::string message;
  };

  struct basic {
    std::string_view messageType;
    std::string_view token;
  };

  struct addRequest {
    std::string_view messageType;
    std::string_view token;
    std::string_view request;
  };

  struct addDirection {
    std::string_view messageType;
    std::string_view token;
    std::string_view direction;
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