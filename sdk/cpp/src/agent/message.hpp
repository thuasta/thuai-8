#pragma once
#ifndef _THUAI8_AGENT_MESSAGE_HPP_
#define _THUAI8_AGENT_MESSAGE_HPP_

#include <nlohmann/json.hpp>
#include <string>

#include "available_buffs.hpp"
#include "player_info.hpp"

namespace thuai8_agent {

NLOHMANN_JSON_SERIALIZE_ENUM(SkillKind, {{SkillKind::BlackOut, "BLACK_OUT"},
                                         {SkillKind::SpeedUp, "SPEED_UP"},
                                         {SkillKind::Flash, "FLASH"},
                                         {SkillKind::Destroy, "DESTROY"},
                                         {SkillKind::Construct, "CONSTRUCT"},
                                         {SkillKind::Trap, "TRAP"},
                                         {SkillKind::Missile, "MISSILE"},
                                         {SkillKind::Kamui, "KAMUI"}});

NLOHMANN_JSON_SERIALIZE_ENUM(BuffKind, {{BuffKind::BulletCount, "BULLET_COUNT"},
                                        {BuffKind::BulletSpeed, "BULLET_SPEED"},
                                        {BuffKind::AttackSpeed, "ATTACK_SPEED"},
                                        {BuffKind::Laser, "LASER"},
                                        {BuffKind::Damage, "DAMAGE"},
                                        {BuffKind::AntiArmor, "ANTI_ARMOR"},
                                        {BuffKind::Armor, "ARMOR"},
                                        {BuffKind::Reflect, "REFLECT"},
                                        {BuffKind::Dodge, "DODGE"},
                                        {BuffKind::Knife, "KNIFE"},
                                        {BuffKind::Gravity, "GRAVITY"},
                                        {BuffKind::BlackOut, "BLACK_OUT"},
                                        {BuffKind::SpeedUp, "SPEED_UP"},
                                        {BuffKind::Flash, "FLASH"},
                                        {BuffKind::Destroy, "DESTROY"},
                                        {BuffKind::Construct, "CONSTRUCT"},
                                        {BuffKind::Trap, "TRAP"},
                                        {BuffKind::Missile, "MISSILE"},
                                        {BuffKind::Kamui, "KAMUI"}});

struct Message {
  nlohmann::json msg;
  Message() = default;
  explicit Message(const std::string& msg_str)
      : msg(nlohmann::json::parse(msg_str)) {}
  [[nodiscard]] auto to_string() const -> std::string { return msg.dump(); }
};

struct PerformMove : public Message {
  PerformMove(const std::string& token, const std::string& direction) {
    msg["messageType"] = "PERFORM_MOVE";
    msg["token"] = token;
    msg["direction"] = direction;
  }
};

struct PerformTurn : public Message {
  PerformTurn(const std::string& token, const std::string& direction) {
    msg["messageType"] = "PERFORM_TURN";
    msg["token"] = token;
    msg["direction"] = direction;
  }
};

struct PerformAttack : public Message {
  explicit PerformAttack(const std::string& token) {
    msg["messageType"] = "PERFORM_ATTACK";
    msg["token"] = token;
  }
};

struct PerformSkill : public Message {
  PerformSkill(const std::string& token, const SkillKind& skillName) {
    msg["messageType"] = "PERFORM_SKILL";
    msg["token"] = token;
    msg["skillName"] = skillName;
  }
};

struct PerformSelect : public Message {
  PerformSelect(const std::string& token, const BuffKind& buffName) {
    msg["messageType"] = "PERFORM_SELECT";
    msg["token"] = token;
    msg["buffName"] = buffName;
  }
};

struct GetPlayerInfo : public Message {
  GetPlayerInfo(const std::string& token, const std::string& request) {
    msg["messageType"] = "GET_PLAYER_INFO";
    msg["token"] = token;
    msg["request"] = request;
  }
};

struct GetEnvironmentInfo : public Message {
  explicit GetEnvironmentInfo(const std::string& token) {
    msg["messageType"] = "GET_ENVIRONMENT_INFO";
    msg["token"] = token;
  }
};

struct GetGameStatistics : public Message {
  explicit GetGameStatistics(const std::string& token) {
    msg["messageType"] = "GET_GAME_STATISTICS";
    msg["token"] = token;
  }
};

struct GetAvailableBuffs : public Message {
  explicit GetAvailableBuffs(const std::string& token) {
    msg["messageType"] = "GET_AVAILABLE_BUFFS";
    msg["token"] = token;
  }
};

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_MESSAGE_HPP_