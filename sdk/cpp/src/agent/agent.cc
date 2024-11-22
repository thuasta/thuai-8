#include "agent.hpp"

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>
#include <hv/hloop.h>
#include <spdlog/spdlog.h>

#include <cstdint>
#include <memory>
#include <nlohmann/json.hpp>
#include <optional>
#include <string>
#include <string_view>
#include <utility>
#include <vector>

#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "format.hpp"
#include "game_statistics.hpp"
#include "message.hpp"
#include "player_info.hpp"

constexpr std::uint8_t kMinDelayMs{10};
constexpr std::uint16_t kReserveSize{500};

namespace thuai8_agent {

Agent::Agent(std::string_view token, const hv::EventLoopPtr& event_loop,
             int loop_interval_ms)
    : token_(token), event_loop_(event_loop) {
  timer_id_ = event_loop->setInterval(loop_interval_ms,
                                      [this](hv::TimerID) { Loop(); });

  ws_client_ = std::make_unique<hv::WebSocketClient>(event_loop);

  reconn_setting_t reconn_setting;
  reconn_setting.delay_policy = 0;
  reconn_setting.min_delay = kMinDelayMs;
  ws_client_->setReconnect(&reconn_setting);

  ws_client_->onmessage = [this](std::string_view msg) {
    OnMessage(Message(msg));
  };
}

void Agent::MoveForward() const {
  spdlog::debug("{}.MoveForward", *this);
  ws_client_->send(PerformMove(token_, "FORTH").to_string());
}

void Agent::MoveBackward() const {
  spdlog::debug("{}.MoveBackward", *this);
  ws_client_->send(PerformMove(token_, "BACK").to_string());
}

void Agent::TurnClockwise() const {
  spdlog::debug("{}.TurnClockwise", *this);
  ws_client_->send(PerformMove(token_, "CLOCKWISE").to_string());
}

void Agent::TurnCounterClockwise() const {
  spdlog::debug("{}.TurnCounterClockwise", *this);
  ws_client_->send(PerformMove(token_, "COUNTER_CLOCKWISE").to_string());
}

void Agent::Attack() const {
  spdlog::debug("{}.Attack", *this);
  ws_client_->send(PerformAttack(token_).to_string());
}

void Agent::UseSkill(SkillKind skill) const {
  spdlog::debug("{}.UseSkill({})", *this, skill);
  ws_client_->send(PerformSkill(token_, skill).to_string());
}

void Agent::SelectBuff(BuffKind buff) const {
  spdlog::debug("{}.SelectBuff({})", *this, buff);
  ws_client_->send(PerformSelect(token_, buff).to_string());
}

void Agent::Loop() {
  if (!IsConnected()) {
    return;
  }
  ws_client_->send(GetPlayerInfo(token_, "SELF").to_string());
  ws_client_->send(GetPlayerInfo(token_, "OPPONENT").to_string());
  ws_client_->send(GetGameStatistics(token_).to_string());
  ws_client_->send(GetEnvironmentInfo(token_).to_string());
  ws_client_->send(GetAvailableBuffs(token_).to_string());
}

void Agent::OnMessage(const Message& message) {
  const auto& msg_dict = message.msg;
  auto msg_type = msg_dict["messageType"].get<std::string>();
  if (msg_type == "PLAYER_INFO") {
    const auto& weapon_data{msg_dict["weapon"]};
    Weapon weapon{
        .isLaser = weapon_data["isLaser"].get<bool>(),
        .antiArmor = weapon_data["antiArmor"].get<bool>(),
        .damage = weapon_data["damage"].get<unsigned int>(),
        .maxBullets = weapon_data["maxBullets"].get<unsigned int>(),
        .currentBullets = weapon_data["currentBullets"].get<unsigned int>(),
        .attackSpeed = weapon_data["attackSpeed"].get<double>(),
        .bulletSpeed = weapon_data["bulletSpeed"].get<double>()};
    const auto& armor_data{msg_dict["armor"]};
    Armor armor{.canReflect = armor_data["canReflect"].get<bool>(),
                .gravityField = armor_data["gravityField"].get<bool>(),
                .armorValue = armor_data["armorValue"].get<unsigned int>(),
                .health = armor_data["health"].get<unsigned int>(),
                .dodgeRate = armor_data["dodgeRate"].get<double>(),
                .knife = static_cast<ArmorKnifeState>(
                    armor_data["knife"].get<unsigned int>())};
    std::vector<Skill> skills;
    for (const auto& skill_data : msg_dict["skills"]) {
      skills.emplace_back(Skill{
          .name = skill_data["name"].get<SkillKind>(),
          .maxCoolDown = skill_data["maxCoolDown"].get<unsigned int>(),
          .currentCoolDown = skill_data["currentCoolDown"].get<unsigned int>(),
          .isActive = skill_data["isActive"].get<bool>()});
    }
    if (auto token = msg_dict["token"].get<std::string>(); token == token_) {
      self_info_ = PlayerInfo{
          .token = token,
          .position = {.x = msg_dict["position"]["x"].get<double>(),
                       .y = msg_dict["position"]["y"].get<double>(),
                       .angle = msg_dict["position"]["angle"].get<double>()},
          .weapon = weapon,
          .armor = armor,
          .skills = std::move(skills)};
    } else {
      opponent_info_ = PlayerInfo{
          .token = token,
          .position = {.x = msg_dict["position"]["x"].get<double>(),
                       .y = msg_dict["position"]["y"].get<double>(),
                       .angle = msg_dict["position"]["angle"].get<double>()},
          .weapon = weapon,
          .armor = armor,
          .skills = std::move(skills)};
    }
  } else if (msg_type == "ENVIRONMENT_INFO") {
    std::vector<Wall> walls;
    walls.reserve(kReserveSize);
    for (const auto& wall_data : msg_dict["walls"]) {
      walls.emplace_back(Wall{wall_data["x"].get<int>(),
                              wall_data["y"].get<int>(),
                              wall_data["angle"].get<int>()});
    }
    std::vector<Fence> fences;
    for (const auto& fence_data : msg_dict["fences"]) {
      fences.emplace_back(Fence{
          .position = {.x = fence_data["position"]["x"].get<int>(),
                       .y = fence_data["position"]["y"].get<int>(),
                       .angle = fence_data["position"]["angle"].get<int>()},
          .health = fence_data["health"].get<unsigned int>()});
    }
    std::vector<Bullet> bullets;
    for (const auto& bullet_data : msg_dict["bullets"]) {
      bullets.emplace_back(Bullet{
          .position = {.x = bullet_data["position"]["x"].get<double>(),
                       .y = bullet_data["position"]["y"].get<double>(),
                       .angle = bullet_data["position"]["angle"].get<double>()},
          .speed = bullet_data["speed"].get<double>(),
          .damage = bullet_data["damage"].get<double>(),
          .traveledDistance = bullet_data["traveledDistance"].get<double>()});
    }
    environment_info_ =
        EnvironmentInfo{.mapSize = msg_dict["mapSize"].get<unsigned int>(),
                        .walls = std::move(walls),
                        .fences = std::move(fences),
                        .bullets = std::move(bullets)};
  } else if (msg_type == "GAME_STATISTICS") {
    std::vector<OnesScore> scores;
    scores.reserve(2);
    for (const auto& score_data : msg_dict["scores"]) {
      scores.emplace_back(
          OnesScore{.token = score_data["token"].get<std::string>(),
                    .score = score_data["score"].get<unsigned int>()});
    }
    game_statistics_ =
        GameStatistics{.currentStage = msg_dict["currentStage"].get<Stage>(),
                       .countDown = msg_dict["countDown"].get<unsigned int>(),
                       .ticks = msg_dict["ticks"].get<unsigned int>(),
                       .scores = std::move(scores)};
  } else if (msg_type == "AVAILABLE_BUFFS") {
    if (available_buffs_.has_value()) {
      available_buffs_->clear();
    } else {
      available_buffs_ = std::vector<BuffKind>{};
      available_buffs_->reserve(3);
    }
    for (const auto& buff_data : msg_dict["buffs"]) {
      available_buffs_->emplace_back(buff_data.get<BuffKind>());
    }
  } else if (msg_type == "ERROR") {
    spdlog::error("{} got an error from server: [{}] {}]", *this,
                  msg_dict["errorCode"].get<int>(),
                  msg_dict["message"].get<std::string>());
  }
}

}  // namespace thuai8_agent
