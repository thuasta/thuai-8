#pragma once
#ifndef _THUAI8_AGENT_MESSAGE_HPP_
#define _THUAI8_AGENT_MESSAGE_HPP_

#include <nlohmann/json.hpp>
#include <string>
#include <string_view>

#include "available_buffs.hpp"
#include "game_statistics.hpp"
#include "player_info.hpp"

namespace thuai8_agent {

NLOHMANN_JSON_SERIALIZE_ENUM(ArmorKnifeState,
                             {{ArmorKnifeState::NotOwned, "NOT_OWNED"},
                              {ArmorKnifeState::Available, "AVAILABLE"},
                              {ArmorKnifeState::Active, "ACTIVE"},
                              {ArmorKnifeState::Broken, "BROKEN"}});

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

NLOHMANN_JSON_SERIALIZE_ENUM(Stage, {{Stage::Rest, "REST"},
                                     {Stage::Battle, "BATTLE"},
                                     {Stage::End, "END"}});

// NOLINTBEGIN(misc-non-private-member-variables-in-classes)
struct Message {
  nlohmann::json msg;
  Message() = default;
  explicit Message(std::string_view msg_str)
      : msg(nlohmann::json::parse(msg_str)) {}
  [[nodiscard]] auto to_string() const -> std::string { return msg.dump(); }
};
// NOLINTEND(misc-non-private-member-variables-in-classes)

struct PerformMove : public Message {
  PerformMove(std::string_view token, std::string_view direction) {
    msg = {{"messageType", "PERFORM_MOVE"},
           {"token", token},
           {"direction", direction}};
  }
};

struct PerformTurn : public Message {
  PerformTurn(std::string_view token, std::string_view direction) {
    msg = {{"messageType", "PERFORM_TURN"},
           {"token", token},
           {"direction", direction}};
  }
};

struct PerformAttack : public Message {
  explicit PerformAttack(std::string_view token) {
    msg = {{"messageType", "PERFORM_ATTACK"}, {"token", token}};
  }
};

struct PerformSkill : public Message {
  PerformSkill(std::string_view token, SkillKind skillName) {
    msg = {{"messageType", "PERFORM_SKILL"},
           {"token", token},
           {"skillName", skillName}};
  }
};

struct PerformSelect : public Message {
  PerformSelect(std::string_view token, BuffKind buffName) {
    msg = {{"messageType", "PERFORM_SELECT"},
           {"token", token},
           {"buffName", buffName}};
  }
};

struct GetPlayerInfo : public Message {
  GetPlayerInfo(std::string_view token, std::string_view request) {
    msg = {{"messageType", "GET_PLAYER_INFO"},
           {"token", token},
           {"request", request}};
  }
};

struct GetEnvironmentInfo : public Message {
  explicit GetEnvironmentInfo(std::string_view token) {
    msg = {{"messageType", "GET_ENVIRONMENT_INFO"}, {"token", token}};
  }
};

struct GetGameStatistics : public Message {
  explicit GetGameStatistics(std::string_view token) {
    msg = {{"messageType", "GET_GAME_STATISTICS"}, {"token", token}};
  }
};

struct GetAvailableBuffs : public Message {
  explicit GetAvailableBuffs(std::string_view token) {
    msg = {{"messageType", "GET_AVAILABLE_BUFFS"}, {"token", token}};
  }
};

}  // namespace thuai8_agent

#endif  // _THUAI8_AGENT_MESSAGE_HPP_