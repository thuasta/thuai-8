#include "agent.hpp"

#include <hv/Event.h>
#include <hv/EventLoop.h>
#include <hv/WebSocketClient.h>
#include <hv/hloop.h>
#include <spdlog/spdlog.h>

#include <exception>
#include <functional>
#include <memory>
#include <optional>
#include <span>
#include <string>
#include <string_view>

#include "available_buffs.hpp"
#include "environment_info.hpp"
#include "format.hpp"
#include "game_statistics.hpp"
#include "message.hpp"
#include "player_info.hpp"

namespace thuai8_agent {

Agent::Agent(std::string_view token, const hv::EventLoopPtr& event_loop,
             int loop_interval_ms)
    : token_(token), event_loop_(event_loop) {
  timer_id_ = event_loop->setInterval(loop_interval_ms,
                                      [this](hv::TimerID) { Loop(); });

  ws_client_ = std::make_unique<hv::WebSocketClient>(event_loop);

  reconn_setting_t reconn_setting;
  reconn_setting.delay_policy = 0;
  reconn_setting.max_delay = 0;
  ws_client_->setReconnect(&reconn_setting);

  ws_client_->onmessage = [this](const std::string& msg) {
    OnMessage(Message(msg));
  };
}

Agent::~Agent() { event_loop_->killTimer(timer_id_); }

void Agent::Connect(const std::string& server_address) {
  ws_client_->open(server_address.data());
}

void Agent::Disconnect() { ws_client_->close(); }

auto Agent::IsConnected() const -> bool { return ws_client_->isConnected(); }

auto Agent::IsGameReady() const -> bool {
  return players_.has_value() && game_statistics_.has_value() &&
         environment_info_.has_value() && available_buffs_.has_value();
}

auto Agent::token() const -> std::string { return token_; }

auto Agent::players_info() const -> std::optional<std::span<const PlayerInfo>> {
  return players_;
}

auto Agent::game_statistics() const
    -> std::optional<std::reference_wrapper<const GameStatistics>> {
  return game_statistics_;
}

auto Agent::environment_info() const
    -> std::optional<std::reference_wrapper<const EnvironmentInfo>> {
  return environment_info_;
}

auto Agent::available_buffs() const
    -> std::optional<std::span<const BuffKind>> {
  return available_buffs_;
}

void Agent::MoveForward() {
  spdlog::debug("{}.MoveForward", *this);
  ws_client_->send(PerformMove(token_, "FORTH").to_string());
}

void Agent::MoveBackward() {
  spdlog::debug("{}.MoveBackward", *this);
  ws_client_->send(PerformMove(token_, "BACK").to_string());
}

void Agent::TurnClockwise() {
  spdlog::debug("{}.TurnClockwise", *this);
  ws_client_->send(PerformMove(token_, "CLOCKWISE").to_string());
}

void Agent::TurnCounterClockwise() {
  spdlog::debug("{}.TurnCounterClockwise", *this);
  ws_client_->send(PerformMove(token_, "COUNTER_CLOCKWISE").to_string());
}

void Agent::Attack() {
  spdlog::debug("{}.Attack", *this);
  ws_client_->send(PerformAttack(token_).to_string());
}

void Agent::UseSkill(SkillKind skill) {
  spdlog::debug("{}.UseSkill({})", *this, skill);
  ws_client_->send(PerformSkill(token_, skill).to_string());
}

void Agent::SelectBuff(BuffKind buff) {
  spdlog::debug("{}.SelectBuff({})", *this, buff);
  ws_client_->send(PerformSelect(token_, buff).to_string());
}

void Agent::Loop() {
  try {
    if (!IsConnected()) {
      return;
    }
  } catch (const std::exception& e) {
    spdlog::error("{} encountered an error [{}] in loop", *this, e.what());
  }
}

void Agent::OnMessage(const Message& message) {
  try {
    auto msg_dict = message.msg;
    auto msg_type = msg_dict["messageType"].get<std::string>();
    if (msg_type == "ERROR") {
      spdlog::error("{} got an error from server: [code: {}, msg: {}]", *this,
                    msg_dict["errorCode"].get<int>(),
                    msg_dict["message"].get<std::string>());
    } else if (msg_type == "PLAYER_INFO") {
    }
  } catch (const std::exception& e) {
    spdlog::error("{} encountered an error [{}] in OnMessage", *this, e.what());
  }
}

}  // namespace thuai8_agent